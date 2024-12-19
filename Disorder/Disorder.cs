using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Lagrange.XocMat.Commands;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Plugin;
using Lagrange.XocMat.Utility;
using Microsoft.Extensions.Logging;
using System.Text.Json.Nodes;

namespace Disorder;

public class Disorder(ILogger logger, CommandManager commandManager, BotContext bot) : XocMatPlugin(logger, commandManager, bot)
{
    public override string Name => "Disorder";

    public override string Description => "提供一些乱七八糟的功能";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 0);

    private const string FellowUrl = "https://oiapi.net/API/CPUni/";

    private const string JrrpUrl = "https://oiapi.net/API/Yun/";

    private const string CosUrl = "https://imgapi.cn/cos2.php?return=jsonpro";

    private readonly static HttpClient client = new();

    public override void Initialize()
    {
        CommandManager.AddGroupCommand(new("今日道侣", Fellow, ""));
        CommandManager.AddGroupCommand(new("jrrp", Jrrp, ""));
        CommandManager.AddGroupCommand(new("cos", Cos, "onebot.cos.use"));
    }

    private async ValueTask Cos(CommandArgs args)
    {
        var res = await client.GetStringAsync(CosUrl);
        var json = JsonNode.Parse(res);
        var arr = json?["imgurls"]?.AsArray();
        if (arr == null)
            return;
        List<MessageChain> chains = [];
        foreach (var img in arr)
        {
            var url = img?.ToString();
            if (url == null) continue;
            chains.Add(MessageBuilder.Friend(args.EventArgs.Chain.GroupMemberInfo!.Uin).Image(HttpUtils.HttpGetByte(url).Result).Build());
        }
        var build = MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value);
        await args.EventArgs.Reply(build.MultiMsg(chains));
    }

    private async ValueTask Jrrp(CommandArgs args)
    {
        var current = JrrpConfig.Instance.GetJrrp(args.EventArgs.Chain.GroupMemberInfo!.Uin);
        byte val = 0;
        if (current != null && current.Time.Date == DateTime.Now.Date)
        {
            val = current.Value;
        }
        else
        {
            val = Convert.ToByte(new Random().Next(0, 100));
            JrrpConfig.Instance.SaveJrrp(args.EventArgs.Chain.GroupMemberInfo!.Uin, val);
            JrrpConfig.Save();
        }
        var buffer = await client.GetByteArrayAsync(JrrpUrl + $"?let={args.EventArgs.Chain.GroupMemberInfo!.Uin}{val}");
        List<MessageChain> chains = [
                MessageBuilder.Friend(args.EventArgs.Chain.GroupMemberInfo!.Uin).Text($"今日运势").Build(),
                MessageBuilder.Friend(args.EventArgs.Chain.GroupMemberInfo!.Uin).Text($"你今日的运势值: {val}").Build(),
                MessageBuilder.Friend(args.EventArgs.Chain.GroupMemberInfo!.Uin).Image(buffer).Build()
            ];
        await args.EventArgs.Reply(MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value).MultiMsg(chains));
    }

    private async ValueTask Fellow(CommandArgs args)
    {
        var w = FollowConfig.Instance.GetFollow(args.EventArgs.Chain.GroupMemberInfo!.Uin);
        long targerid = 0;
        var targetName = string.Empty;
        if (w != null && w.Time.Date == DateTime.Now.Date)
        {
            targerid = w.Follow;
            targetName = w.WeaponName;

        }
        else
        {
            var members = await args.Bot.FetchMembers(args.EventArgs.Chain.GroupUin!.Value);
            var targer = members.OrderBy(x => Guid.NewGuid()).First();
            targerid = targer.Uin;
            targetName = targer.MemberName.Length > 6 ? targer.MemberName[..6] : targer.MemberName;
            FollowConfig.Instance.SaveFollow(args.EventArgs.Chain.GroupMemberInfo!.Uin, targerid, targetName);
            FollowConfig.Save();
        }

        var stream = await client.GetByteArrayAsync(FellowUrl + $"?first={(args.EventArgs.Chain.GroupMemberInfo!.MemberName.Length > 6 ? args.EventArgs.Chain.GroupMemberInfo!.MemberName[..6] : args.EventArgs.Chain.GroupMemberInfo!.MemberCard)}&second={targetName}");
        List<MessageChain> chains = [
                 MessageBuilder.Friend(args.EventArgs.Chain.GroupMemberInfo!.Uin).Text($"今日道侣").Build(),
                 MessageBuilder.Friend(args.EventArgs.Chain.GroupMemberInfo!.Uin).Image(HttpUtils.HttpGetByte($"http://q.qlogo.cn/headimg_dl?dst_uin={targerid}&spec=640&img_type=png").Result).Text($"账号: {targerid}\n昵称: {targetName}").Build(),
                 MessageBuilder.Friend(args.EventArgs.Chain.GroupMemberInfo!.Uin).Image(stream).Build()
            ];
        await args.EventArgs.Reply(MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value).MultiMsg(chains));
    }

    protected override void Dispose(bool dispose)
    {
        CommandManager.GroupCommandDelegate.RemoveAll(x => x.CallBack == Fellow);
        CommandManager.GroupCommandDelegate.RemoveAll(x => x.CallBack == Jrrp);
        CommandManager.GroupCommandDelegate.RemoveAll(x => x.CallBack == Cos);
    }
}
