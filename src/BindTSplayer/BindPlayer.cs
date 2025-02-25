using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.DB.Manager;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Utility;
using Microsoft.Extensions.Logging;

namespace BindTSplayer;

public class BindPlayer : Command
{
    public override string[] Alias => ["绑定"];

    public override string HelpText => "绑定账号";

    public override string[] Permissions => ["onebot.tshock.bind"];

    private readonly Dictionary<long, List<Tuple<string, string>>> _temp = [];

    public override async Task InvokeAsync(GroupCommandArgs args, ILogger log)
    {
        if (!UserLocation.Instance.TryGetServer(args.Event.Chain.GroupMemberInfo!.Uin, args.Event.Chain.GroupUin!.Value, out var server) || server == null)
        {
            await args.Event.Reply("服务器不存在或，未切换至一个服务器！", true);
            return;
        }
        if (TerrariaUser.GetUserById(args.Event.Chain.GroupMemberInfo!.Uin, server.Name).Count >= server.RegisterMaxCount)
        {
            await args.Event.Reply($"同一个服务器上绑定账户不能超过{server.RegisterMaxCount}个", true);
            return;
        }

        if (args.Parameters.Count == 1)
        {
            var userName = args.Parameters[0];
            var account = await server.QueryAccount();
            if (!account.Status || !account.Accounts.Any(x => x.Name == userName))
            {
                await args.Event.Reply($"没有在服务器中找到{userName}账户，无法绑定!", true);
                return;
            }
            var token = Guid.NewGuid().ToString()[..8];
            AddTempData(args.Event.Chain.GroupUin!.Value, userName, token);
            MailHelper.SendMail($"{args.Event.Chain.GroupMemberInfo!.Uin}@qq.com", "绑定账号验证码", $"您的验证码为: {token}");
            await args.Event.Reply($"绑定账号 {userName} => {args.Event.Chain.GroupMemberInfo.Uin} 至{server.Name}服务器!" +
                $"\n请在之后进行使用/绑定 验证 [令牌]" +
                $"\n验证令牌已发送至你的邮箱点击下方链接可查看" +
                $"\nhttps://wap.mail.qq.com/home/index");
        }
        else if (args.Parameters.Count == 2 && args.Parameters[0] == "验证")
        {
            if (GetTempData(args.Event.Chain.GroupUin!.Value, args.Parameters[1], out var name) && !string.IsNullOrEmpty(name))
            {
                try
                {
                    TerrariaUser.Add(args.Event.Chain.GroupMemberInfo.Uin, args.Event.Chain.GroupUin!.Value, server.Name, name, "");
                    await args.Event.Reply($"验证完成！\n已绑定账号至{server.Name}服务器!", true);
                }
                catch (Exception ex)
                {
                    await args.Event.Reply(ex.Message, true);
                }
            }
            else
            {
                await args.Event.Reply($"请先输入{args.CommamdPrefix}{args.Name} [名称] 在进行验证", true);
            }
        }
    }


    public void AddTempData(long groupid, string name, string token)
    {
        if (_temp.TryGetValue(groupid, out var list) && list != null)
        {
            list.Add(new Tuple<string, string>(name, token));
        }
        else
        {
            _temp[groupid] = [new Tuple<string, string>(name, token)];
        }
    }

    public bool GetTempData(long groupid, string token, out string? name)
    {
        if (_temp.TryGetValue(groupid, out var list) && list != null)
        {
            var res = list.Find(x => x.Item2 == token);
            name = res?.Item1;
            return res != null;
        }
        name = null;
        return false;
    }
}
