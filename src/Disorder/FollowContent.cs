using Lagrange.XocMat.Attributes;
using Lagrange.XocMat.Configuration;
using Newtonsoft.Json;

namespace Disorder;

public class FollowContent
{
    [JsonProperty("时间")]
    public DateTime Time { get; set; }

    [JsonProperty("道侣")]
    public long Follow { get; set; }

    [JsonProperty("名称")]
    public string WeaponName { get; set; } = string.Empty;

    
}
