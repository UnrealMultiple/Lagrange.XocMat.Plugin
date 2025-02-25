using Lagrange.Core.Message;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;
using Lagrange.XocMat.Utility;
using Microsoft.Extensions.Logging;

namespace Music.Commands;

public class ChageMusic : Command
{
    public override string HelpText => "选歌";
    public override string[] Alias => ["选"];
    public override string[] Permissions => [OneBotPermissions.Music];

    public override async Task InvokeAsync(GroupCommandArgs args, ILogger log)
    {
        if (args.Parameters.Count > 0)
        {
            var musicName = MusicTool.GetName(args.Event.Chain.GroupMemberInfo!.Uin);
            if (musicName != null)
            {
                if (int.TryParse(args.Parameters[0], out int id))
                {
                    if (MusicTool.GetLocal(args.Event.Chain.GroupMemberInfo!.Uin) == "QQ")
                    {
                        try
                        {
                            var music = await MusicTool.GetMusicQQ(musicName, id);
                            var json = MusicSigner.Sign(new("qq", music.PageUrl, music.PlayUrl, music.Album.Picture, music.Name, string.Join(",", music.Singer.Select(i => i.Name))));

                            if (json != null)
                                await args.Event.Reply(MessageBuilder.Group(args.Event.Chain.GroupUin!.Value).LightApp(json));
                        }
                        catch (Exception ex)
                        {
                            await args.Event.Reply(ex.ToString());
                        }
                    }
                    else
                    {
                        try
                        {
                            var music = await MusicTool.GetMusic163(musicName, id);
                            var json = MusicSigner.Sign(new("163", music!.JumpUrl, music.MusicUrl, music.Picture, music.Name, string.Join(",", music.Singers)));
                            if (json != null)
                                await args.Event.Reply(MessageBuilder.Group(args.Event.Chain.GroupUin!.Value).LightApp(json));
                        }
                        catch (Exception ex)
                        {
                            await args.Event.Reply(ex.Message);
                        }
                    }
                }
                else
                {
                    await args.Event.Reply("请输入一个正确的序号!");
                }
            }
            else
            {
                await args.Event.Reply("请先点歌!");
            }
        }
        else
        {
            await args.Event.Reply("请输入一个正确的序号!");
        }
    }
}
