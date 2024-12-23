using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Music.QQ.Internal;

public class Response
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("ts")]
    public long Ts { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("start_ts")]
    public long StartTs { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("traceid")]
    public string Traceid { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("req")]
    public Req Req { get; set; } = new();
}
