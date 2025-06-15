using System.Text.Json.Serialization;

namespace ChatGlm.JContent.Segment;

[SegmentType("system_error")]
public class PythonSywErrorSegment : BaseSegment
{
    [JsonPropertyName("content")]
    public string Error { get; set; } = string.Empty;
}
