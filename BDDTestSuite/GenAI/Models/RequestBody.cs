using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BDDTestSuite.GenAI.Models
{
    public class RequestBody
    {
        [JsonPropertyName("systemInstruction")]
        public SystemInstruction SystemInstruction { get; set; }

        [JsonPropertyName("contents")]
        public List<Content> Contents { get; set; }

        [JsonPropertyName("generationConfig")]
        public JsonNode GenerationConfig { get; set; }
    }

    public class Content
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("parts")]
        public List<ContentPart> Parts { get; set; }
    }

    public class ContentPart
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("inline_data")]
        public InlineData InlineData { get; set; }
    }

    public class InlineData
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("data")]
        public string Data { get; set; }
    }

    public class SystemInstruction
    {
        [JsonPropertyName("parts")]
        public List<ContentPart> Parts { get; set; }
    }

}
