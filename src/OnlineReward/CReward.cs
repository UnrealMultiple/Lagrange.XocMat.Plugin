using System.Text;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.DB.Manager;
using Lagrange.XocMat.Extensions;
using Microsoft.Extensions.Logging;

namespace OnlineReward;

public class CReward : Command
{
    public override string HelpText => "领取在线时长奖励";
    public override string[] Alias => ["领取在线奖励"];
    public override string[] Permissions => ["onebot.online.reward"];

    public override async Task InvokeAsync(GroupCommandArgs args, ILogger log)
    {
        if (UserLocation.Instance.TryGetServer(args.MemberUin, args.GroupUin, out var server) && server != null)
        {
            var user = TerrariaUser.GetUserById(args.MemberUin, server.Name);
            if (user.Count == 0)
            {
                await args.Event.Reply("未找到注册的账户", true);
                return;
            }
            var online = await server.OnlineRank();
            if (!online.Status)
            {
                await args.Event.Reply(online.Message, true);
                return;
            }
            var sb = new StringBuilder();
            foreach (var u in user)
            {
                if (online.OnlineRank.TryGetValue(u.Name, out var time))
                {
                    Config.Instance.Reward.TryGetValue(u.Name, out int ctime);
                    var ntime = time - ctime;
                    if (ntime > 0)
                    {
                        Config.Instance.Reward[u.Name] = time;
                        sb.AppendLine($"角色: {u.Name}在线时长{time}秒,本次领取{ntime}秒奖励，共{ntime * Config.Instance.TimeRate}个星币!");
                        Currency.Add(args.MemberUin, ntime * Config.Instance.TimeRate);
                    }
                    else
                    {
                        sb.AppendLine($"角色: {u.Name}因在线时长不足无法领取");
                    }
                }
                else
                {
                    sb.AppendLine($"角色: {u.Name}因在线时长不足无法领取");
                }
            }
            await args.Event.Reply(sb.ToString().Trim());
            Config.Save();
        }
        else
        {
            await args.Event.Reply("请切换至一个有效的服务器!", true);
            return;
        }
    }
}
