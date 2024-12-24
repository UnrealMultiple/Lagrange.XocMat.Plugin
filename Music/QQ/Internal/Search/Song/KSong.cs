using Newtonsoft.Json;

namespace Music.QQ.Internal.Search.Song;

public class KSong
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
}
