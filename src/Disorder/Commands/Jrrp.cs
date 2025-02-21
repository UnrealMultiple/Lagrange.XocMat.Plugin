using Lagrange.Core.Message;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Utility;

namespace Disorder.Commands;

public class Jrrp : Command
{
    public override string[] Alias => ["今日人品", "jrrp"];
    public override string HelpText => "测试今日运气如何";
    public override string[] Permissions => ["onebot.jrrp"];

    private const string JrrpUrl = "https://oiapi.net/API/Yun/";

    private static readonly Random _random = new();
    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        var current = JrrpConfig.Instance.GetJrrp(args.Event.Chain.GroupMemberInfo!.Uin);
        byte val = 0;
        if (current != null && current.Time.Date == DateTime.Now.Date)
        {
            val = current.Value;
        }
        else
        {
            val = Convert.ToByte(_random.Next(0, 100));
            JrrpConfig.Instance.SaveJrrp(args.Event.Chain.GroupMemberInfo!.Uin, val);
            JrrpConfig.Save();
        }
        var buffer = await HttpUtils.HttpGetByte(JrrpUrl + $"?let={args.Event.Chain.GroupMemberInfo!.Uin}{val}");
        List<MessageChain> chains = [
                MessageBuilder.Friend(args.Event.Chain.GroupMemberInfo!.Uin).Text($"今日运势").Build(),
                MessageBuilder.Friend(args.Event.Chain.GroupMemberInfo!.Uin).Text($"你今日的运势值: {val}").Build(),
                MessageBuilder.Friend(args.Event.Chain.GroupMemberInfo!.Uin).Image(buffer).Build()
            ];
        await args.Event.Reply(MessageBuilder.Group(args.Event.Chain.GroupUin!.Value).MultiMsg(chains));
    }
}