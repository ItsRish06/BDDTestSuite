using BDDTestSuite.GenAI;
using BDDTestSuite.GenAI.Models;
using OpenQA.Selenium.DevTools.V134.Network;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace BDDTestSuite.GenAi
{
    public static class Gemini
    {

        public static string GenerateSummary(string apiKey, GeminiRequest geminiRequest, string model = "gemini-2.5-flash")
        {
            var client = new RestClient("https://generativelanguage.googleapis.com/");

            var request = new RestRequest($"v1beta/models/{model}:generateContent", Method.Post)
                .AddHeader("x-goog-api-key", apiKey)
                .AddJsonBody(BuildRequest(geminiRequest));

            var response = client.Execute<GeminiResponse>(request, Method.Post);

            if (!response.IsSuccessful)
                throw response?.ErrorException ?? new Exception("Gemini api request failed");

            return response.Data.Candidates.First().Content.Parts.First().Text;
        }
        
        private static string BuildRequest(GeminiRequest geminiRequest)
        {

            var systemInstructionTxt = File.ReadAllText(
                Path.Combine(Environment.CurrentDirectory, "GenAI", "SystemInstruction.txt")
            );

            geminiRequest.AddSystemInstructionText(systemInstructionTxt);

            var configPath = Path.Combine(Environment.CurrentDirectory, "GenAI", "ResponseGenerationConfig.json");
            var jsonContent = File.ReadAllText(configPath);
            var responseGenerationConfig = JsonNode.Parse(jsonContent);

            var requestBody = new RequestBody()
            {
                SystemInstruction = new SystemInstruction()
                {
                    Parts = geminiRequest.systemParts
                },
                Contents =
                [
                    new Content()
                    {
                        Role = "user",
                        Parts = geminiRequest.contentParts
                    }
                ],
                GenerationConfig = responseGenerationConfig
            };

            var options = new JsonSerializerOptions()
            {
                WriteIndented = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            return JsonSerializer.Serialize(requestBody, options);
        }

    }
}
