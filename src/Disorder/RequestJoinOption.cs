using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Disorder;

public class RequestJoinOption
{
    [JsonProperty("群号")]
    public uint GroupUin { get; set; }

    [JsonProperty("启用")]
    public uint Enable { get; set;}

    [JsonProperty("答案")]
    public string Answer { get; set; } = string.Empty;
}
