using ChatGlm.JContent.Segment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatGlm.JContent;

public class StreamSyncResult
{
    [JsonPropertyName("history_id")]
    public string HistoryId { get; set; } = string.Empty;

    [JsonPropertyName("conversation_id")]
    public string ComversationId { get; set; } = string.Empty;

    [JsonPropertyName("output")]
    public List<Part> Parts { get; set; } = [];

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    public List<T> GetEntity<T>() where T : BaseSegment
    {
        return [.. Parts.SelectMany(p => p.Chain.GetEntity<T>())];
    }
}

public class Part
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty ;

    [JsonPropertyName("content")]
    public MessageChain Chain { get; set; } = [];

    [JsonPropertyName("status")]
    public string Status { get; set; }= string.Empty;

    [JsonPropertyName("created_at")]
    public string Created { get; set; } = string.Empty;

    [JsonPropertyName("meta_data")]
    public Dictionary<string, object> MetaData { get; set; } = [];  
}