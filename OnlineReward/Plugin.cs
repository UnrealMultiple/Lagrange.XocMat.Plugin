using Lagrange.Core;
using Lagrange.XocMat.Commands;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.DB.Manager;
using Lagrange.XocMat.Event;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Permission;
using Lagrange.XocMat.Plugin;
using Microsoft.Extensions.Logging;
using System.Text;

namespace OnlineReward;

public class Plugin(ILogger logger, CommandManager commandManager, BotContext bot) : XocMatPlugin(logger, commandManager, bot)
{
    public override string Name => "OnlineReward";

    public override string Description => "领取在线时长奖励";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 0);

    public override void Initialize()
    {
        CommandManager.AddGroupCommand(new("领取在线奖励", CReward, OneBotPermissions.OnlineRank));
        OperatHandler.OnCommand += OperatHandler_OnCommand;
    }


    private async ValueTask OperatHandler_OnCommand(CommandArgs args)
    {
        if (args.Name == "泰拉服务器重置")
        {
            Config.Instance.Reset();
        }
        await Task.CompletedTask;
    }

    private async ValueTask CReward(CommandArgs args)
    {
        if (UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) && server != null)
        {
            var user = TerrariaUser.GetUserById(args.EventArgs.Chain.GroupMemberInfo!.Uin, server.Name);
            if (user.Count == 0)
            {
                await args.EventArgs.Reply("未找到注册的账户", true);
                return;
            }
            var online = await server.OnlineRank();
            if (!online.Status)
            {
                await args.EventArgs.Reply(online.Message, true);
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
                        Currency.Add(args.EventArgs.Chain.GroupUin!.Value, args.EventArgs.Chain.GroupMemberInfo!.Uin, ntime * Config.Instance.TimeRate);
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
            await args.EventArgs.Reply(sb.ToString().Trim());
            Config.Save();
        }
        else
        {
            await args.EventArgs.Reply("请切换至一个有效的服务器!", true);
            return;
        }
    }

    protected override void Dispose(bool dispose)
    {
        CommandManager.GroupCommandDelegate.RemoveAll(x => x.CallBack == CReward);
        OperatHandler.OnCommand -= OperatHandler_OnCommand;
    }
}
