using Lagrange.Core;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Internal.Context;
using Lagrange.Core.Internal.Context.Logic.Implementation;
using Lagrange.Core.Utility.Extension;
using Lagrange.XocMat.Plugin;
using Microsoft.Extensions.Logging;
using ProtoBuf.Meta;
using System.Reflection;

namespace Disorder;

public class Disorder(ILogger logger, BotContext bot) : XocMatPlugin(logger, bot)
{
    public override string Name => "Disorder";

    public override string Description => "提供一些乱七八糟的功能";

    public override string Author => "少司命";

    public override Version Version => new(1, 0, 0, 0);

    protected override void Initialize()
    {
        BotContext.Invoker.OnGroupInvitationReceived += Invoker_OnGroupInvitationReceived;
        BotContext.Invoker.OnGroupJoinRequestEvent += Invoker_OnGroupJoinRequestEvent;
    }

    private async void Invoker_OnGroupJoinRequestEvent(BotContext context, Lagrange.Core.Event.EventArg.GroupJoinRequestEvent e)
    {
        if (!Config.Instance.AllowGroupJoinRequest)
            return;
        var info = await context.FetchUserInfo(e.TargetUin);
        if (info == null || Config.Instance.Level <= 0 && info.Level < Config.Instance.Level)
        {
            return;
        }
        var flag = BindingFlags.NonPublic | BindingFlags.Instance;
        if (typeof(BotContext)
            .GetField("ContextCollection", flag)
            ?.GetValue(context) is ContextCollection ContextCollection)
        {
            if (typeof(BusinessContext).GetProperty("OperationLogic", flag)?.GetValue(ContextCollection.Business) is OperationLogic logic)
            {
                var requests = await bot.FetchGroupRequests();
                if (requests?.FirstOrDefault(x => e.GroupUin == x.GroupUin && e.TargetUin == x.TargetMemberUin) is { } request)
                {
                    var sequence = (ulong)request.GetType().GetProperty("Sequence", flag)!.GetValue(request)!;
                    await logic.SetGroupRequest(e.GroupUin, sequence, (uint)request.EventType, GroupRequestOperate.Allow, "");
                }
            }
        }
    }

    private async void Invoker_OnGroupInvitationReceived(BotContext context, Lagrange.Core.Event.EventArg.GroupInvitationEvent e)
    {
        if (!Config.Instance.AllowGroupJoinRequest)
            return;
        var flag = BindingFlags.NonPublic | BindingFlags.Instance;
        if (typeof(BotContext)
            .GetField("ContextCollection", flag)
            ?.GetValue(context) is ContextCollection ContextCollection)
        {
            if (typeof(BusinessContext).GetProperty("OperationLogic", flag)?.GetValue(ContextCollection.Business) is OperationLogic logic)
            {
                var sequence = e.Sequence;
                if (sequence == null)
                { 
                    var requests = await context.FetchGroupRequests();
                    if (requests == null) return;

                    var request = requests.FirstOrDefault(r =>
                    {
                        return r.EventType == BotGroupRequest.Type.SelfInvitation
                            && r.GroupUin ==e.GroupUin
                            && r.InvitorMemberUin == e.InvitorUin;
                    });
                    if (request == null) return;
                    sequence = (ulong)request.GetType().GetProperty("Sequence", flag)!.GetValue(request)!;
                }
                await logic.SetGroupRequest(e.GroupUin, sequence!.Value, 2, GroupRequestOperate.Allow, "");
            }
        }
    }

    protected override void Dispose(bool dispose)
    {
    }
}
