using Lagrange.XocMat.Attributes;
using Lagrange.XocMat.Configuration;
using Newtonsoft.Json;

namespace Music;

[ConfigSeries]
public class Config : JsonConfigBase<Config>
{
    protected override string Filename => "Music";

    protected override string? ReloadMsg => "[Music] config reload successfully!\n";

    [JsonProperty("访问Key")]
    public string Key { get; set; } = string.Empty;
}
