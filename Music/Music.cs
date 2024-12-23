using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Lagrange.XocMat.Commands;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Permission;
using Lagrange.XocMat.Plugin;
using Lagrange.XocMat.Utility;
using LinqToDB;
using Microsoft.Extensions.Logging;
using Music.QQ;
using Music.WangYi;

namespace Music;

public class Music(ILogger logger, CommandManager commandManager, BotContext bot) : XocMatPlugin(logger, commandManager, bot)
{
    public override string Name => "Music";

    public override string Description => "提供点歌功能";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 0);

    public override void Initialize()
    {
        CommandManager.AddGroupCommand(new("点歌", MusicCmd, OneBotPermissions.Music));
        CommandManager.AddGroupCommand(new("音乐扫码", QrcodeLogin, "onebot.music.qrcode"));
        CommandManager.AddGroupCommand(new("选", ChageMusic, OneBotPermissions.Music));
        CommandManager.AddGroupCommand(new("下载歌单", DownloadPlay, OneBotPermissions.Music));
        CommandManager.AddGroupCommand(new("切换音源", ChangeMusicSource, OneBotPermissions.Music));
    }

    private async ValueTask ChangeMusicSource(CommandArgs args)
    {
       if(args.Parameters.Count > 0)
        {
            if (args.Parameters[0] == "QQ" || args.Parameters[0] == "网易")
            {
                MusicTool.ChangeLocal(args.Parameters[0], args.EventArgs.Chain.GroupMemberInfo!.Uin);
                await args.EventArgs.Reply($"音源已切换至{args.Parameters[0]}");
            }
            else
            {
                await args.EventArgs.Reply("请输入正确的音源!");
            }
        }
        else
        {
            await args.EventArgs.Reply("请输入正确的音源!");
        }
    }

    private async ValueTask DownloadPlay(CommandArgs args)
    {
        if (long.TryParse(args.Parameters[0], out var id))
        {
            try
            {
                await args.EventArgs.Reply("正在下载歌单中的歌曲...");
                var source = MusicTool.GetLocal(args.EventArgs.Chain.GroupMemberInfo!.Uin);
                var buffer = source switch
                {
                    "QQ" => await Config.Instance.MusicQQ.DownloadPlaylists(id),
                    "网易" => await Music_163.DownloadPlaylists(id),
                    _ => throw new("未知的音乐源")
                };
                await BotContext.GroupFSUpload(args.EventArgs.Chain.GroupUin!.Value, new(buffer, $"{source}歌单[{id}].zip"));
            }
            catch (Exception ex)
            {
                await args.EventArgs.Reply(ex.Message);
            }
        }
        else
        { 
            await args.EventArgs.Reply("请输入一个正确的歌单ID!");
        }
    }

    protected override void Dispose(bool dispose)
    {
        CommandManager.GroupCommandDelegate.RemoveAll(x => x.CallBack == MusicCmd);
        CommandManager.GroupCommandDelegate.RemoveAll(x => x.CallBack == ChageMusic);
    }

    private async ValueTask QrcodeLogin(CommandArgs args)
    {
        try
        {
            var (qrsig, buffer) = await Login.GetLoginQrcode();
            await args.EventArgs.Reply(MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value).Image(buffer).Text("请尽快扫描此二维码60秒后失效"));
            await Login.CheckLoginQrcode(qrsig, 120,  async (state, token) =>
            { 
                if(state == QQ.Enums.QrcodeLoginType.DONE)
                {
                    Config.Instance.SetToken(token);
                    Config.Instance.SaveTo();
                    var user = await Config.Instance.MusicQQ.GetUserInfo();
                    await args.EventArgs.Reply($"账户`{user.Info.BaseInfo.Name}`登录成功，欢迎回来!");
                }
                else if (state == QQ.Enums.QrcodeLoginType.TIMEOUT)
                {
                    await args.EventArgs.Reply("二维码已失效!");
                }
                else if (state == QQ.Enums.QrcodeLoginType.REFUSE)
                {
                    await args.EventArgs.Reply("拒绝登陆!");
                }
                else if (state == QQ.Enums.QrcodeLoginType.CANCEL)
                {
                    await args.EventArgs.Reply("此登陆被取消!");
                }
            });
        }
        catch(Exception ex)
        {
            await args.EventArgs.Reply(ex.Message);
        }
        
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
                        Logger.LogError($"点歌错误:{ex.Message}");
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
                            var json = MusicSigner.Sign(new("qq", music.PageUrl, music.PlayUrl, music.Album.Picture, music.Name, string.Join(",", music.Singer.Select(i=>i.Name))));
                            
                            if (json != null)
                                await args.EventArgs.Reply(MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value).LightApp(json));
                        }
                        catch (Exception ex)
                        {

                            await args.EventArgs.Reply(ex.ToString());

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
