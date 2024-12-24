using Newtonsoft.Json;

namespace Music.QQ.Internal.Search.Song;

public class Volume
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("gain")]
    public double Gain { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("lra")]
    public double Lra { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("peak")]
    public int Peak { get; set; }
}
