using Lagrange.Core.Common.Interface.Api;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;
using Microsoft.Extensions.Logging;
using Music.WangYi;

namespace Music.Commands;

public class DownloadPlay : Command
{
    public override string HelpText => "下载歌单";
    public override string[] Alias => ["下载歌单"];
    public override string[] Permissions => [OneBotPermissions.Music];

    public override async Task InvokeAsync(GroupCommandArgs args, ILogger log)
    {
        if (long.TryParse(args.Parameters[0], out var id))
        {
            try
            {
                await args.Event.Reply("正在下载歌单中的歌曲...");
                var source = MusicTool.GetLocal(args.Event.Chain.GroupMemberInfo!.Uin);
                var buffer = source switch
                {
                    "QQ" => await Config.Instance.MusicQQ.DownloadPlaylists(id),
                    "网易" => await Music_163.DownloadPlaylists(id),
                    _ => throw new("未知的音乐源")
                };
                await args.Bot.GroupFSUpload(args.Event.Chain.GroupUin!.Value, new(buffer, $"{source}歌单[{id}].zip"));
            }
            catch (Exception ex)
            {
                await args.Event.Reply(ex.Message);
            }
        }
        else
        {
            await args.Event.Reply("请输入一个正确的歌单ID!");
        }
    }
}
