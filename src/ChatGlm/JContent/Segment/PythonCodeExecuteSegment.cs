using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatGlm.JContent.Segment;

[SegmentType("code")]
public class PythonCodeExecuteSegment : BaseSegment
{
    [JsonPropertyName("code")]
    public string Content { get; set; } = string.Empty;
}
