using Newtonsoft.Json;

namespace Music.QQ.Internal.Playlists;

public class SongtagItem
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// 昨日热播
    /// </summary>
    [JsonProperty("tag")]
    public string Tag { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("link")]
    public string Link { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("tagid")]
    public int Tagid { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("from_type")]
    public int FromType { get; set; }
}