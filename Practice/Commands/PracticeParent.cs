using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RemoteAdmin;
using System;

namespace Practice.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class PracticeParent : ParentCommand, IUsageProvider
    {
        public PracticeParent() => LoadGeneratedCommands();
        public override string Command => "practice";
        public override string[] Aliases => new string[] { "pc" };
        public override string Description => "Practice";
        public string[] Usage => new[] { "create / join / list / rooms / cancel" };

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Create());
            RegisterCommand(new Join());
            RegisterCommand(new List());
            RegisterCommand(new Rooms());
            RegisterCommand(new Cancel());
            RegisterCommand(new CreateFor());
            RegisterCommand(new CancelFor());
            RegisterCommand(new DebugPos());
            RegisterCommand(new JoinFor());
            RegisterCommand(new Leave());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response =
                "Commands: \n" +
                "<color=yellow>pc create [Room] [Type] [Mode]</color> - Create a duel\n" +
                "<color=yellow>pc join [OwnerNickname]</color> - Join to existing duel\n" +
                "<color=yellow>pc cancel</color> - Cancel your duel\n" +
                 "<color=yellow>pc leave</color> - Leave a duel\n" +
                "<color=yellow>pc list</color> - List of creating duels\n" +
                "<color=yellow>pc rooms</color> - List available rooms to duel\n";
                
            return false;
        }

        public static bool TryGetPlayer(ICommandSender sender, out Player player, out string response)
        {
            player = null;
            response = null;

            if (sender is not PlayerCommandSender plSender || !Player.TryGet(plSender, out player))
            {
                response = "Command only for player!";
                return false;
            }

            if (!player.CheckPermission("practice.all"))
            {
                response = "You don't have permision: practice.all!";
                player = null;
                return false;
            }

            return true;
        }
    }
}