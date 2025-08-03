using BDDTestSuite.GenAi;
using BDDTestSuite.GenAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BDDTestSuite.GenAI
{
    public class GeminiRequest
    {
        public List<ContentPart> contentParts = [];
        public List<ContentPart> systemParts = [];

        public GeminiRequest AddContentText(string text)
        {
            contentParts.Add(AddPart(text));
            return this;
        }

        public GeminiRequest AddContentData(string dataType, string base64)
        {
            contentParts.Add(AddPart(dataType, base64));
            return this;
        }

        public GeminiRequest AddSystemInstructionText(string text)
        {
            systemParts.Add(AddPart(text));
            return this;
        }

        public GeminiRequest AddSystemInstructionData(string dataType, string base64)
        {
            systemParts.Add(AddPart(dataType, base64));
            return this;
        }

        private ContentPart AddPart(string text)
        {
            return new ContentPart()
            {
                Text = text,
            };
        }
        private ContentPart AddPart(string dataType, string base64)
        {
            return new ContentPart()
            {
                InlineData = new InlineData() 
                {
                    MimeType = dataType,
                    Data = base64
                }
            };
        }
    }
}
