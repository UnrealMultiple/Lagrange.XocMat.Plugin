using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;
using Microsoft.Extensions.Logging;

namespace Music.Commands;

public class ChangeMusicSource : Command
{
    public override string HelpText => "切换音乐源";
    public override string[] Alias => ["切换音源"];
    public override string[] Permissions => [OneBotPermissions.Music];

    public override async Task InvokeAsync(GroupCommandArgs args, ILogger log)
    {
        if (args.Parameters.Count > 0)
        {
            if (args.Parameters[0] == "QQ" || args.Parameters[0] == "网易")
            {
                MusicTool.ChangeLocal(args.Parameters[0], args.Event.Chain.GroupMemberInfo!.Uin);
                await args.Event.Reply($"音源已切换至{args.Parameters[0]}");
            }
            else
            {
                await args.Event.Reply("请输入正确的音源!");
            }
        }
        else
        {
            await args.Event.Reply("请输入正确的音源!");
        }
    }
}
