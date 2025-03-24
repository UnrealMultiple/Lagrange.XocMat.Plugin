using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Utility;
using Microsoft.Extensions.Logging;

namespace Disorder.Commands;

public class Fellow : Command
{
    public override string[] Alias => ["今日道侣"];
    public override string HelpText => "fellow";
    public override string[] Permissions => ["onebot.fellow"];

    private const string FellowUrl = "https://oiapi.net/API/CPUni/";
    public override async Task InvokeAsync(GroupCommandArgs args, ILogger log)
    {
        var w = FollowConfig.Instance.GetFollow(args.Event.Chain.GroupMemberInfo!.Uin);
        long targerid = 0;
        var targetName = string.Empty;
        if (w != null && w.Time.Date == DateTime.Now.Date)
        {
            targerid = w.Follow;
            targetName = w.WeaponName;

        }
        else
        {
            var members = await args.Bot.FetchMembers(args.Event.Chain.GroupUin!.Value);
            var targer = members.OrderBy(x => Guid.NewGuid()).First();
            targerid = targer.Uin;
            targetName = targer.MemberName.Length > 6 ? targer.MemberName[..6] : targer.MemberName;
            FollowConfig.Instance.SaveFollow(args.Event.Chain.GroupMemberInfo!.Uin, targerid, targetName);
            FollowConfig.Save();
        }

        var stream = await HttpUtils.GetByteAsync(FellowUrl + $"?first={(args.Event.Chain.GroupMemberInfo!.MemberName.Length > 6 ? args.Event.Chain.GroupMemberInfo!.MemberName[..6] : args.Event.Chain.GroupMemberInfo!.MemberCard)}&second={targetName}");
        List<MessageChain> chains = [
                 MessageBuilder.Friend(args.Event.Chain.GroupMemberInfo!.Uin).Text($"今日道侣").Build(),
                 MessageBuilder.Friend(args.Event.Chain.GroupMemberInfo!.Uin).Image(HttpUtils.GetByteAsync($"http://q.qlogo.cn/headimg_dl?dst_uin={targerid}&spec=640&img_type=png").Result).Text($"账号: {targerid}\n昵称: {targetName}").Build(),
                 MessageBuilder.Friend(args.Event.Chain.GroupMemberInfo!.Uin).Image(stream).Build()
            ];
        await args.Event.Reply(MessageBuilder.Group(args.Event.Chain.GroupUin!.Value).MultiMsg(chains));
    }
}
