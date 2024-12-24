using Music.QQ.Internal.Search.Song;
using Newtonsoft.Json;

namespace Music.QQ.Internal.QuerSong;

public class QuerySongData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("tracks")]
    public List<SongData> Tracks { get; set; } = [];
}
