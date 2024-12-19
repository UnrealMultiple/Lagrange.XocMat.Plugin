

using Lagrange.Core;
using Lagrange.Core.Message;
using Lagrange.XocMat;
using Lagrange.XocMat.Commands;
using Lagrange.XocMat.EventArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Permission;
using Lagrange.XocMat.Plugin;
using Lagrange.XocMat.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace Music;

public class Music(ILogger logger, CommandManager cmdManager, BotContext bot) : XocMatPlugin(logger, cmdManager, bot)
{
    public override string Name => "Music";

    public override string Description => "提供点歌功能";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 0);

    public override void Initialize()
    { 
        cmdManager.AddGroupCommand(new("点歌", MusicCmd, OneBotPermissions.Music));
        cmdManager.AddGroupCommand(new("选", ChageMusic, OneBotPermissions.Music));
    }

    protected override void Dispose(bool dispose)
    {
        cmdManager.GroupCommandDelegate.RemoveAll(x => x.CallBack == MusicCmd);
        cmdManager.GroupCommandDelegate.RemoveAll(x => x.CallBack == ChageMusic);
    }


    #region 点歌
    private async ValueTask MusicCmd(CommandArgs args)
    {
        if (args.Parameters.Count > 0)
        {
            var musicName = string.Join(" ", args.Parameters);
            if (args.Parameters[0] == "网易")
            {
                if (args.Parameters.Count > 1)
                {
                    try
                    {
                        await args.EventArgs.Reply(MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value).MarkdownImage(await MusicTool.GetMusic163Markdown(musicName[2..])));
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"点歌错误:{ex.Message}");
                    }
                    MusicTool.ChangeName(musicName[2..], args.EventArgs.Chain.GroupMemberInfo!.Uin);
                    MusicTool.ChangeLocal("网易", args.EventArgs.Chain.GroupMemberInfo!.Uin);
                }
                else
                {
                    await args.EventArgs.Reply("请输入一个歌名!");
                }
            }
            else if (args.Parameters[0] == "QQ")
            {
                if (args.Parameters.Count > 1)
                {
                    await args.EventArgs.Reply(MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value).MarkdownImage(await MusicTool.GetMusicQQMarkdown(musicName[2..])));
                    MusicTool.ChangeName(musicName[2..], args.EventArgs.Chain.GroupMemberInfo!.Uin);
                    MusicTool.ChangeLocal("QQ", args.EventArgs.Chain.GroupMemberInfo!.Uin);
                }
                else
                {
                    await args.EventArgs.Reply("请输入一个歌名!");
                }
            }
            else
            {
                var type = MusicTool.GetLocal(args.EventArgs.Chain.GroupMemberInfo!.Uin);
                if (type == "网易")
                {
                    try
                    {
                        await args.EventArgs.Reply(MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value).MarkdownImage(await MusicTool.GetMusic163Markdown(musicName)));
                    }
                    catch (Exception ex)
                    {
                        await args.EventArgs.Reply(ex.Message);
                    }
                    MusicTool.ChangeName(musicName, args.EventArgs.Chain.GroupMemberInfo!.Uin);
                }
                else
                {

                    await args.EventArgs.Reply(MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value).MarkdownImage(await MusicTool.GetMusicQQMarkdown(musicName)));
                    MusicTool.ChangeName(musicName, args.EventArgs.Chain.GroupMemberInfo!.Uin);
                }

            }
        }
        else
        {
            await args.EventArgs.Reply("请输入一个歌名!");
        }
    }
    #endregion

    #region 选歌
    private async ValueTask ChageMusic(CommandArgs args)
    {
        if (args.Parameters.Count > 0)
        {
            var musicName = MusicTool.GetName(args.EventArgs.Chain.GroupMemberInfo!.Uin);
            if (musicName != null)
            {
                if (int.TryParse(args.Parameters[0], out int id))
                {
                    if (MusicTool.GetLocal(args.EventArgs.Chain.GroupMemberInfo!.Uin) == "QQ")
                    {
                        try
                        {
                            var music = await MusicTool.GetMusicQQ(musicName, id);
                            var json = MusicSigner.Sign(new("qq", music!.Url, music.Music, music.Picture, music.Song, music.Singer));
                            if (json != null)
                                await args.EventArgs.Reply( MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value).LightApp(json));
                        }
                        catch (Exception ex)
                        {

                            await args.EventArgs.Reply(ex.Message);

                        }
                    }
                    else
                    {
                        try
                        {
                            var music = await MusicTool.GetMusic163(musicName, id);
                            var json = MusicSigner.Sign(new("163", music!.JumpUrl, music.MusicUrl, music.Picture, music.Name, string.Join(",", music.Singers)));
                            if (json != null)
                                await args.EventArgs.Reply(MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value).LightApp(json));
                           
                        }
                        catch (Exception ex)
                        {
                            await args.EventArgs.Reply(ex.Message);
                        }
                    }

                }
                else
                {
                    await args.EventArgs.Reply("请输入一个正确的序号!");
                }

            }
            else
            {
                await args.EventArgs.Reply("请先点歌!");
            }

        }
        else
        {
            await args.EventArgs.Reply("请输入一个正确的序号!");
        }
    }
    #endregion  
}
