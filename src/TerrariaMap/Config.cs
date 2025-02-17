using Lagrange.XocMat.Attributes;
using Lagrange.XocMat.Configuration;
using Newtonsoft.Json;

namespace TerrariaMap;

[ConfigSeries]
public class Config : JsonConfigBase<Config>
{
    protected override string Filename => "TerrariaMap";


    [JsonProperty("程序路径")]
    public string AppPath { get; set; } = string.Empty;
}
