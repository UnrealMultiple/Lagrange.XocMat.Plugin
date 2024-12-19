using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message.Entity;
using Lagrange.XocMat;
using Lagrange.XocMat.Commands;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Permission;
using Lagrange.XocMat.Plugin;
using Lagrange.XocMat.Terraria;
using Lagrange.XocMat.Utility;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TerrariaMap;

public class TerrariaMap(ILogger logger, CommandManager cmd, BotContext bot) : XocMatPlugin(logger, cmd, bot)
{
    public override string Name => "TerrariaMap";

    public override string Description => "生成TerrariaMap的插件";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 0);

    public string SavePath = Path.Combine(XocMatAPI.SAVE_PATH, "TerrariaMap.json");

    public override void Initialize()
    {
        cmd.AddGroupCommand(new("获取地图", LoadWorld, OneBotPermissions.GenerateMap));
        bot.Invoker.OnGroupMessageReceived += Event_OnGroupMessage;
    }

    private void Event_OnGroupMessage(BotContext context, GroupMessageEvent e)
    {
        if (e.Chain.GroupMemberInfo!.Uin != bot.BotUin && e.Chain.FirstOrDefault(x => x is FileEntity) is FileEntity file)
        {
            if (file.FileSize > 1024 * 1024 * 30)
                return;
            if (!string.IsNullOrEmpty(file.FileUrl))
            {
                var buffer = HttpUtils.HttpGetByte(file.FileUrl).Result;
                if (TerrariaServer.IsReWorld(buffer))
                {
                    e.Reply("检测到Terraria地图，正在生成.map文件....");
                    var uuid = Guid.NewGuid().ToString();
                    Spawn(uuid);
                    var (name, data) = IPCO.Start(uuid, buffer);
                    context.GroupFSUpload(e.Chain.GroupUin!.Value, new FileEntity(data, name));
                }
            }
                
        }
    }


    private async ValueTask LoadWorld(CommandArgs args)
    {
        if (UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) && server != null)
        {
            var file = await server.GetWorldFile();
            if (file.Status)
            {
                await args.Bot.GroupFSUpload(args.EventArgs.Chain.GroupUin!.Value,new FileEntity(file.WorldBuffer, file.WorldName + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".wld"));
            }
            else
            {
                await args.EventArgs.Reply("无法连接到服务器!");
            }
        }
        else
        {

            await args.EventArgs.Reply("请切换至一个有效的服务器!");
        }
    }

    private void Spawn(string uuid)
    {
        Process process = new();
        process.StartInfo.WorkingDirectory = Config.Instance.AppPath;
        process.StartInfo.FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "TerrariaMap.exe" : "TerrariaMap";
        process.StartInfo.Arguments = "-mapname " + uuid;
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.RedirectStandardInput = false;
        process.StartInfo.RedirectStandardOutput = false;
        process.StartInfo.RedirectStandardError = false;
        process.StartInfo.CreateNoWindow = true;
        if (process.Start())
        {
            process.Close();
        }
    }

    protected override void Dispose(bool dispose)
    {
        bot.Invoker.OnGroupMessageReceived -= Event_OnGroupMessage;
        cmd.GroupCommandDelegate.RemoveAll(x => x.CallBack == LoadWorld);
    }
}
