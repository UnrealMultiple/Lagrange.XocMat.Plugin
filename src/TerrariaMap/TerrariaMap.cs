using System.Diagnostics;
using System.Runtime.InteropServices;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message.Entity;
using Lagrange.XocMat;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Plugin;
using Lagrange.XocMat.Terraria;
using Lagrange.XocMat.Utility;
using Microsoft.Extensions.Logging;

namespace TerrariaMap;

public class TerrariaMap(ILogger logger, BotContext bot) : XocMatPlugin(logger, bot)
{
    public override string Name => "TerrariaMap";

    public override string Description => "生成TerrariaMap的插件";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 0);

    public string SavePath = Path.Combine(XocMatAPI.SAVE_PATH, "TerrariaMap.json");

    protected override void Initialize()
    {
        BotContext.Invoker.OnGroupMessageReceived += Event_OnGroupMessage;
    }

    private void Event_OnGroupMessage(BotContext context, GroupMessageEvent e)
    {
        Task.Factory.StartNew(async () =>
        {
            if (e.Chain.GroupMemberInfo!.Uin != context.BotUin && e.Chain.FirstOrDefault(x => x is FileEntity) is FileEntity file)
            {
                if (file.FileSize > 1024 * 1024 * 30)
                    return;
                if (!string.IsNullOrEmpty(file.FileUrl))
                {
                    var buffer = HttpUtils.GetByteAsync(file.FileUrl).Result;
                    if (TerrariaServer.IsReWorld(buffer))
                    {
                        await e.Reply("检测到Terraria地图，正在生成.map文件....");
                        var uuid = Guid.NewGuid().ToString();
                        Spawn(uuid);
                        var (name, data) = IPCO.Start(uuid, buffer);
                        await context.GroupFSUpload(e.Chain.GroupUin!.Value, new FileEntity(data, name));
                    }
                }

            }
        });

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
        BotContext.Invoker.OnGroupMessageReceived -= Event_OnGroupMessage;
    }
}
