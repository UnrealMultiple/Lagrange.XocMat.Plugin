using Lagrange.Core;
using Lagrange.XocMat.Commands;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Plugin;
using Lagrange.XocMat.Attributes;
using Microsoft.Extensions.Logging;

namespace CommandUtils;

public class Plugin(ILogger logger, CommandManager commandManager, BotContext bot) : XocMatPlugin(logger, commandManager, bot)
{
    public override void Initialize()
    {
        Lagrange.XocMat.Event.OperatHandler.OnCommand += OnCommand;
        //CommandManager.AddGroupCommand(new("cmd", DisableCommand, "onebot.cmd.utils"));
    }

    private async ValueTask OnCommand(CommandArgs args)
    {
        if(Config.Instance.GroupDisabledCommands.TryGetValue(args.EventArgs.Chain.GroupUin!.Value, out CommandBody? commandBody))
        {
            if (commandBody.DisabledCommands.Count > 0 && commandBody.DisabledCommands.Contains(args.Name))
            {
                await args.EventArgs.Reply("此指令已被禁用");
                args.Handler = true;
                return;
            }
            if (commandBody.AllowedCommands.Count > 0 && !commandBody.AllowedCommands.Contains(args.Name))
            {
                await args.EventArgs.Reply("此指令未被允许");
                args.Handler = true;
            }
        }
    }


    protected override void Dispose(bool dispose)
    {
        Lagrange.XocMat.Event.OperatHandler.OnCommand -= OnCommand;
    }
}
