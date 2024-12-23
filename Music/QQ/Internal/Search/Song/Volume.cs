using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
