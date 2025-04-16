using System.Globalization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.XocMat;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Utility.Images;
using Octokit.Webhooks;
using Octokit.Webhooks.Events;
using Octokit.Webhooks.Events.Issues;
using Octokit.Webhooks.Events.PullRequest;
using Octokit.Webhooks.Events.Star;

namespace GitHook;

public record StartOperationRecords(string Repo, string User, string Operation, DateTime Time);
public class WebHook : WebhookEventProcessor
{
    private HashSet<StartOperationRecords> _operations = [];
    private static bool VerifyRepoFeature(WebhookEvent e, WebhookHeaders headers, out uint[] groups)
    {
        if(Config.Instance.Notices.TryGetValue(e.Repository?.FullName ?? "", out var notice))
        {
            if(notice.Features.Contains(headers.Event))
            {
                groups = notice.Groups;
                return true;
            }
        }
        groups = [];
        return false;
    }
    public static async Task SendGroupMsg(IMessageEntity entity, uint[] groups)
    {
        var tasks = groups.Select(i => MessageBuilder.Group(i).Add(entity).Reply());
        await Task.WhenAll(tasks);
    }

    protected override async Task ProcessIssuesWebhookAsync(WebhookHeaders headers, IssuesEvent issuesEvent, IssuesAction action)
    {
        if (action.Equals(IssuesAction.Opened) && VerifyRepoFeature(issuesEvent, headers, out var groups))
        { 
            var title = issuesEvent.Issue.Title;
            var userName = issuesEvent.Issue.User.Login;
            var repName = issuesEvent.Repository?.FullName;
            var tableBuider = new TableBuilder()
                .SetTitle($"新议题")
                .SetMemberUin(XocMatAPI.BotContext.BotUin)
                .SetHeader("序号", issuesEvent.Issue.Number.ToString())
                .AddRow("发起者", userName)
                .AddRow("仓库", repName ?? "")
                .AddRow("标题", title);
            await SendGroupMsg(new ImageEntity(tableBuider.Builder()), groups);
        }
    }

    protected override async Task ProcessStarWebhookAsync(WebhookHeaders headers, StarEvent starEvent, StarAction action)
    {
        if (VerifyRepoFeature(starEvent, headers, out var groups))
        {
            var record = new StartOperationRecords(starEvent.Repository?.FullName ?? "empty",
                starEvent.Sender?.Login ?? "empty",
                CultureInfo.InvariantCulture.TextInfo.ToTitleCase(action),
                DateTime.Now.Date);
            if (_operations.Contains(record))
                return;
            else
                _operations.Add(record);
            var msg = $"用户 {starEvent.Sender?.Login} {CultureInfo.InvariantCulture.TextInfo.ToTitleCase(action)} Start 仓库 {starEvent.Repository?.FullName} 共计({starEvent.Repository?.StargazersCount})个Star";
            await SendGroupMsg(new TextEntity(msg), groups);
        } 
    }

    protected override async Task ProcessPullRequestWebhookAsync(WebhookHeaders headers, PullRequestEvent pullRequestEvent, PullRequestAction action)
    {
        if (action == PullRequestAction.Opened && VerifyRepoFeature(pullRequestEvent, headers, out var groups))
        {
            var title = pullRequestEvent.PullRequest.Title;
            var userName = pullRequestEvent.PullRequest.User.Login;
            var repName = pullRequestEvent.Repository?.FullName;
            var tableBuider = new TableBuilder()
                .SetTitle($"新的拉取请求")
                .SetMemberUin(XocMatAPI.BotContext.BotUin)
                .SetHeader("序号", pullRequestEvent.Number.ToString())
                .AddRow("发起者", userName)
                .AddRow("仓库", repName ?? "")
                .AddRow("标题", title);
            await SendGroupMsg(new ImageEntity(tableBuider.Builder()), groups);
        }
    }
}
