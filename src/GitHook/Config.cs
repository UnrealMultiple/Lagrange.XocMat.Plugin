using Lagrange.XocMat.Attributes;
using Lagrange.XocMat.Configuration;
using Newtonsoft.Json;

namespace GitHook;

[ConfigSeries]
public class Config : JsonConfigBase<Config>
{
    [JsonProperty("启用")]
    public bool Enable { get; set; } = true;

    [JsonProperty("路由")]
    public string Path { get; set; } = "/update/";

    [JsonProperty("端口")]
    public int Port { get; set; } = 7000;

    [JsonProperty("私人令牌")]
    public string Token { get; set; } = string.Empty;

    [JsonProperty("通知群")]
    public uint[] Groups { get; set; } = Array.Empty<uint>();
}
