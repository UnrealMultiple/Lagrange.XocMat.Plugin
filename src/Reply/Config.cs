using System.Text.RegularExpressions;
using Lagrange.XocMat.Attributes;
using Lagrange.XocMat.Configuration;
using Newtonsoft.Json;

namespace Reply;


[ConfigSeries]
internal class Config : JsonConfigBase<Config>
{
    [JsonProperty("匹配列表")]
    public List<ReplyRule> Rules { get; set; } = [];

    public void RemoveRule(int index)
    {
        if (index < 1 || index > Rules.Count) return;
        Rules.RemoveAt(index - 1);
    }
}
