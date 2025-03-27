using System.Text;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.XocMat;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Plugin;
using Lagrange.XocMat.Utility;
using Microsoft.Extensions.Logging;

namespace DetectTShock;

public class Plugin(ILogger logger, BotContext bot) : XocMatPlugin(logger, bot)
{
    public override string Name => "DetetTShock";

    public override string Description => "提供一些乱七八糟的功能";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 0);

    protected override void Dispose(bool dispose)
    {
        throw new NotImplementedException();
    }

    protected override void Initialize()
    {
        BotContext.Invoker.OnGroupMessageReceived += OnGroupMessageReceived;
    }

    public async Task WriteFiles(string dir, List<(string name, byte[] buffer)> files)
    {
        await Task.WhenAll(files.Select(async file =>
        {
            var path = Path.Combine(dir, file.name);
            await File.WriteAllBytesAsync(path, file.buffer);
        }));
    }

    public void DeleteFiles(string dir, List<(string name, byte[] buffer)> files)
    {
        foreach(var file in files)
        {
            var path = Path.Combine(dir, file.name);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }   
    }

    private void OnGroupMessageReceived(BotContext context, GroupMessageEvent e)
    {
        var file = e.Chain.GetFile();
        var dirInfo = new DirectoryInfo(Config.Instance.DetectPath);
        if (!dirInfo.Exists)
        {
            dirInfo.Create();
        }
        Task.Factory.StartNew(async () =>
        {
            if (file != null && !string.IsNullOrEmpty(file.FileUrl) && file.FileSize < Config.Instance.FileSizeLimit)
            {
                try
                {
                    var buffer = await HttpUtils.GetByteAsync(file.FileUrl);
                    var files = SecureDllExtractor.ExtractDllFiles(buffer);
                    if (files.Count == 0)
                    {
                        return;
                    }
                    var msg = MessageBuilder.Group(e.Chain.GroupUin!.Value);
                    var tasks = dirInfo.GetDirectories().Select(async dir =>
                    {
                        await WriteFiles(Path.Combine(dir.FullName, "ServerPlugins"), files);
                        var result = ProcessUtils.StartProcess(Path.Combine(dir.FullName, Config.Instance.DetectProgram));
                        DeleteFiles(Path.Combine(dir.FullName, "ServerPlugins"), files);
                        return MessageBuilder.Friend(e.Chain.GroupMemberInfo!.Uin)
                            .MultiMsg([MessageBuilder.Friend(e.Chain.GroupMemberInfo!.Uin)
                                    .Text(dir.Name)
                                    .Markdown(new MarkdownData(){ Content = $"```\n{result.StandardOutput}\n```" })]);
                    });
                    var mul = await Task.WhenAll(tasks);
                    await e.Reply(msg.MultiMsg([..mul]));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred while processing file");

                }
            }
    });
        
    }
}
