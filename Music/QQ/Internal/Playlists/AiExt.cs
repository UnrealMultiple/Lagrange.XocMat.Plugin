using Newtonsoft.Json;

namespace Music.QQ.Internal.Playlists;

public class AiExt
{
    /// <summary>
    /// 
    /// </summary>
    public int CountdownTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool ISJoinExp { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("blkCntDnlist")]
    public List<string> BlkCntDnlist { get; set; } = [];
}