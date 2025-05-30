﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;

public abstract class ApiIntegrationTestBase : IDisposable
{
    protected readonly HttpClient _client;
    protected readonly IConfiguration _configuration;
    protected const string DRIVER_API_BASE = "/api/Driver";

    public ApiIntegrationTestBase(IConfiguration configuration)
    {
        _configuration = configuration;
        _client = new CustomWebApplicationFactory(configuration)
            .CreateClient();
    }

    public void Dispose() => _client.Dispose();

    protected HttpRequestMessage SetContent(HttpRequestMessage request, object content)
    {
        var json = JsonConvert.SerializeObject(content);
        request.Content = new StringContent(json);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return request;
    }

    protected async Task<T> HttpClientSendRequest<T>(HttpRequestMessage request)
    {
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(jsonString);
    }
}
