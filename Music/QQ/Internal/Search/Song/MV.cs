using Newtonsoft.Json;

namespace Music.QQ.Internal.Search.Song;

public class MV
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("list")]
    public List<string> List { get; set; } = [];
}