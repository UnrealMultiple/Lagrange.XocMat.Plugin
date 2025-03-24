using Lagrange.Core.Message;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Utility;
using Microsoft.Extensions.Logging;

namespace Reply;

public class ImageUpLoadCommand : Command
{
    public override string[] Alias => ["上传"];
    public override string HelpText => "取图片url或上传";
    public override string[] Permissions => ["onebot.image.admin"];

    public override async Task InvokeAsync(GroupCommandArgs args, ILogger log)
    {
        var image = args.Event.Chain.GetImage();
        if (!image.Any())
        {
            await args.Event.Reply("没有图片", true);
            return;
        }
        var tasks = image.Select(async i =>
        {
            var buffer = await HttpUtils.GetByteAsync(i.ImageUrl);
            if (!Directory.Exists("images"))
            {
                Directory.CreateDirectory("images");
            }
            var path = Path.Combine("images", i.FilePath);
            await File.WriteAllBytesAsync(path, buffer);
            return (buffer, path);

        });
        var results = await Task.WhenAll(tasks);
        var builders = results.Select(r => MessageBuilder.Friend(args.MemberUin).Image(r.buffer).Text("保存路径: " + r.path));
        await args.MessageBuilder.MultiMsg([.. builders]).Reply();
    }
}
