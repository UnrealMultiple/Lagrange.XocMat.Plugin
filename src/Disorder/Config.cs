using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lagrange.XocMat.Attributes;
using Lagrange.XocMat.Configuration;
using Newtonsoft.Json;

namespace Disorder;

[ConfigSeries]
public class Config : JsonConfigBase<Config>
{
    [JsonProperty("Cosplay撤回时间")]
    public int CosplayRecallTime { get; set; } = 20;

    [JsonProperty("道侣数据")]
    public Dictionary<long, FollowContent> TempSave { get; set; } = [];

    [JsonProperty("人品数据")]
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

    public FollowContent? GetFollow(long userid)
    {
        if (TempSave.TryGetValue(userid, out var temp))
        {
            return temp;
        }
        return null;
    }

    public void SaveFollow(long userid, long targetid, string targetName)
    {
        TempSave[userid] = new FollowContent()
        {
            Follow = targetid,
            WeaponName = targetName,
            Time = DateTime.Now
        };
    }
}
