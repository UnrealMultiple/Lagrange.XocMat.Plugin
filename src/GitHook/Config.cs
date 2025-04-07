using Lagrange.XocMat.Attributes;
using Lagrange.XocMat.Configuration;
using Newtonsoft.Json;
using Octokit.Webhooks;

namespace GitHook;

[ConfigSeries]
public class Config : JsonConfigBase<Config>
{
    [JsonProperty("启用")]
    public bool Enable { get; set; } = true;

    [JsonProperty("路由")]
    public string Path { get; set; } = "/update/";

    [JsonProperty("端口")]
    public int Port { get; set; } = 7000;

    [JsonProperty("私人令牌")]
    public string Token { get; set; } = string.Empty;

    [JsonProperty("通知群")]
    public Dictionary<string, RepoNotice> Notices { get; set; } = [];

    protected override void SetDefault()
    {
        Notices["Owner/Repo"] = new RepoNotice
        {
            Groups = [123456789],
            Features = [WebhookEventType.Star, WebhookEventType.PullRequest, WebhookEventType.Issues]
        };
    }
}

public class RepoNotice
{
    [JsonProperty("通知群")]
    public uint[] Groups { get; set; } = [];

    [JsonProperty("功能")]
    public string[] Features { get; set; } = [];
}
