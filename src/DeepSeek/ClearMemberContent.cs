using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;

namespace DeepSeek;

public class ClearMemberContent : Command
{
    public override string[] Alias => ["清空上下文"];

    public override string HelpText => "清空AI提问上下文!";

    public override string[] Permissions => ["onebot.aichat.clear"];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        Utils.Instance.ClearChat(args.MemberUin);
        await args.Event.Reply("上下文已清除", true);
    }
}
