using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RemoteAdmin;
using System;

namespace Practice.Commands
{
    public class CreateFor : ICommand, IUsageProvider
    {
        public string Command => "createfor";
        public string[] Aliases => new[] { "crf" };
        public string Description => "Create a duel for another player (admin only)";
        public string[] Usage => new[] { "[Player] [Room] [Type] [Mode]" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender plSender)
            {
                var admin = Player.Get(plSender);
                if (admin == null)
                {
                    response = "Command only for player/RA.";
                    return false;
                }

                if (!admin.CheckPermission("practice.admin"))
                {
                    response = "You don't have permission: practice.admin";
                    return false;
                }
            }

            if (arguments.Count < 2)
            {
                response = "Using: pc createfor [Player] [Room] (Type optional: Classic/Jailbird) (Mode optional: 1v1/2v2)";
                return false;
            }

            var targetArg = arguments.At(0);
            var roomArg = arguments.At(1);
            var typeArg = arguments.Count >= 3 ? arguments.At(2) : "Classic";
            var modeArg = arguments.Count >= 4 ? arguments.At(3) : "1v1";

            var target = Player.Get(targetArg);
            if (target == null)
            {
                response = $"Player '{targetArg}' not found.";
                return false;
            }

            if (!Enum.TryParse<DuelRoom>(roomArg, true, out var room))
            {
                response = $"Unknown room '{roomArg}'. Available: {string.Join(", ", Enum.GetNames(typeof(DuelRoom)))}";
                return false;
            }

            if (!Enum.TryParse<DuelType>(typeArg, true, out var type))
            {
                response = $"Unknown duel type '{typeArg}'. Available: {string.Join(", ", Enum.GetNames(typeof(DuelType)))}";
                return false;
            }

            DuelMode mode = DuelMode.OneVsOne;

            if (modeArg.Equals("2v2", StringComparison.OrdinalIgnoreCase))
                mode = DuelMode.TwoVsTwo;

            var duel = DuelManager.CreateDuel(room, type, mode, target, out var error);

            if (duel == null)
            {
                response = error;
                return false;
            }

            response =
                $"Duel for player '{target.Nickname}' was created. Room: {duel.Room}, Type: {duel.DuelType}, Mode: {duel.Mode}. " +
                $"Waiting for players 1/{duel.RequiredPlayers}.";

            target.Broadcast(7, $"<color=yellow>The admin has created a duel for you in the arena {duel.Room} ({duel.DuelType}, {duel.Mode}).</color>");

            return true;
        }
    }
}