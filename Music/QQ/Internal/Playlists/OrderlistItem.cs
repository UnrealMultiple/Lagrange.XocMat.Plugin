using Newtonsoft.Json;

namespace Music.QQ.Internal.Playlists;

public class OrderlistItem
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("musicid")]
    public int Musicid { get; set; }

    /// <summary>
    /// 叾屾
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
    [JsonProperty("encrypt_uin")]
    public string EncryptUin { get; set; } = string.Empty;
}