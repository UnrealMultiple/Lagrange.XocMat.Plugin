using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Microsoft.Extensions.Logging;

namespace ChatGlm;

public class ChatCommand : Command
{
    public override string[] Alias => ["chat"];

    public override string[] Permissions => ["onebot.chat.use"];

    public override string HelpText => "智谱唉";

    public override async Task InvokeAsync(GroupCommandArgs args, ILogger log)
    {
        if (args.Parameters.Count == 0)
            return;
        var msg = args.Parameters.JoinToString(" ");
        if (string.IsNullOrEmpty(msg))
            return;
        await args.Event.Reply(await Utils.Chat(msg), true);
    }
}
