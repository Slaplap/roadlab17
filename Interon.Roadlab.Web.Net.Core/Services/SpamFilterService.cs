using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Anthropic.SDK;
using Anthropic.SDK.Constants;
using Anthropic.SDK.Messaging;
using Interon.Roadlab.Web.Net.Core.Logging;
using Interon.Roadlab.Web.Net.Core.Models.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Interon.Roadlab.Web.Net.Core.Services
{
    public interface ISpamFilterService
    {
        Task<SpamAnalysisResult> AnalyzeEmailAsync(string subject, string sender, string body, CancellationToken cancellationToken = default);
    }

    public class SpamFilterService : ISpamFilterService
    {
        private readonly AnthropicClient? _client;
        private readonly TokenBucketRateLimiter _rateLimiter;
        private readonly ILogger<SpamFilterService> _logger;
        private readonly string _systemPrompt;

        public SpamFilterService(IConfiguration config, ILogger<SpamFilterService> logger)
        {
            _logger = logger;
            var apiKey = Environment.GetEnvironmentVariable("ROADLAB_ANTHROPIC_API_KEY") ?? config["AnthropicApiKey"];
            if (!string.IsNullOrEmpty(apiKey))
            {
                _client = new AnthropicClient(apiKey);
            }
            else
            {
                _logger.LogWarning("Anthropic API key not configured (neither ROADLAB_ANTHROPIC_API_KEY env var nor AnthropicApiKey config value is set). Spam filtering is disabled; all submissions will be treated as legitimate.");
            }
            _rateLimiter = new TokenBucketRateLimiter(50, 1); // 50 requests per minute
            
            _systemPrompt = @"You are an expert email security analyst specializing in spam detection. 
Analyze the provided email across multiple dimensions:
1. Content authenticity and relevance
2. Sender credibility and reputation  
3. Technical indicators (URLs, attachments, headers)
4. Social engineering patterns
5. Urgency and manipulation tactics
6. Make sure the Location is a geolocation for example a city or province or suburb 
7. MAke sure to return a confidence_score between 0 and 100

Return your analysis as JSON in this exact format:
{
  ""classification"": ""SPAM"" or ""LEGITIMATE"",
  ""confidence_score"": 0-100,
  ""risk_level"": ""HIGH"", ""MEDIUM"", or ""LOW"",
  ""key_indicators"": [""indicator1"", ""indicator2"", ""indicator3""],
  ""reasoning"": ""detailed explanation""
}";
        }

        public async Task<SpamAnalysisResult> AnalyzeEmailAsync(
            string subject,
            string sender,
            string body,
            CancellationToken cancellationToken = default)
        {
            if (_client == null)
            {
                return CreateFallbackResult("Spam filter disabled (no Anthropic API key configured).");
            }

            try
            {
                // Rate limiting check
                if (!await _rateLimiter.TryConsumeAsync())
                {
                    _logger.LogWarning("Rate limit exceeded, waiting before processing spam analysis");
                    await Task.Delay(1000, cancellationToken);
                }

                var userPrompt = $@"Analyze this email for spam:

Subject: {subject}
From: {sender}
Body: {body?.Substring(0, Math.Min(body?.Length ?? 0, 4000))}

Provide your analysis in JSON format.";

                var messages = new List<Message>
                {
                    new Message(RoleType.User, userPrompt)
                };

                var parameters = new MessageParameters
                {
                    Messages = messages,
                    MaxTokens = 300,
                    Model = "claude-haiku-4-5-20251001", // Fastest for classification
                    Temperature = 0.1m, // Low temperature for consistent results
                    System = new List<SystemMessage> { new SystemMessage(_systemPrompt) }
                };

                var response = await _client.Messages.GetClaudeMessageAsync(parameters);
                var responseText = response.Message.ToString();
                
                _logger.LogInformation("Claude spam analysis completed for sender: {Sender}", PiiMasking.MaskEmail(sender));
                
                return ParseSpamAnalysisResult(responseText);
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("429") || ex.Message.Contains("TooManyRequests"))
            {
                _logger.LogWarning("Rate limited by Claude API, implementing exponential backoff");
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, 1)), cancellationToken);
                
                // Return safe fallback result
                return CreateFallbackResult("Rate limit exceeded - treated as legitimate to avoid false positives");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing email for spam. Subject: {Subject}, Sender: {Sender}", subject, PiiMasking.MaskEmail(sender));
                
                // Fallback to basic keyword detection
                return CreateFallbackAnalysis(subject, body);
            }
        }

        private SpamAnalysisResult ParseSpamAnalysisResult(string responseText)
        {
            try
            {
                // Extract JSON from response
                var jsonStart = responseText.IndexOf('{');
                var jsonEnd = responseText.LastIndexOf('}');
                
                if (jsonStart >= 0 && jsonEnd > jsonStart)
                {
                    var jsonText = responseText.Substring(jsonStart, jsonEnd - jsonStart + 1);
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<SpamAnalysisResult>(jsonText, options);
                    
                    if (result != null)
                    {
                        result.AnalyzedAt = DateTime.UtcNow;
                        return result;
                    }
                }
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "Failed to parse Claude response as JSON: {Response}", responseText);
            }

            // Fallback parsing
            return CreateFallbackResult($"Analysis completed but response format was unexpected: {responseText}");
        }

        private SpamAnalysisResult CreateFallbackAnalysis(string subject, string body)
        {
            var spamKeywords = new[] { "urgent", "limited time", "act now", "free money", "viagra", "casino", "lottery", "winner" };
            var foundKeywords = new List<string>();
            var content = $"{subject} {body}".ToLowerInvariant();

            foreach (var keyword in spamKeywords)
            {
                if (content.Contains(keyword))
                {
                    foundKeywords.Add(keyword);
                }
            }

            var isSpam = foundKeywords.Count >= 2;
            
            return new SpamAnalysisResult
            {
                Classification = isSpam ? "SPAM" : "LEGITIMATE",
                ConfidenceScore = isSpam ? Math.Min(foundKeywords.Count * 30, 95) : 70,
                RiskLevel = isSpam ? "MEDIUM" : "LOW",
                KeyIndicators = foundKeywords,
                Reasoning = $"Fallback analysis detected {foundKeywords.Count} spam keywords: {string.Join(", ", foundKeywords)}",
                AnalyzedAt = DateTime.UtcNow
            };
        }

        private SpamAnalysisResult CreateFallbackResult(string reason)
        {
            return new SpamAnalysisResult
            {
                Classification = "LEGITIMATE",
                ConfidenceScore = 50,
                RiskLevel = "LOW",
                KeyIndicators = new List<string>(),
                Reasoning = reason,
                AnalyzedAt = DateTime.UtcNow
            };
        }
    }
}