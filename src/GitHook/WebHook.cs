using System.Text;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Lagrange.XocMat;
using Lagrange.XocMat.Extensions;
using Octokit.Webhooks;
using Octokit.Webhooks.Events;
using Octokit.Webhooks.Events.PullRequest;
using Octokit.Webhooks.Events.Star;

namespace GitHook;

public class WebHook : WebhookEventProcessor
{

    public static async Task SendGroupMsg(IEnumerable<MessageBuilder> builders)
    {
        foreach (var builder in builders)
        {
            await XocMatAPI.BotContext.SendMessage(builder.Build());
        }
    }

    protected override async Task ProcessStarWebhookAsync(WebhookHeaders headers, StarEvent starEvent, StarAction action)
    {
        var msg = $"用户 {starEvent.Sender?.Login} Stared 仓库 {starEvent.Repository?.FullName} 共计({starEvent.Repository?.StargazersCount})个Star";
        await SendGroupMsg(Config.Instance.Groups.Select(i => MessageBuilder.Group(i).Text(msg)));
    }
    //protected override async Task ProcessPushWebhookAsync(WebhookHeaders headers, PushEvent pushEvent)
    //{
    //    if (!Config.Instance.GithubActions.TryGetValue(WebhookEventType.Push, out var groups)
    //        || groups == null
    //        || groups.Count == 0)
    //        return;
    //    if (pushEvent.Pusher.Name != "github-actions[bot]")
    //    {
    //        var repName = pushEvent.Repository?.FullName;
    //        var sb = new StringBuilder($"# Push Github 仓库 {repName} # {}");
    //        foreach (var commit in pushEvent.Commits)
    //        {
    //            sb.AppendLine();
    //            sb.AppendLine($"### {commit.Message}");
    //            sb.AppendLine($"- 用户名: `{commit.Author.Username}`");
    //            sb.AppendLine($"- 添加文件: {(commit.Added.Any() ? string.Join(" ", commit.Added.Select(x => $"`{x}`")) : "无")}");
    //            sb.AppendLine($"- 删除文件: {(commit.Removed.Any() ? string.Join(" ", commit.Removed.Select(x => $"`{x}`")) : "无")}");
    //            sb.AppendLine($"- 更改文件: {(commit.Modified.Any() ? string.Join(" ", commit.Modified.Select(x => $"`{x}`")) : "无")}");
    //        }
    //        await SendGroupMsg(groups.Select(i => MessageBuilder.Group(i).MarkdownImage(sb.ToString())));
    //    }
    //}

    protected override async Task ProcessPullRequestWebhookAsync(WebhookHeaders headers, PullRequestEvent pullRequestEvent, PullRequestAction action)
    {
        if (action == PullRequestAction.Opened)
        {
            var title = pullRequestEvent.PullRequest.Title;
            var userName = pullRequestEvent.PullRequest.User.Login;
            var repName = pullRequestEvent.Repository?.FullName;
            var sb = new StringBuilder($"# Pull Request Github 仓库 {repName} #{pullRequestEvent.Number}");
            sb.AppendLine();
            sb.AppendLine($"## {title}");
            sb.AppendLine($"- 发起者: `{userName}`");
            sb.AppendLine($"```");
            sb.AppendLine(pullRequestEvent.PullRequest.Body);
            sb.AppendLine($"```");
            await SendGroupMsg(Config.Instance.Groups.Select(i => MessageBuilder.Group(i).MarkdownImage(sb.ToString())));
        }

    }
}
