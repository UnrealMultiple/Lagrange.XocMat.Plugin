using Lagrange.Core;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Plugin;
using Microsoft.Extensions.Logging;

namespace CommandUtils;

public class Plugin(ILogger logger, BotContext bot) : XocMatPlugin(logger, bot)
{
    public override string Name => "CommandUtils";

    public override string Description => "提供命令扩展程序";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 0);

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
