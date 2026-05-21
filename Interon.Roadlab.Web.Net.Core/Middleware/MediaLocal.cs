using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

public class MediaFileMiddleware
{
    private const string DefaultRemoteMediaUrl = "https://roadlabstaging.azurewebsites.net/media/";

    private readonly RequestDelegate _next;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;
    private readonly ILogger<MediaFileMiddleware> _logger;

    public MediaFileMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory, IConfiguration config, ILogger<MediaFileMiddleware> logger)
    {
        _next = next;
        _httpClientFactory = httpClientFactory;
        _config = config;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/media") && _config.GetValue<bool>("Custom:LoadMediaFromStaging"))
        {
            var httpClient = _httpClientFactory.CreateClient();
            var filePath = context.Request.Path.ToString();
            var remoteBase = _config["Custom:RemoteMediaUrl"];
            if (string.IsNullOrWhiteSpace(remoteBase)) remoteBase = DefaultRemoteMediaUrl;
            if (!remoteBase.EndsWith('/')) remoteBase += "/";
            var requestUrl = remoteBase + filePath.Substring(7); // strip leading "/media/"

            try
            {
                var response = await httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    context.Response.ContentType = response.Content.Headers.ContentType.ToString();
                    await response.Content.CopyToAsync(context.Response.Body);
                    return;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Remote media proxy failed for {RequestUrl}; falling through to local pipeline.", requestUrl);
            }
        }

        // If not a media file request or the download failed, continue down the middleware pipeline
        await _next(context);
    }
}
