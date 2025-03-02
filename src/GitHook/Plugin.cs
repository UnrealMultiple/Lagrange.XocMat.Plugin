using Lagrange.Core;
using Lagrange.XocMat.Plugin;
using Microsoft.Extensions.Logging;

namespace GitHook;

public class Plugin(ILogger logger, BotContext bot) : XocMatPlugin(logger, bot)
{
    public override string Name => "GitHook";

    public override string Description => "用于管理github仓库";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 1);

    protected override void Initialize()
    {
    }

    protected override void Dispose(bool dispose)
    {
    }
}
