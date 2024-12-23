using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.QQ.Internal.Search.Song;

public class Hotness
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("desc")]
    public string Desc { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("icon_url")]
    public string IconUrl { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("jump_type")]
    public int JumpType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("jump_url")]
    public string JumpUrl { get; set; } = string.Empty;
}
