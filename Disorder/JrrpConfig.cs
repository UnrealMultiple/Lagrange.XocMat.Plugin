using Lagrange.XocMat.Attributes;
using Lagrange.XocMat.Configuration;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Disorder;

[ConfigSeries]
public class JrrpConfig : JsonConfigBase<JrrpConfig>
{
    public class JrrpContent
    {
        [JsonProperty("账号")]
        public long UserId { get; set; }

        [JsonProperty("人品")]
        public byte Value { get; set; }

        [JsonProperty("时间")]
        public DateTime Time { get; set; }
    }

    protected override string Filename => "Jrrp";

    protected override string? ReloadMsg => "[jrrp] config reload successfully!\n";

    public Dictionary<long, JrrpContent> Jrrps { get; set; } = [];

    public JrrpContent? GetJrrp(long userid)
    {
        if (Jrrps.TryGetValue(userid, out var temp))
        {
            return temp;
        }
        return null;
    }

    public void SaveJrrp(long userid, byte jrrp)
    {
        Jrrps[userid] = new JrrpContent()
        {
            Value = jrrp,
            UserId = userid,
            Time = DateTime.Now
        };
    }
}
