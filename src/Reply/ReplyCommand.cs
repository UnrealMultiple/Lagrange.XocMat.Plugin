using System.Text;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Utility.Images;
using Microsoft.Extensions.Logging;

namespace Reply;

public class ReplyCommand : Command
{
    public override string[] Alias => ["reply"];

    public override string HelpText => "reply <匹配词> <回复内容>";

    public override string[] Permissions => ["onebot.reply"];

    private Dictionary<string, Func<GroupCommandArgs, ILogger, Task>> _action = new()
    {
        { "add", Add },
        { "remove", Remove },
        { "del", Remove },
        { "list", ReplyList },
        { "var", Variables },
        { "content", Contents }
    };

    private static async Task Contents(GroupCommandArgs args, ILogger logger)
    {
        var handlers = ReplyAdapter.GetContentHandlers();
        if (handlers.Count == 0)
        {
            await args.Event.Reply("没有任何内容处理器", true);
            return;
        }
        await args.Event.Reply("内容处理器列表: " + string.Join(", ", handlers), true);
    }

    private static async Task Variables(GroupCommandArgs args, ILogger logger)
    {
        var variables = ReplyAdapter.GetVariables();
        if (variables.Count == 0)
        {
            await args.Event.Reply("没有任何变量", true);
            return;
        }
        await args.Event.Reply("变量列表: " + string.Join(", ", variables), true);
    }

    private static async Task ReplyList(GroupCommandArgs args, ILogger logger)
    {
        var rules = Config.Instance.Rules;
        if (rules.Count == 0)
        {
            await args.Event.Reply("没有任何规则", true);
            return;
        }
        var builder = new TableBuilder()
            .SetTitle("ReplyRule")
            .SetMemberUin(args.MemberUin)
            .SetHeader("序号", "规则", "回复");
        for (var i = 0; i < rules.Count; i++)
        {
            builder.AddRow((i + 1).ToString(), rules[i].MatchPattern, rules[i].ReplyTemplate);
        }
        await args.MessageBuilder.Image(builder.Builder()).Reply();
    }

    private static async Task Remove(GroupCommandArgs args, ILogger logger)
    {
        if(args.Parameters.Count < 2)
        {
            await args.Event.Reply($"语法错误，正确语法: {args.CommamdPrefix}{args.Name} remove <序号>");
            return;
        }
        if (int.TryParse(args.Parameters[1], out var index))
        {
            Config.Instance.RemoveRule(index);
            Config.Save();
            await args.Event.Reply("删除成功", true);
        }
        else
        {
            await args.Event.Reply("序号必须是数字", true);
        }
    }

    public override async Task InvokeAsync(GroupCommandArgs args, ILogger log)
    {
        if (args.Parameters.Count >= 1 && _action.TryGetValue(args.Parameters[0], out var act))
        {
            await act(args, log);
            return;
        }
        await args.Event.Reply($"语法错误，正确语法:" +
            $"\n{args.CommamdPrefix}{args.Name} add <规则> <回复>" +
            $"\n{args.CommamdPrefix}{args.Name} del <序号>" +
            $"\n{args.CommamdPrefix}{args.Name} list" +
            $"\n{args.CommamdPrefix}{args.Name} var" +
            $"\n{args.CommamdPrefix}{args.Name} content", true);
    }

    private static async Task Add(GroupCommandArgs args, ILogger logger)
    {
        if(args.Parameters.Count < 3)
        {
            await args.Event.Reply($"语法错误，正确语法: {args.CommamdPrefix}{args.Name} add <匹配词> <回复词>");
            return;
        }
        Config.Instance.Rules.Add(new(args.Parameters[1], args.Parameters[2]));
        Config.Save();
        await args.Event.Reply("添加成功", true);
    }
}
