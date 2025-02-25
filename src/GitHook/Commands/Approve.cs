using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Octokit;

namespace GitHook.Commands;

public class Approve : Command
{
    public override string[] Alias => ["approve"];
    public override string HelpText => "Approve a pull request";
    public override string[] Permissions => ["onebot.approve"];
    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (args.Parameters.Count >= 1 && int.TryParse(args.Parameters[0], out var num))
        {
            var state = await TShockPluginRepoClient.Approve(num);
            if (state == PullRequestReviewState.Approved)
            {
                await args.Event.Reply($"仓库 {TShockPluginRepoClient.Owner}/{TShockPluginRepoClient.Repo} Pull Request #{num} 被批准合并!", true);
            }
            else
            {
                await args.Event.Reply($"批准失败，返回状态码:{state}");
            }

        }
        else
        {
            await args.Event.Reply("请输入一个正确的pr号!", true);
        }
    }
}
