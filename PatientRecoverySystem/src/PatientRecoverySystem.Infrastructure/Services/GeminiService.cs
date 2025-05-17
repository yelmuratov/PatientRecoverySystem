
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PatientRecoverySystem.Application.Interfaces;
using PatientRecoverySystem.Application.DTOs;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public GeminiService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    public async Task<string> GenerateAdviceAsync(string symptom)
    {
        var apiKey = _config["Gemini:ApiKey"];
        var requestBody = new
        {
            contents = new[] {
                new {
                    parts = new[] {
                        new { text = $"Patient symptom: {symptom}. Give safe advice only." }
                    }
                }
            }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}")
        {
            Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
        };

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<GeminiResponse>();
        return result?.Candidates?[0]?.Content?.Parts?[0]?.Text ?? "Unable to generate advice.";
    }
}
