using System.Text;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Microsoft.Extensions.Logging;
using Octokit;

namespace GitHook.Commands;

public class IssueManager : Command
{
    public override string[] Alias => ["issue", "issues"];
    public override string HelpText => "issue";
    public override string[] Permissions => ["onebot.aichat.issue"];
    public override async Task InvokeAsync(GroupCommandArgs args, ILogger log)
    {
        if (args.Parameters.Count < 1)
        {
            await args.Event.Reply($"语法错误，正确语法: \n{args.CommamdPrefix}{args.Name} list\n" +
                $"{args.CommamdPrefix}{args.Name} see [编号]\n" +
                $"{args.CommamdPrefix}{args.Name} close [编号]\n" +
                $"{args.CommamdPrefix}{args.Name} reply [编号] [回复]");
            return;
        }
        switch (args.Parameters[0].ToLower())
        {
            case "see":
                if (args.Parameters.Count > 1 && int.TryParse(args.Parameters[1], out var num))
                {
                    var issue = await TShockPluginRepoClient.GetIssueNumber(num);
                    var buffer = await GithubPageUtils.ScreenPage(issue.HtmlUrl);
                    await args.MessageBuilder.Image(buffer).Reply();
                }
                else
                {
                    await args.Event.Reply("请输入一个正确的Issue编号!", true);
                }
                break;
            case "close":
                if (args.Parameters.Count > 1 && int.TryParse(args.Parameters[1], out var id))
                {
                    var issue = await TShockPluginRepoClient.CloseIssue(id);
                    if (issue.State == ItemState.Closed)
                    {
                        await args.Event.Reply($"Issue #{id} 关闭成功!", true);
                    }
                    else
                    {
                        await args.Event.Reply($"未知错误，关闭失败。", true);
                    }
                }
                else
                {
                    await args.Event.Reply("请输入一个正确的Issue编号!", true);
                }
                break;
            case "reply":
                if (args.Parameters.Count > 2 && int.TryParse(args.Parameters[1], out var index))
                {
                    var issue = await TShockPluginRepoClient.ReplyIssue(index, $"`{args.Event.Chain.GroupMemberInfo?.MemberName}({args.Event.Chain.GroupMemberInfo?.Uin}@qq.com) Reply:`{args.Parameters[2]}");
                    await args.Event.Reply("回复成功!", true);
                }
                else
                {
                    await args.Event.Reply("请输入一个正确的Issue编号与回复内容!", true);
                }
                break;
            case "list":
                var issues = await TShockPluginRepoClient.GetIssueOpen();
                var sb = new StringBuilder();
                sb.AppendLine($$"""<div align="center">""");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine($"# 正在进行的Issue列表");
                sb.AppendLine();
                sb.AppendLine("|Issue编号|标题|发起人|");
                sb.AppendLine("|:--:|:--:|:--:|");
                foreach (var issue in issues)
                {
                    sb.AppendLine($"|{issue.Number}|{issue.Title}|{issue.User.Login}|");
                }
                sb.AppendLine();
                sb.AppendLine($$"""</div>""");
                await args.MessageBuilder.MarkdownImage(sb.ToString()).Reply();
                break;
            default:
                await args.Event.Reply("错误的子命令!");
                return;
        }
    }
}
