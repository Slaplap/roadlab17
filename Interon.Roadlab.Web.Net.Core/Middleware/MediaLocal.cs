using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;

public class MediaFileMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _remoteServerUrl = "https://roadlabstaging.azurewebsites.net/media/";

    public MediaFileMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory)
    {
        _next = next;
        _httpClientFactory = httpClientFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/media"))
        {
            var httpClient = _httpClientFactory.CreateClient();
            var filePath = context.Request.Path.ToString();
            var requestUrl = _remoteServerUrl + filePath.Substring(7); // Adjust substring based on your path structure

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
            catch (HttpRequestException)
            {
                // Handle errors or log them
            }
        }

        // If not a media file request or the download failed, continue down the middleware pipeline
        await _next(context);
    }
}