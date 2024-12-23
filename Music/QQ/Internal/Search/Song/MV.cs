using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.QQ.Internal.Search.Song;

public class MV
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("list")]
    public List<string> List { get; set; } = [];
}