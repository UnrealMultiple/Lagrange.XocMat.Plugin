
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Music.QQ;

public class ApiRespone
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; set; }


    [JsonProperty("message")]
    public string Message { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("data")]
    public JsonNode Data { get; set; }
}
