
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Music.QQ;

public class ApiRespone
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; set; }


    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("data")]
    public JObject? Data { get; set; }
}
