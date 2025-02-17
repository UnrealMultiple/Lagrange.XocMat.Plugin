using Newtonsoft.Json;

namespace Music.QQ.Internal.Search;

public class Body
{
    ///// <summary>
    ///// 
    ///// </summary>
    //[JsonProperty("album")]
    //public Album Album { get; set; }

    ///// <summary>
    ///// 
    ///// </summary>
    //[JsonProperty("gedantip")]
    //public Gedantip Gedantip { get; set; }

    ///// <summary>
    ///// 
    ///// </summary>
    //[JsonProperty("mv")]
    //public Mv Mv { get; set; }

    ///// <summary>
    ///// 
    ///// </summary>
    //[JsonProperty("qc")]
    //public List<string> Qc { get; set; }

    ///// <summary>
    ///// 
    ///// </summary>
    //[JsonProperty("singer")]
    //public Singer Singer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("song")]
    public Songs Songs { get; set; } = new();

    /// <summary>
    /// 
    ///// </summary>
    //[JsonProperty("songlist")]
    //public Songlist Songlist { get; set; }

    ///// <summary>
    ///// 
    ///// </summary>
    //[JsonProperty("user")]
    //public User User { get; set; }

    ///// <summary>
    ///// 
    ///// </summary>
    //[JsonProperty("zhida")]
    //public Zhida Zhida { get; set; }
}
