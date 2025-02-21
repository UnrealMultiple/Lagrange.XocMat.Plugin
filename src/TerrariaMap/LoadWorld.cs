using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message.Entity;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace TerrariaMap;

public class LoadWorld : Command
{
    public override string HelpText => "获取Terraria地图";
    public override string[] Alias => ["获取地图"];
    public override string[] Permissions => [OneBotPermissions.GenerateMap];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (UserLocation.Instance.TryGetServer(args.MemberUin, args.GroupUin, out var server) && server != null)
        {
            var file = await server.GetWorldFile();
            if (file.Status)
            {
                await args.Bot.GroupFSUpload(args.Event.Chain.GroupUin!.Value, new FileEntity(file.WorldBuffer, file.WorldName + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".wld"));
            }
            else
            {
                await args.Event.Reply("无法连接到服务器!");
            }
        }
        else
        {
            await args.Event.Reply("请切换至一个有效的服务器!");
        }
    }
}


