using DeepSeek.Core.Models;
using Lagrange.Core;
using Lagrange.Core.Message.Entity;
using Lagrange.Core.Message;
using Lagrange.XocMat;
using Lagrange.XocMat.Commands;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Plugin;
using Microsoft.Extensions.Logging;

namespace DeepSeek;

public class Plugin(ILogger logger, CommandManager commandManager, BotContext bot) : XocMatPlugin(logger, commandManager, bot)
{
    private readonly Utils _utils = new();
    public override void Initialize()
    {
        BotContext.Invoker.OnGroupMessageReceived += Invoker_OnGroupMessageReceived;
        CommandManager.AddGroupCommand(new("清除上下文", ClearMemberChat, "onebot.chat.deepseek"));
    }

    private async ValueTask ClearMemberChat(CommandArgs args)
    {
        _utils.ClearChat(args.EventArgs.Chain.GroupMemberInfo!.Uin);
        await args.EventArgs.Reply("上下文已清除", true);
    }

    private void Invoker_OnGroupMessageReceived(BotContext context, Lagrange.Core.Event.EventArg.GroupMessageEvent e)
    {
        if (e.Chain.GetMention().Any(x => x.Uin == context.BotUin))
        { 
            Task.Factory.StartNew(async () =>
            {
                if (e.Chain.GetMention().Any(x => x.Uin == context.BotUin))
                {
                    var text = e.Chain.GetText();
                    if (string.IsNullOrEmpty(text))
                    {
                        await e.Reply("内容不能为空", true);
                        return;
                    }
                    try
                    {
                        
                        var res = Config.Instance.UseContext ? await _utils.ChatContent(e.Chain.GroupMemberInfo!.Uin, text) : await _utils.Chatt(text);
                        var builder = MessageBuilder.Group(e.Chain.GroupUin!.Value)
                            .MultiMsg(MessageBuilder.Friend(e.Chain.GroupMemberInfo!.Uin).MultiMsg(MessageBuilder.Friend(e.Chain.GroupMemberInfo!.Uin).Markdown(new MarkdownData() { Content = res })));
                        await e.Reply(builder);
                    
                    }
                    catch (Exception ex)
                    {
                        await e.Reply(ex.Message, true);
                    }
                }
            });
        }
    }

    protected override void Dispose(bool dispose)
    {
        BotContext.Invoker.OnGroupMessageReceived -= Invoker_OnGroupMessageReceived;
        CommandManager.GroupCommandDelegate.RemoveAll(x => x.CallBack == ClearMemberChat);
    }
}
