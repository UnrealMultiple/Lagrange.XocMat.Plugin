using Lagrange.Core;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Plugin;
using Microsoft.Extensions.Logging;

namespace CommandUtils;

public class Plugin(ILogger logger, BotContext bot) : XocMatPlugin(logger, bot)
{
    protected override void Initialize()
    {
        Lagrange.XocMat.Event.OperatHandler.OnGroupCommand += OnCommand;
    }

    private ValueTask OnCommand(GroupCommandArgs args)
    {
        if (Config.Instance.GroupDisabledCommands.TryGetValue(args.Event.Chain.GroupUin!.Value, out CommandBody? commandBody))
        {
            if (commandBody.DisabledCommands.Count > 0 && commandBody.DisabledCommands.Contains(args.Name))
            {
                args.Handler = true;
            }
            if (commandBody.AllowedCommands.Count > 0 && !commandBody.AllowedCommands.Contains(args.Name))
            {
                args.Handler = true;
            }
        }
        return ValueTask.CompletedTask;
    }


    protected override void Dispose(bool dispose)
    {
        Lagrange.XocMat.Event.OperatHandler.OnGroupCommand -= OnCommand;
    }
}
