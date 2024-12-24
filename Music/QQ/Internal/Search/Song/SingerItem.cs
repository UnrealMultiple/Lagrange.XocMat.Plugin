using Newtonsoft.Json;

namespace Music.QQ.Internal.Search.Song;

public class SingerItem
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("mid")]
    public string Mid { get; set; } = string.Empty;

    /// <summary>
    /// 周杰伦
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("pmid")]
    public string Pmid { get; set; } = string.Empty;

    /// <summary>
    /// 周杰伦
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("type")]
    public int Type { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("uin")]
    public long Uin { get; set; }
}
