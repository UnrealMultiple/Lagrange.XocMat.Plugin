using System.Text;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Octokit;

namespace GitHook.Commands;

public class PullRequestManager : Command
{
    public override string[] Alias => ["pr"];
    public override string HelpText => "管理Pull Request";
    public override string[] Permissions => ["onebot.pr"];
    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (args.Parameters.Count < 1)
        {
            await args.Event.Reply($"语法错误，正确语法: \n{args.CommamdPrefix}{args.Name} list\n" +
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
                    await args.Event.Reply("请输入一个正确的PR编号!", true);
                }
                break;
            case "close":
                if (args.Parameters.Count > 1 && int.TryParse(args.Parameters[1], out var id))
                {
                    var pr = await TShockPluginRepoClient.ClosePullRequest(id);
                    if (pr.State == ItemState.Closed)
                    {
                        await args.Event.Reply($"Pull Request #{id} 关闭成功!", true);
                    }
                    else
                    {
                        await args.Event.Reply($"未知错误，关闭失败。", true);
                    }
                }
                else
                {
                    await args.Event.Reply("请输入一个正确的PR编号!", true);
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
                await args.Event.Reply("错误的子命令!");
                return;
        }
    }
}
