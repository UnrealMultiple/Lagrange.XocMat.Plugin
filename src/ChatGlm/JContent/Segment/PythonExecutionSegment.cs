using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatGlm.JContent.Segment;

[SegmentType("execution_ouput")]
public class PythonExecutionSegment : BaseSegment
{
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}
