using Lagrange.Core.Message;
using Lagrange.XocMat;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Utility;
using Microsoft.Extensions.Logging;

namespace Reply;

public class ImageCommand : Command
{
    public override string[] Alias => ["image"];
    public override string HelpText => "取图片url或上传";
    public override string[] Permissions => ["onebot.image.admin"];

    private static string _imagePath => Path.Combine(XocMatAPI.PATH, "images");

    private static Dictionary<string, Func<GroupCommandArgs, ILogger, Task>> _action { get; set; } = new()
    {
        { "upload", Upload },
        { "list", Images },
        { "del", Delete },
        { "url", GenerateUrl }
    };

    private static async Task GenerateUrl(GroupCommandArgs args, ILogger logger)
    {
        var image = args.Event.Chain.GetImage();
        if (!image.Any())
        {
            await args.Event.Reply("请携带一张或多张图片!", true);
            return;
        }
        var tasks = image.Select(async i =>
        {
            var buffer = await HttpUtils.GetByteAsync(i.ImageUrl);
            return (buffer, i.ImageUrl);

        });
        var results = await Task.WhenAll(tasks);
        var builders = results.Select(r => MessageBuilder.Friend(args.MemberUin).Image(r.buffer).Text(r.ImageUrl));
        await args.MessageBuilder.MultiMsg([.. builders]).Reply();
    }

    private static async Task Delete(GroupCommandArgs args, ILogger logger)
    {
        if(args.Parameters.Count < 2)
        {
            await args.Event.Reply("请指定要删除的图片", true);
            return;
        }
        var name = args.Parameters[1];
        var path = Path.Combine(_imagePath, name);
        if (!File.Exists(path))
        {
            await args.Event.Reply("图片不存在", true);
            return;
        }
        File.Delete(path);
        await args.Event.Reply("删除成功", true);
    }

    private static async Task Images(GroupCommandArgs args, ILogger logger)
    {
        var images = Directory.GetFiles(_imagePath);
        if (images.Length == 0)
        {
            await args.Event.Reply("没有任何图片", true);
            return;
        }

        var builders = images.Select(i => MessageBuilder.Friend(args.MemberUin).Image(FileReader.ReadFileBuffer(i)).Text("文件路径:" + Path.Combine("images", Path.GetFileName(i))));
        await args.MessageBuilder.MultiMsg([.. builders]).Reply();
    }

    private static async Task Upload(GroupCommandArgs args, ILogger logger)
    {
        var image = args.Event.Chain.GetImage();
        if (!image.Any())
        {
            await args.Event.Reply("请携带一张或多张图片!", true);
            return;
        }
        var tasks = image.Select(async i =>
        {
            var buffer = await HttpUtils.GetByteAsync(i.ImageUrl);
            if (!Directory.Exists(_imagePath))
            {
                Directory.CreateDirectory(_imagePath);
            }
            var path = Path.Combine(_imagePath, i.FilePath);
            await File.WriteAllBytesAsync(path, buffer);
            return (buffer, path);

        });
        var results = await Task.WhenAll(tasks);
        var builders = results.Select(r => MessageBuilder.Friend(args.MemberUin).Image(r.buffer).Text("保存路径: " + r.path));
        await args.MessageBuilder.MultiMsg([.. builders]).Reply();
    }

    public override async Task InvokeAsync(GroupCommandArgs args, ILogger log)
    {
        if(args.Parameters.Count > 0 && _action.TryGetValue(args.Parameters[0], out var action))
        {
            await action(args, log);
            return;
        }
        await args.Event.Reply($"语法错误，正确语法:" +
            $"\n{args.CommamdPrefix}{args.Name} upload" +
            $"\n{args.CommamdPrefix}{args.Name} list" +
            $"\n{args.CommamdPrefix}{args.Name} url" +
            $"\n{args.CommamdPrefix}{args.Name} del <文件名>", true);
    }
}
