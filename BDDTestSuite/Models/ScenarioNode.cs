using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BDDTestSuite.Models
{
    public class ScenarioNode
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("steps")]
        public List<StepNode> Steps { get; set; } = [];
    }
}
