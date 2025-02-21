using Lagrange.XocMat.Configuration;
using Newtonsoft.Json;

namespace DeepSeek;

[Lagrange.XocMat.Attributes.ConfigSeries]
public class Config : JsonConfigBase<Config>
{
    [JsonProperty("使用上下文")]
    public bool UseContext { get; set; } = false;

    [JsonProperty("API_KEY")]
    public string APIKey { get; set; } = string.Empty;

    [JsonProperty("设定")]
    public string SystemMessage { get; set; } = "你是Terraria专家!";
}
