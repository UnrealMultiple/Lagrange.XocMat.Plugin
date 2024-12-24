using Lagrange.XocMat.Attributes;
using Lagrange.XocMat.Configuration;
using Newtonsoft.Json;

namespace OnlineReward;

[ConfigSeries]
public class Config : JsonConfigBase<Config>
{
    protected override string Filename => "OnlineReward";

    protected override string? ReloadMsg => "[OnlineReward] config reload successfully!\n";

    [JsonProperty("领取比例")]
    public int TimeRate { get; set; } = 100;

    [JsonProperty("领取记录")]
    public Dictionary<string, int> Reward { get; set; } = [];

    public void Reset()
    {
        Reward.Clear();
        Save();
    }

}
