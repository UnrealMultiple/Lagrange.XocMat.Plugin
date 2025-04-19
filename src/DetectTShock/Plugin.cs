using Lagrange.Core;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
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
    
    public readonly static HashSet<string> compressedFileExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".zip", ".rar", ".7z", 
       ".tar.gz", ".tar.bz2", ".tar.xz",
       ".gz", ".bz2", ".xz"
    };

    private bool _isRunning = false;

    protected override void Dispose(bool dispose)
    {
        throw new NotImplementedException();
    }

    protected override void Initialize()
    {
        BotContext.Invoker.OnGroupMessageReceived += OnGroupMessageReceived;
    }

    public static async Task WriteFiles(string dir, List<(string name, byte[] buffer)> files)
    {
        await Task.WhenAll(files.Select(async file =>
        {
            if(Path.GetFileNameWithoutExtension(file.name).Equals("tshockapi", StringComparison.CurrentCultureIgnoreCase))
                return;
            var path = Path.Combine(dir, file.name);
            await File.WriteAllBytesAsync(path, file.buffer);
        }));
    }

    public static void DeleteFiles(string dir)
    {
        if(!Directory.Exists(dir)) return;
        foreach(var file in Directory.GetFiles(dir))
        {
            if (Path.GetFileNameWithoutExtension(file).Equals("tshockapi", StringComparison.CurrentCultureIgnoreCase))
                continue;
            File.Delete(file);
        }
    }

    private void OnGroupMessageReceived(BotContext context, GroupMessageEvent e)
    {
        if (_isRunning)
        {
            e.Reply("当前有个任务正在运行!请等待结束后重试!");
            return;
        }
        var file = e.Chain.GetFile();
        if (file == null) return;
        if (!compressedFileExtensions.Contains(Path.GetExtension(file.FileName))) return;

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
                    _isRunning = true;
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
                        DeleteFiles(Path.Combine(dir.FullName, "ServerPlugins"));
                        var segments = result.StandardOutput.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
                        var markdownContents = segments.Split(100).Select(segment =>
                        {
                            return MessageBuilder.Friend(e.Chain.GroupMemberInfo!.Uin)
                                .Text(dir.Name)
                                .Markdown(new MarkdownData() { Content = $"```\n{segment.JoinToString("\n")}\n```" });
                        }); 
                        return MessageBuilder.Friend(e.Chain.GroupMemberInfo!.Uin)
                            .MultiMsg([..markdownContents]);
                    });
                    var mul = await Task.WhenAll(tasks);
                    await e.Reply(msg.MultiMsg([..mul]));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred while processing file");

                }
                finally
                {
                    _isRunning = false;
                }
            }
    });
        
    }
}