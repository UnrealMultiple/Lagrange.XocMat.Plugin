using Lagrange.Core.Message;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;
using Microsoft.Extensions.Logging;

namespace Music.Commands;

public class MusicCmd : Command
{
    public override string HelpText => "点歌";
    public override string[] Alias => ["点歌"];
    public override string[] Permissions => [OneBotPermissions.Music];

    public override async Task InvokeAsync(GroupCommandArgs args, ILogger log)
    {
        if (args.Parameters.Count > 0)
        {
            var musicName = string.Join(" ", args.Parameters);
            if (args.Parameters[0] == "网易")
            {
                if (args.Parameters.Count > 1)
                {
                    await args.Event.Reply(MessageBuilder.Group(args.Event.Chain.GroupUin!.Value).MarkdownImage(await MusicTool.GetMusic163Markdown(musicName[2..])));
                    MusicTool.ChangeName(musicName[2..], args.Event.Chain.GroupMemberInfo!.Uin);
                    MusicTool.ChangeLocal("网易", args.Event.Chain.GroupMemberInfo!.Uin);
                }
                else
                {
                    await args.Event.Reply("请输入一个歌名!");
                }
            }
            else if (args.Parameters[0] == "QQ")
            {
                if (args.Parameters.Count > 1)
                {
                    await args.Event.Reply(MessageBuilder.Group(args.Event.Chain.GroupUin!.Value).MarkdownImage(await MusicTool.GetMusicQQMarkdown(musicName[2..])));
                    MusicTool.ChangeName(musicName[2..], args.Event.Chain.GroupMemberInfo!.Uin);
                    MusicTool.ChangeLocal("QQ", args.Event.Chain.GroupMemberInfo!.Uin);
                }
                else
                {
                    await args.Event.Reply("请输入一个歌名!");
                }
            }
            else
            {
                var type = MusicTool.GetLocal(args.Event.Chain.GroupMemberInfo!.Uin);
                if (type == "网易")
                {
                    try
                    {
                        await args.Event.Reply(MessageBuilder.Group(args.Event.Chain.GroupUin!.Value).MarkdownImage(await MusicTool.GetMusic163Markdown(musicName)));
                    }
                    catch (Exception ex)
                    {
                        await args.Event.Reply(ex.Message);
                    }
                    MusicTool.ChangeName(musicName, args.Event.Chain.GroupMemberInfo!.Uin);
                }
                else
                {
                    await args.Event.Reply(MessageBuilder.Group(args.Event.Chain.GroupUin!.Value).MarkdownImage(await MusicTool.GetMusicQQMarkdown(musicName)));
                    MusicTool.ChangeName(musicName, args.Event.Chain.GroupMemberInfo!.Uin);
                }
            }
        }
        else
        {
            await args.Event.Reply("请输入一个歌名!");
        }
    }
}
