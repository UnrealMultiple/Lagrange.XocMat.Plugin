using System.Text;
using Lagrange.Core.Message;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace TerrariaCart;

public class CartManager : Command
{

    public override string HelpText => "管理购物车";
    public override string[] Alias => ["cart"];
    public override string[] Permissions => [OneBotPermissions.TerrariaShop];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        try
        {
            if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "add")
            {
                if (int.TryParse(args.Parameters[2], out int id))
                {
                    Config.Instance.Add(args.Event.Chain.GroupMemberInfo!.Uin, args.Parameters[1], id);
                    await args.Event.Reply("添加成功!", true);
                }
                else
                {
                    await args.Event.Reply("请填写一个正确的商品ID!", true);
                }
            }
            else if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "del")
            {
                if (int.TryParse(args.Parameters[2], out int id))
                {
                    Config.Instance.Remove(args.Event.Chain.GroupMemberInfo!.Uin, args.Parameters[1], id);
                    await args.Event.Reply("删除成功!", true);
                }
                else
                {
                    await args.Event.Reply("请填写一个正确的商品ID!", true);
                }
            }
            else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "clear")
            {
                Config.Instance.ClearCart(args.Event.Chain.GroupMemberInfo!.Uin, args.Parameters[1]);
                await args.Event.Reply("已清除购物车" + args.Parameters[1]);
            }
            else if (args.Parameters.Count == 1 && args.Parameters[0].ToLower() == "list")
            {
                var carts = Config.Instance.GetCarts(args.Event.Chain.GroupMemberInfo!.Uin);
                if (carts.Count == 0)
                {
                    await args.Event.Reply("购物车空空如也!", true);
                    return;
                }
                var sb = new StringBuilder();
                sb.AppendLine($$"""<div align="center">""");
                sb.AppendLine();
                sb.AppendLine();
                foreach (var (name, shops) in carts)
                {
                    sb.AppendLine();
                    sb.AppendLine($"# 购物车`{name}`");
                    sb.AppendLine();
                    sb.AppendLine("|商品ID|商品名称|数量|价格|");
                    sb.AppendLine("|:--:|:--:|:--:|:--:|");
                    foreach (var index in shops)
                    {
                        var shop = TerrariaShop.Instance.GetShop(index);
                        if (shop != null)
                            sb.AppendLine($"|{index}|{shop.Name}|{shop.Num}|{shop.Price}|");
                    }
                }
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("</div>");

                await args.Event.Reply(MessageBuilder.Group(args.Event.Chain.GroupUin!.Value).MarkdownImage(sb.ToString()));
            }
            else
            {
                await args.Event.Reply("语法错误,正确语法\n" +
                    $"{args.CommamdPrefix}{args.Name} add [购物车] [商品ID]\n" +
                    $"{args.CommamdPrefix}{args.Name} del [购物车] [商品ID]\n" +
                    $"{args.CommamdPrefix}{args.Name} clear [购物车]\n" +
                    $"{args.CommamdPrefix}{args.Name} list");
            }
        }
        catch (Exception e)
        {
            await args.Event.Reply(e.Message);
        }
        Config.Save();
    }
}

