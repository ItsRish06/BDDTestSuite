using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BDDTestSuite.Models
{
    public class FeatureNode
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("scenarios")]
        public List<ScenarioNode>? Scenarios { get; set; } = [];
    }
}
