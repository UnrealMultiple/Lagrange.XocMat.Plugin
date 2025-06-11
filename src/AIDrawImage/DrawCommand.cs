using Lagrange.Core.Message;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Utility;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AIDrawImage;

public class DrawCommand : Command
{
    public override string HelpText => "AI 绘图命令";

    public override string[] Alias => ["绘图"];

    public override string[] Permissions => ["onebot.draw.image"];

    private const string Url = "https://oiapi.net/api/AiDrawImage";

    public override async Task InvokeAsync(GroupCommandArgs args, ILogger log)
    {
        if (args.Parameters.Count == 0)
        {
            await args.Event.Reply("请输入绘图提示词!", true);
            return;
        }
        var prompt = string.Join(" ", args.Parameters);
        var response = await GetImageUrlAsync(prompt);
        if (response.Code != 1)
        {
            await args.Event.Reply($"绘图失败: {response.Message}", true);
            return;
        }
        var list = response.Data.ToObject<List<ImageContent>>()!;
        if (list.Count == 0)
        {
            await args.Event.Reply("绘图结果为空，请稍后再试!", true);
            return;
        }
        var MessageBuilders = new List<MessageBuilder>()
        {
            MessageBuilder.Group(args.MemberUin).Text(prompt)
        };
        foreach (var image in list)
        {
            MessageBuilders.Add(MessageBuilder.Group(args.MemberUin).Image(await HttpUtils.GetByteAsync(image.Url)));
            MessageBuilders.Add(MessageBuilder.Group(args.MemberUin).Text(image.Url));
        }
        await args.MessageBuilder.MultiMsg(MessageBuilders.ToArray()).Reply();
    }

    private async Task<Response> GetImageUrlAsync(string prompt)
    {
        var args = new Dictionary<string, string>
        {
            { "prompt", prompt },
            { "size", "2" },
            { "style", "100" },
            { "llm", "true" }
        };
        var result = await HttpUtils.PostAsync(Url, args);
        return result.ToObject<Response>()!;
    }

}

public class Response
{
    [JsonProperty("code")]
    public int Code { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;

    #nullable disable
    [JsonProperty("data")]
    public JToken Data { get; set; }
    #nullable enable
}

public class ImageContent
{
    [JsonProperty("width")]
    public int Width { get; set; }

    [JsonProperty("height")]
    public int Height { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;
}