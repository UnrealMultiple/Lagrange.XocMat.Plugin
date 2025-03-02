using Lagrange.Core;
using Lagrange.XocMat.Plugin;
using Microsoft.Extensions.Logging;

namespace TerrariaCart;

public class Plugin(ILogger logger, BotContext bot) : XocMatPlugin(logger, bot)
{
    public override string Name => "TerrariaCart";

    public override string Description => "提供泰拉商店的购物车功能，更加方便的使用商店!";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 0);


    protected override void Initialize()
    {
    }


    protected override void Dispose(bool dispose)
    {
    }
}
