using Lagrange.Core;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.XocMat.Commands;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Plugin;
using Lagrange.XocMat.Utility;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json.Nodes;

namespace Bilibili;

public partial class Bilibili(ILogger logger, CommandManager cmd, BotContext bot) : XocMatPlugin(logger, cmd, bot)
{
    public override string Name => "Bilibili插件";

    public override string Description => "提供Bilibili相关功能";

    public override string Author => "不知名作者 少司命更改";

    public override Version Version => new(1, 0, 0, 0);

    private HttpClient _httpClient = null!;

    private async Task<MessageBuilder> ParseVideo(string parseUrl, string id, MessageBuilder builder)
    {
        try
        {
            //var url = $"https://api.bilibili.com/x/web-interface/view?aid={aid}";
            var response = await _httpClient.GetAsync(parseUrl);
            response.EnsureSuccessStatusCode();
            var json = JsonNode.Parse(await response.Content.ReadAsStringAsync());
            var code = (int?)json?["code"] ?? throw new Exception("code is null.");
            if (code != 0)
            {
                throw new Exception($"code is {code}.");
            }
            var data = json?["data"] ?? throw new Exception("data is null.");
            var title = data["title"]?.GetValue<string>() ?? throw new Exception("data.title is null.");
            var pic = data["pic"]?.GetValue<string>() ?? throw new Exception("data.pic is null.");
            //var picStream = await _httpClient.GetStreamAsync(pic);
            var owner = data["owner"] ?? throw new Exception("data.owner is null.");
            var ownerName = owner["name"]?.GetValue<string>() ?? throw new Exception("data.owner.name is null.");
            var ctime = data["ctime"]?.GetValue<long>() ?? throw new Exception("data.ctime is null.");
            var stat = data["stat"] ?? throw new Exception("data.stat is null.");
            var view = stat["view"]?.GetValue<int>() ?? throw new Exception("data.stat.view is null.");
            var like = stat["like"]?.GetValue<int>() ?? throw new Exception("data.stat.like is null.");
            var coin = stat["coin"]?.GetValue<int>() ?? throw new Exception("data.stat.coin is null.");
            var favorite = stat["favorite"]?.GetValue<int>() ?? throw new Exception("data.stat.favorite is null.");
            var share = stat["share"]?.GetValue<int>() ?? throw new Exception("data.stat.share is null.");
            var danmaku = stat["danmaku"]?.GetValue<int>() ?? throw new Exception("data.stat.danmaku is null.");
            var reply = stat["reply"]?.GetValue<int>() ?? throw new Exception("data.stat.reply is null.");
            var sb = new StringBuilder();
            sb.AppendLine($"https://www.bilibili.com/video/{id}");
            sb.AppendLine($"发布时间：{ctime.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")}");
            sb.Append($"UP主:{ownerName} | ");
            sb.Append($"播放量:{view} | ");
            sb.Append($"点赞:{like} | ");
            sb.Append($"投币:{coin} | ");
            sb.Append($"收藏:{favorite} | ");
            sb.Append($"转发:{share} | ");
            sb.Append($"弹幕:{danmaku} ");
            sb.Append($"评论:{reply} ");
            builder.Image(HttpUtils.HttpGetByte(pic).Result);
            builder.Text(title);
            builder.Text(sb.ToString().Trim());

            try
            {
                var aid2 = data["aid"]?.GetValue<string>() ?? throw new Exception("data.aid is null.");
                var url2 = $"https://api.bilibili.com/x/v2/reply?type=1&sort=1&ps=1&oid={aid2}";
                var response2 = await _httpClient.GetAsync(url2);
                response2.EnsureSuccessStatusCode();
                var json2 = JsonNode.Parse(await response2.Content.ReadAsStringAsync());
                var code2 = (int?)json2?["code"] ?? throw new Exception("code is null.");
                if (code2 != 0)
                {
                    throw new Exception("code is not 0.");
                }
                var data2 = json2?["data"] ?? throw new Exception("data is null.");
                var replies2_ = data2["replies"] ?? throw new Exception("data.replies is null.");
                if (replies2_ != null)
                {
                    var replies2 = replies2_.AsArray() ?? throw new Exception("data.replies is null.");
                    if (replies2.Count > 0)
                    {
                        var reply2 = replies2[0] ?? throw new Exception("data.reply[0] is null.");
                        var content2 = reply2["content"] ?? throw new Exception("data.reply[0].content is null.");
                        var message2 = content2["message"]?.GetValue<string>() ?? throw new Exception("data.reply[0].content.message is null.");
                        var pictures = content2["pictures"]?.AsArray() ?? throw new Exception("data.reply[0].content.pictures is null.");
                        var sb2 = new StringBuilder();
                        sb2.AppendLine("热评：");
                        sb2.AppendLine(message2);
                        builder.Text(sb2.ToString().Trim());

                        for (int i = 0; i < pictures.Count; i++)
                        {
                            var picture = pictures[i] ?? throw new Exception($"data.reply[0].content.pictures[{i}] is null.");
                            var imgsrc = picture["img_src"]?.GetValue<string>() ?? throw new Exception($"data.reply[0].content.pictures[{i}].img_src is null.");
                            builder.Image(HttpUtils.HttpGetByte(imgsrc).Result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"热评获取失败：{ex.Message}");
            }

            return builder;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            builder.Text(ex.Message);
        }
        return builder;
    }

    public override void Initialize()
    {
        _httpClient = new HttpClient();
        bot.Invoker.OnGroupMessageReceived += Event_OnGroupMessage;
        Logger.LogInformation("Plugin Bilibili Initiate Successfully!");
    }

    private void Event_OnGroupMessage(BotContext bot, GroupMessageEvent args)
    {
        if (args.Chain.GroupMemberInfo!.Uin == bot.BotUin)
        {
            return;
        }
        var text = args.Chain.GetText();
        if (args.Chain.FirstOrDefault(x => x is LightAppEntity) is LightAppEntity json)
        {
            var data = JsonNode.Parse(json.Payload);
            var appid = data?["meta"]?["detail_1"]?["appid"];
            if (appid?.ToString() == "1109937557")
                text = (data?["meta"]?["detail_1"]?["qqdocurl"])?.ToString();
        }
        var b23 = BilibiliHelpers.B23_Regex().Match(text);
        var bv = BilibiliHelpers.Bv_Regex().Match(text);
        var av = BilibiliHelpers.Av_Regex().Match(text);
        if (b23.Success)
        {
            try
            {
                var _b23 = b23.Groups["B23"].Value;
                var url = $"https://{_b23}";
                var response =  _httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                var path = response.RequestMessage?.RequestUri?.OriginalString ?? throw new Exception("request uri is null.");
                var bvid = BilibiliHelpers.BVIDRegex().Match(path).Groups["BVID"].Value;
                url = $"https://api.bilibili.com/x/web-interface/view?bvid={bvid}";
                var message = ParseVideo(url, bvid, MessageBuilder.Group(args.Chain.GroupUin!.Value)).Result;
                if (message is null)
                {
                    return;
                }
                args.Reply(message);
            }
            catch (Exception ex)
            {
                args.Reply(ex.Message);
            }
        }
        else if (bv.Success)
        {
            var bvid = bv.Groups["BVID"].Value;
            var url = $"https://api.bilibili.com/x/web-interface/view?bvid={bvid}";

            var message = ParseVideo(url, bvid, MessageBuilder.Group(args.Chain.GroupUin!.Value)).Result;
            if (message is null)
            {
                return;
            }
            args.Reply(message);
        }
        else if (av.Success)
        {
            var sAid = av.Groups["AID"].Value;
            if (!int.TryParse(sAid, out var aid))
            {
                return;
            }
            var url = $"https://api.bilibili.com/x/web-interface/view?aid={aid}";

            var message = ParseVideo(url, $"av{aid}", MessageBuilder.Group(args.Chain.GroupUin!.Value)).Result;
            if (message is null)
            {
                return;
            }
            args.Reply(message);
        }

    }

    protected override void Dispose(bool dispose)
    {
        if (dispose)
        {
            _httpClient.Dispose();
            bot.Invoker.OnGroupMessageReceived -= Event_OnGroupMessage;
        }
        base.Dispose();
    }
}