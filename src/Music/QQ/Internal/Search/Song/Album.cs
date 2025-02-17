using Newtonsoft.Json;

namespace Music.QQ.Internal.Search.Song;

public class Album
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
    /// 七里香
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("pmid")]
    public string Pmid { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("subtitle")]
    public string Subtitle { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("time_public")]
    public string TimePublic { get; set; } = string.Empty;

    /// <summary>
    /// 七里香
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    [JsonIgnore]
    public string Picture => $"https://y.gtimg.cn/music/photo_new/T002R800x800M000{Mid}.jpg";
}
