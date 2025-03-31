using Lagrange.XocMat.Attributes;
using Lagrange.XocMat.Configuration;
using Newtonsoft.Json;

namespace Disorder;


public class JrrpContent
{
    [JsonProperty("账号")]
    public long UserId { get; set; }

    [JsonProperty("人品")]
    public byte Value { get; set; }

    [JsonProperty("时间")]
    public DateTime Time { get; set; }
}