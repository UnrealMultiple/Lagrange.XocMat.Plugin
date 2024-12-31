using Lagrange.Core;
using Lagrange.Core.Message;
using Lagrange.XocMat;
using Lagrange.XocMat.Commands;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Plugin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;

namespace PluginManager;

public class Plugin(ILogger logger, CommandManager commandManager, BotContext bot) : XocMatPlugin(logger, commandManager, bot)
{
    public override string Name => "PluginManager";

    public override string Description => "提供插件管理功能";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 0);

    private PluginLoader Loader => XocMatApp.Instance.Services.GetRequiredService<PluginLoader>();

    public override void Initialize()
    {
        CommandManager.AddGroupCommand(new("pm", PManager, "onebot.plugin.admin"));
    }

    protected override void Dispose(bool dispose)
    {
        CommandManager.GroupCommandDelegate.RemoveAll(x => x.CallBack == PManager);
    }

    private async ValueTask HotReloadPlugin(CommandArgs args)
    {
        Stopwatch sw = new();
        sw.Start();
        CommandManager.GroupCommandDelegate.Clear();
        CommandManager.ServerCommandDelegate.Clear();

        Loader.UnLoad();
        Loader.Load();
        sw.Stop();
        await args.EventArgs.Reply($"插件热重载成功{AppDomain.CurrentDomain.GetAssemblies().Length}!\n用时:{sw.Elapsed.TotalSeconds:F5} 秒!", true);
    }

    private async ValueTask PManager(CommandArgs args)
    {
        if (args.Parameters.Count == 1 && args.Parameters[0].Equals("list", StringComparison.CurrentCultureIgnoreCase))
        {
            var sb = new StringBuilder();
            sb.AppendLine($$"""<div align="center">""");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine($"# 插件列表");
            sb.AppendLine();
            sb.AppendLine("|序号|插件名称|插件作者|插件说明|插件版本|");
            sb.AppendLine("|:--:|:--:|:--:|:--:|:--:|");
            int index = 1;
            foreach (var plugin in Loader.PluginContext.Plugins)
            {
                sb.AppendLine($"|{index}|{plugin.Name}|{plugin.Author}|{plugin.Description}|{plugin.Version}");
                index++;
            }
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("</div>");
            await args.EventArgs.Reply(MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value).MarkdownImage(sb.ToString()));
        }
        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "off")
        {
            if (!int.TryParse(args.Parameters[1], out var index) || index < 1 || index > Loader.PluginContext.Plugins.Count)
            {
                await args.EventArgs.Reply("请输入一个正确的序号!", true);
                return;
            }
            var instance = Loader.PluginContext.Plugins[index - 1];
            instance.Dispose();
            await args.EventArgs.Reply($"{instance.Name} 插件卸载成功!", true);
        }
        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "on")
        {
            if (!int.TryParse(args.Parameters[1], out var index) || index < 1 || index > Loader.PluginContext.Plugins.Count)
            {
                await args.EventArgs.Reply("请输入一个正确的序号!", true);
                return;
            }
            var instance = Loader.PluginContext.Plugins[index - 1];
            instance.Initialize();
            await args.EventArgs.Reply($"{instance.Name} 插件加载成功!", true);
        }
        else if (args.Parameters.Count == 1 && args.Parameters[0].ToLower() == "reload")
        {
            await HotReloadPlugin(args);
        }
        else
        {
            await args.EventArgs.Reply("语法错误,正确语法:\n" +
                $"{args.CommamdPrefix}{args.Name} list" +
                $"{args.CommamdPrefix}{args.Name} off [序号]" +
                $"{args.CommamdPrefix}{args.Name} on [序号]");
        }
    }
}
