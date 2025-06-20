using Lagrange.Core;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message.Entity;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Plugin;
using Microsoft.Extensions.Logging;

namespace Reply;

public class Plugin(ILogger logger, BotContext bot) : XocMatPlugin(logger, bot)
{
    public override string Name => "Reply";

    public override string Description => "提供插件管理功能";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 0);

    protected override void Dispose(bool dispose)
    {
        ReplyAdapter.RemoveAsyncHandler("uin");
        ReplyAdapter.RemoveAsyncHandler("card");
        ReplyAdapter.RemoveAsyncHandler("groupid");
        ReplyAdapter.RemoveAsyncHandler("time");
        ReplyAdapter.RemoveContentHandler("image");
        ReplyAdapter.RemoveContentHandler("video");
        ReplyAdapter.RemoveContentHandler("face");
        ReplyAdapter.RemoveContentHandler("at");
        ReplyAdapter.RemoveContentHandler("forward");
        ReplyAdapter.RemoveContentHandler("select");
        BotContext.Invoker.OnGroupMessageReceived -= OnGroupMessageReceived;
    }

    protected override void Initialize()
    {
        ReplyAdapter.RegisterAsyncHandler("uin", (name, param, chain) => Task.FromResult(chain.GroupMemberInfo!.Uin.ToString()));
        ReplyAdapter.RegisterAsyncHandler("card", (name, param, chain) => Task.FromResult(chain.GroupMemberInfo!.MemberCard!));
        ReplyAdapter.RegisterAsyncHandler("groupid", (name, param, chain) => Task.FromResult(chain.GroupUin!.Value.ToString()));
        ReplyAdapter.RegisterAsyncHandler("time", (name, param, chain) => Task.FromResult(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
        ReplyAdapter.RegisterContentHandler("image", (type, content, chain, builder) =>
        {
            var bytes = FileReader.ReadFileBuffer(content);
            if (bytes.Length > 0)
            {
                builder.Image(bytes);
            }
            return Task.CompletedTask;
        });
        ReplyAdapter.RegisterContentHandler("video", (type, content, chain, builder) =>
        {
            var bytes = FileReader.ReadFileBuffer(content);
            if (bytes.Length > 0)
            {
                builder.Video(bytes);
            }
            return Task.CompletedTask;
        });
        ReplyAdapter.RegisterContentHandler("face", (type, content, chain, builder) =>
        {
            if (ushort.TryParse(content, out var faceId))
            {
                builder.Face(faceId);
            }
            return Task.CompletedTask;
        });
        ReplyAdapter.RegisterContentHandler("at", (type, content, chain, builder) =>
        {
            if (uint.TryParse(content, out var uin))
            {
                builder.Mention(uin);
            }
            return Task.CompletedTask;
        });
        ReplyAdapter.RegisterContentHandler("forward", (type, content, chain, builder) =>
        {
            builder.Add(new ForwardEntity(chain));
            return Task.CompletedTask;
        });
        ReplyAdapter.RegisterContentHandler("select", (type, content, chain, builder) =>
        {
            chain.GetMention().ForEach(i => builder.Mention(i.Uin));
            return Task.CompletedTask;
        });
        BotContext.Invoker.OnGroupMessageReceived += OnGroupMessageReceived;
    }
   
    
    private void OnGroupMessageReceived(BotContext bot, GroupMessageEvent e)
    {
        if(e.Chain.GroupMemberInfo!.Uin == bot.BotUin) return;
        try
        {
            var response = ReplyAdapter.ProcessMessageAsync(e.Chain).Result;
            if (response == null) return;
            bot.Reply(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "处理消息时出错");
        }
    }
}
