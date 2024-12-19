using Lagrange.XocMat.Attributes;
using Lagrange.XocMat.Configuration;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Disorder;

[ConfigSeries]
public class FollowConfig : JsonConfigBase<FollowConfig>
{
    public class FollowWeapon
    {
        [JsonProperty("时间")]
        public DateTime Time  { get; set;}

        [JsonProperty("道侣")]
        public long Follow { get; set; }

        [JsonProperty("名称")]
        public string WeaponName { get; set;} = string.Empty;
    }

    protected override string Filename => "Follow";

    protected override string? ReloadMsg => "[今日道侣] config reload successfully!\n";

    [JsonProperty("道侣数据")]
    public Dictionary<long, FollowWeapon> TempSave { get; set; } = [];

    public FollowWeapon? GetFollow(long userid)
    {
        if (TempSave.TryGetValue(userid, out var temp))
        {
            return temp;
        }
        return null;
    }

    public void SaveFollow(long userid, long targetid, string targetName)
    {
        TempSave[userid] = new FollowWeapon()
        {
            Follow = targetid,
            WeaponName = targetName,
            Time = DateTime.Now
        };
    }
}
