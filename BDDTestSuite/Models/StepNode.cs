using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BDDTestSuite.Models
{
    public class StepNode
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("logs")]
        public List<string> Logs { get; set; } = [];
        [JsonPropertyName("status")]
        public string? Status { get; set; }
        [JsonPropertyName("screenshot")]
        public string? Screenshot { get; set; }
        [JsonPropertyName("exception_message")]
        public string? ExceptionMessage { get; set; }
    }
}
