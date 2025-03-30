using System.Text.Json.Nodes;
using Lagrange.Core.Message;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Utility;
using Microsoft.Extensions.Logging;

namespace Disorder.Commands;

public class Cosplay : Command
{
    public override string[] Alias => ["cosplay", "cos"];

    public override string HelpText => "cosplay";

    public override string[] Permissions => ["onebot.aichat.cosplay"];

    private const string CosUrl = "https://imgapi.cn/cos2.php?return=jsonpro";

    public override async Task InvokeAsync(GroupCommandArgs args, ILogger log)
    {
        var res = await HttpUtils.GetStringAsync(CosUrl);
        var json = JsonNode.Parse(res);
        var arr = json?["imgurls"]?.AsArray();
        if (arr == null)
            return;
        List<MessageChain> chains = [];
        foreach (var img in arr)
        {
            var url = img?.ToString();
            if (url == null) continue;
            chains.Add(MessageBuilder.Friend(args.Event.Chain.GroupMemberInfo!.Uin).Image(HttpUtils.GetByteAsync(url).Result).Build());
        }
        var build = MessageBuilder.Group(args.Event.Chain.GroupUin!.Value);
        await args.Event.Reply(build.MultiMsg(chains.ToArray()));
    }
}
