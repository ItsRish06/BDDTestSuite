using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BDDTestSuite.GenAI.Models
{
    public partial class GeminiResponse
    {
        [JsonPropertyName("candidates")]
        public Candidate[] Candidates { get; set; }

        [JsonPropertyName("usageMetadata")]
        public UsageMetadata UsageMetadata { get; set; }

        [JsonPropertyName("modelVersion")]
        public string ModelVersion { get; set; }

        [JsonPropertyName("responseId")]
        public string ResponseId { get; set; }
    }

    public partial class Candidate
    {
        [JsonPropertyName("content")]
        public Content Content { get; set; }

        [JsonPropertyName("finishReason")]
        public string FinishReason { get; set; }

        [JsonPropertyName("index")]
        public long Index { get; set; }
    }

    public class ResponseContent
    {
        [JsonPropertyName("parts")]
        public Part[] Parts { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }
    }

    public class Part
    {
        [JsonPropertyName("text")]
        public Text[] Text { get; set; }
    }

    public class Text
    {
        [JsonPropertyName("reasoning")]
        public string Reasoning { get; set; }

        [JsonPropertyName("recommendation")]
        public string Recommendation { get; set; }

        [JsonPropertyName("scenario")]
        public string Scenario { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }
    }

    public partial class UsageMetadata
    {
        [JsonPropertyName("promptTokenCount")]
        public long PromptTokenCount { get; set; }

        [JsonPropertyName("candidatesTokenCount")]
        public long CandidatesTokenCount { get; set; }

        [JsonPropertyName("totalTokenCount")]
        public long TotalTokenCount { get; set; }

        [JsonPropertyName("promptTokensDetails")]
        public PromptTokensDetail[] PromptTokensDetails { get; set; }

        [JsonPropertyName("thoughtsTokenCount")]
        public long ThoughtsTokenCount { get; set; }
    }

    public partial class PromptTokensDetail
    {
        [JsonPropertyName("modality")]
        public string Modality { get; set; }

        [JsonPropertyName("tokenCount")]
        public long TokenCount { get; set; }
    }
}
