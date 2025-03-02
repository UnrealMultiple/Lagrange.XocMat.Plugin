using Lagrange.Core;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Event;
using Lagrange.XocMat.Plugin;
using Microsoft.Extensions.Logging;

namespace OnlineReward;

public class Plugin(ILogger logger, BotContext bot) : XocMatPlugin(logger, bot)
{
    public override string Name => "OnlineReward";

    public override string Description => "领取在线时长奖励";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 0);

    protected override void Initialize()
    {
        OperatHandler.OnGroupCommand += OperatHandler_OnCommand;
    }


    private async ValueTask OperatHandler_OnCommand(GroupCommandArgs args)
    {
        if (args.Name == "泰拉服务器重置")
        {
            Config.Instance.Reset();
        }
        await Task.CompletedTask;
    }

    protected override void Dispose(bool dispose)
    {
        OperatHandler.OnGroupCommand -= OperatHandler_OnCommand;
    }
}
