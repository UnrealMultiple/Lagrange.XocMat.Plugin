using Newtonsoft.Json;

namespace Music.QQ.Internal.Playlists;

public class Creator
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("musicid")]
    public long Musicid { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("type")]
    public int Type { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("singerid")]
    public int Singerid { get; set; }

    /// <summary>
    /// 人间忧伤
    /// </summary>
    [JsonProperty("nick")]
    public string Nick { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("headurl")]
    public string Headurl { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("ifpicurl")]
    public string Ifpicurl { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("encrypt_uin")]
    public string EncryptUin { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("isVip")]
    public int IsVip { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("ai_uin")]
    public long AiUin { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("encrypt_ai_uin")]
    public string EncryptAiUin { get; set; } = string.Empty;
}