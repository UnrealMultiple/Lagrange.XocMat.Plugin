using Music.QQ.Internal.Search.Song;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.QQ.Internal.Search;

public class Songs
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("list")]
    public List<SongData> List { get; set; } = [];
}
