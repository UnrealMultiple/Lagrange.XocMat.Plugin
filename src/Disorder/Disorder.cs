using Lagrange.Core;
using Lagrange.XocMat.Plugin;
using Microsoft.Extensions.Logging;

namespace Disorder;

public class Disorder(ILogger logger, BotContext bot) : XocMatPlugin(logger, bot)
{
    public override string Name => "Disorder";

    public override string Description => "提供一些乱七八糟的功能";

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
