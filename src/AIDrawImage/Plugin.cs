using Lagrange.Core;
using Lagrange.XocMat.Plugin;
using Microsoft.Extensions.Logging;

namespace AIDrawImage;

public class Plugin(ILogger logger, BotContext bot) : XocMatPlugin(logger, bot)
{
    public override string Name => "DrawImage";

    public override string Description => "使用ai绘制图片";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 0);
    protected override void Dispose(bool dispose)
    {

    }

    protected override void Initialize()
    {

    }
}
