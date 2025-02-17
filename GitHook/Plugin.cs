using System.Text;
using Lagrange.Core;
using Lagrange.XocMat.Commands;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Plugin;
using Microsoft.Extensions.Logging;
using Octokit;

namespace GitHook;

public class Plugin(ILogger logger, CommandManager commandManager, BotContext bot) : XocMatPlugin(logger, commandManager, bot)
{
    public override string Name => "GitHook";

    public override string Description => "用于管理github仓库";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 0);

    public override void Initialize()
    {
        CommandManager.AddGroupCommand(new("pr", PullRequestManager, "onebot.repo.admin"));
        CommandManager.AddGroupCommand(new("issue", IssueManager, "onebot.repo.admin"));
        CommandManager.AddGroupCommand(new("approve", Approve, "onebot.repo.admin"));
    }

    private async ValueTask Approve(CommandArgs args)
    {
        if (args.Parameters.Count >= 1 && int.TryParse(args.Parameters[0], out var num))
        {
            var state = await TShockPluginRepoClient.Approve(num);
            if (state == PullRequestReviewState.Approved)
            {
                await args.EventArgs.Reply($"仓库 {TShockPluginRepoClient.Owner}/{TShockPluginRepoClient.Repo} Pull Request #{num} 被批准合并!", true);
            }
            else
            {
                await args.EventArgs.Reply($"批准失败，返回状态码:{state}");
            }

        }
        else
        {
            await args.EventArgs.Reply("请输入一个正确的pr号!", true);
        }
    }

    private async ValueTask IssueManager(CommandArgs args)
    {
        if (args.Parameters.Count < 1)
        {
            await args.EventArgs.Reply($"语法错误，正确语法: \n{args.CommamdPrefix}{args.Name} list\n" +
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
                    await args.EventArgs.Reply("请输入一个正确的Issue编号!", true);
                }
                break;
            case "close":
                if (args.Parameters.Count > 1 && int.TryParse(args.Parameters[1], out var id))
                {
                    var issue = await TShockPluginRepoClient.CloseIssue(id);
                    if (issue.State == ItemState.Closed)
                    {
                        await args.EventArgs.Reply($"Issue #{id} 关闭成功!", true);
                    }
                    else
                    {
                        await args.EventArgs.Reply($"未知错误，关闭失败。", true);
                    }
                }
                else
                {
                    await args.EventArgs.Reply("请输入一个正确的Issue编号!", true);
                }
                break;
            case "reply":
                if (args.Parameters.Count > 2 && int.TryParse(args.Parameters[1], out var index))
                {
                    var issue = await TShockPluginRepoClient.ReplyIssue(index, $"`{args.EventArgs.Chain.GroupMemberInfo?.MemberName}({args.EventArgs.Chain.GroupMemberInfo?.Uin}@qq.com) Reply:`{args.Parameters[2]}");
                    await args.EventArgs.Reply("回复成功!", true);
                }
                else
                {
                    await args.EventArgs.Reply("请输入一个正确的Issue编号与回复内容!", true);
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
                await args.EventArgs.Reply("错误的子命令!");
                return;
        }
    }

    private async ValueTask PullRequestManager(CommandArgs args)
    {
        if (args.Parameters.Count < 1)
        {
            await args.EventArgs.Reply($"语法错误，正确语法: \n{args.CommamdPrefix}{args.Name} list\n" +
                $"{args.CommamdPrefix}{args.Name} see [编号]\n" +
                $"{args.CommamdPrefix}{args.Name} merge [编号]\n" +
                $"{args.CommamdPrefix}{args.Name} close [编号]");
            return;
        }
        switch (args.Parameters[0].ToLower())
        {
            case "see":
                if (args.Parameters.Count > 1 && int.TryParse(args.Parameters[1], out var num))
                {
                    var pr = await TShockPluginRepoClient.GetPullRequestNumber(num);
                    var buffer = await GithubPageUtils.ScreenPage($"{pr.HtmlUrl}/files", "#hide-file-tree-button");
                    await args.MessageBuilder.Image(buffer).Reply();
                }
                else
                {
                    await args.EventArgs.Reply("请输入一个正确的PR编号!", true);
                }
                break;
            case "close":
                if (args.Parameters.Count > 1 && int.TryParse(args.Parameters[1], out var id))
                {
                    var pr = await TShockPluginRepoClient.ClosePullRequest(id);
                    if (pr.State == ItemState.Closed)
                    {
                        await args.EventArgs.Reply($"Pull Request #{id} 关闭成功!", true);
                    }
                    else
                    {
                        await args.EventArgs.Reply($"未知错误，关闭失败。", true);
                    }
                }
                else
                {
                    await args.EventArgs.Reply("请输入一个正确的PR编号!", true);
                }
                break;
            case "list":
                var prs = await TShockPluginRepoClient.GetPullRequestOpen();
                var sb = new StringBuilder();
                sb.AppendLine($$"""<div align="center">""");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine($"# 正在进行的PR列表");
                sb.AppendLine();
                sb.AppendLine("|PR编号|标题|发起人|");
                sb.AppendLine("|:--:|:--:|:--:|");
                foreach (var pr in prs)
                {
                    sb.AppendLine($"|{pr.Number}|{pr.Title}|{pr.User.Login}|");
                }
                sb.AppendLine();
                sb.AppendLine($$"""</div>""");
                await args.MessageBuilder.MarkdownImage(sb.ToString()).Reply();
                break;
            default:
                await args.EventArgs.Reply("错误的子命令!");
                return;
        }
    }

  

    protected override void Dispose(bool dispose)
    {
        CommandManager.GroupCommandDelegate.RemoveAll(c => c.CallBack == IssueManager || c.CallBack == PullRequestManager || c.CallBack == Approve);
    }
}
