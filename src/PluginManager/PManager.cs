using System.Text;
using Lagrange.Core.Message;
using Lagrange.XocMat;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;

namespace PluginManager;

public class PManager : Command
{
    public override string HelpText => "插件管理";
    public override string[] Alias => ["pm"];
    public override string[] Permissions => ["onebot.plugin.admin"];

    public override async Task InvokeAsync(GroupCommandArgs args)
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
            sb.AppendLine("|序号|插件名称|插件作者|插件说明|插件版本|启用|");
            sb.AppendLine("|:--:|:--:|:--:|:--:|:--:|:--:|");
            int index = 1;
            foreach (var plugin in XocMatAPI.PluginLoader.PluginContext.Plugins)
            {
                sb.AppendLine($"|{index}|{plugin.Plugin.Name}|{plugin.Plugin.Author}|{plugin.Plugin.Description}|{plugin.Plugin.Version}|{plugin.Initialized}|");
                index++;
            }
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("</div>");
            await args.Event.Reply(MessageBuilder.Group(args.Event.Chain.GroupUin!.Value).MarkdownImage(sb.ToString()));
        }
        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "off")
        {
            if (!int.TryParse(args.Parameters[1], out var index) || index < 1 || index > XocMatAPI.PluginLoader.PluginContext.Plugins.Count)
            {
                await args.Event.Reply("请输入一个正确的序号!", true);
                return;
            }
            var instance = XocMatAPI.PluginLoader.PluginContext.Plugins[index - 1];
            if (!instance.Initialized)
            {
                await args.Event.Reply("此插件已经被卸载，无需重复卸载!!", true);
                return;
            }
            instance.DeInitialize();
            await args.Event.Reply($"{instance.Plugin.Name} 插件卸载成功!", true);
        }
        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "on")
        {
            if (!int.TryParse(args.Parameters[1], out var index) || index < 1 || index > XocMatAPI.PluginLoader.PluginContext.Plugins.Count)
            {
                await args.Event.Reply("请输入一个正确的序号!", true);
                return;
            }
            var instance = XocMatAPI.PluginLoader.PluginContext.Plugins[index - 1];
            if (instance.Initialized)
            {
                await args.Event.Reply("此插件已经被启用，无需重复启用!!", true);
                return;
            }
            instance.Initialize();
            await args.Event.Reply($"{instance.Plugin.Name} 插件加载成功!", true);
        }
        else if (args.Parameters.Count == 1 && args.Parameters[0].ToLower() == "reload")
        {
            XocMatAPI.PluginLoader.UnLoad();
            XocMatAPI.PluginLoader.Load();
            await args.Event.Reply("插件列表已经重新加载!", true);
        }
        else
        {
            await args.Event.Reply("语法错误,正确语法:\n" +
                $"{args.CommamdPrefix}{args.Name} list" +
                $"{args.CommamdPrefix}{args.Name} off [序号]" +
                $"{args.CommamdPrefix}{args.Name} on [序号]");
        }
    }
}
