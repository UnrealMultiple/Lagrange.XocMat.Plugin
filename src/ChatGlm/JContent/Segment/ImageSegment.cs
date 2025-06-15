using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatGlm.JContent.Segment;

[SegmentType("image")]
public class ImageSegment : BaseSegment
{
    [JsonPropertyName("image")]
    public List<ImageContent> ImageContents { get; set; } = [];
}

public class ImageContent
{
    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; } = string.Empty;
}
