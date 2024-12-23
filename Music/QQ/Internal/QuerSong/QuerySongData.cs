using Music.QQ.Internal.Search.Song;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.QQ.Internal.QuerSong;

public class QuerySongData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("tracks")]
    public List<SongData> Tracks { get; set; } = [];
}
