using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatGlm.JContent.Segment;

[SegmentType("text")]
public class TextSegment : BaseSegment
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}
