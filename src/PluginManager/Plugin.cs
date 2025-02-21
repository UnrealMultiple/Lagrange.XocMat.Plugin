using Lagrange.Core;
using Lagrange.XocMat.Plugin;
using Microsoft.Extensions.Logging;

namespace PluginManager;

public class Plugin(ILogger logger, BotContext bot) : XocMatPlugin(logger, bot)
{
    public override string Name => "PluginManager";

    public override string Description => "提供插件管理功能";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 0);

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override void Dispose(bool dispose)
    {
        base.Dispose(dispose);
    }

}
