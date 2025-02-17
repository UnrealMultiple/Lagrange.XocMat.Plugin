using Music.QQ.Internal.Search.Song;
using Newtonsoft.Json;

namespace Music.QQ.Internal.Search;

public class Songs
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("list")]
    public List<SongData> List { get; set; } = [];
}
