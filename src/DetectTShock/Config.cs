using Lagrange.XocMat.Attributes;
using Lagrange.XocMat.Configuration;
using Newtonsoft.Json;

namespace DetectTShock;

[ConfigSeries]
public class Config : JsonConfigBase<Config>
{

    [JsonProperty("文件大小限制")]
    public int FileSizeLimit { get; set; } = 40 * 1024 * 1024;

    [JsonProperty("检测项路径")]
    public string DetectPath { get; set; } = "Detect";

    [JsonProperty("检测程序")]
    public string DetectProgram { get; set; } = "Detect.exe";
}
