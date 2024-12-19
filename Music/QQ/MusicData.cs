

using Newtonsoft.Json;

namespace Music.QQ;


public class MusicData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("music")]
    public string Music { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("size")]
    public string Size { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("mid")]
    public string Mid { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("songid")]
    public int Songid { get; set; }

    /// <summary>
    /// 稻香
    /// </summary>
    [JsonProperty("song")]
    public string Song { get; set; } = string.Empty;

    /// <summary>
    /// 周杰伦
    /// </summary>
    [JsonProperty("singer")]
    public string Singer { get; set; } = string.Empty;

    /// <summary>
    /// 魔杰座
    /// </summary>
    [JsonProperty("album")]
    public string Album { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("singerList")]
    public List<string> SingerList { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("picture")]
    public string Picture { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("pay")]
    public bool Pay { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("interval")]
    public int Interval { get; set; }
}