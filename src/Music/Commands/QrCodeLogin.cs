using Lagrange.Core.Message;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Microsoft.Extensions.Logging;
using Music.QQ;

namespace Music.Commands;

public class QrCodeLogin : Command
{
    public override string HelpText => "扫码登录音乐平台";
    public override string[] Alias => ["音乐扫码"];
    public override string[] Permissions => ["onebot.music.qrcode"];

    public override async Task InvokeAsync(GroupCommandArgs args, ILogger log)
    {
        try
        {
            var (qrsig, buffer) = await Login.GetLoginQrcode();
            await args.Event.Reply(MessageBuilder.Group(args.Event.Chain.GroupUin!.Value).Image(buffer).Text("请尽快扫描此二维码60秒后失效"));
            await Login.CheckLoginQrcode(qrsig, 120, async (state, token) =>
            {
                if (state == QQ.Enums.QrcodeLoginType.DONE)
                {
                    Config.Instance.SetToken(token);
                    Config.Instance.SaveTo();
                    var user = await Config.Instance.MusicQQ.GetUserInfo();
                    await args.Event.Reply($"账户`{user.Info.BaseInfo.Name}`登录成功，欢迎回来!");
                }
                else if (state == QQ.Enums.QrcodeLoginType.TIMEOUT)
                {
                    await args.Event.Reply("二维码已失效!");
                }
                else if (state == QQ.Enums.QrcodeLoginType.REFUSE)
                {
                    await args.Event.Reply("拒绝登陆!");
                }
                else if (state == QQ.Enums.QrcodeLoginType.CANCEL)
                {
                    await args.Event.Reply("此登陆被取消!");
                }
            });
        }
        catch (Exception ex)
        {
            await args.Event.Reply(ex.Message);
        }
    }
}
