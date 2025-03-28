using Lagrange.Core.Message;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;
using Lagrange.XocMat.Utility.Images;
using Microsoft.Extensions.Logging;

namespace Music.Commands;

public class MusicCmd : Command
{
    public override string HelpText => "点歌";
    public override string[] Alias => ["点歌"];
    public override string[] Permissions => [OneBotPermissions.Music];


    public override async Task InvokeAsync(GroupCommandArgs args, ILogger log)
    {
        var MusicType = "QQ";
        if (args.Parameters.Count == 0)
        {
            await args.Event.Reply("请输入一个歌名!", true);
            return;
        }
        string musicName;
        switch (args.Parameters[0])
        {
            case "QQ":
            case "网易":
                musicName = args.Parameters[1..].JoinToString(" ");
                MusicType = args.Parameters[0];
                break;
            default:
                musicName = args.Parameters[0];
                MusicType = MusicTool.GetLocal(args.MemberUin);
                break;

        }
        
        MusicTool.ChangeLocal(MusicType, args.MemberUin);
        MusicTool.ChangeName(musicName, args.MemberUin);
        var builder = ListBuilder.Create()
            .SetMemberUin(args.MemberUin);

        if (MusicType == "网易")
        {
            builder.SetTitle("网易音乐");
            var musics = await MusicTool.GetMusic163List(musicName);
            for (int i = 0; i < musics.Count; i++)
            {
                var music = musics[i];
                builder.AddItem($"{i + 1}.{music.Name} -- {music.Singers.JoinToString(",")}");
            }
        }
        else
        {
            builder.SetTitle("QQ音乐");
            var musics = await MusicTool.GetMusicQQList(musicName);
            for (int i = 0; i < musics.Count; i++)
            {
                var music = musics[i];
                builder.AddItem($"{i + 1}.{music.Name} -- {music.Singer.Select(s => s.Name).JoinToString(",")}");
            }
        }
       await args.MessageBuilder.Image(builder.Build()).Reply();
    }
}
