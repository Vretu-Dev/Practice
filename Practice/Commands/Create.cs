using CommandSystem;
using System;
using PlayerRoles;

namespace Practice.Commands
{
    public class Create : ICommand, IUsageProvider
    {
        public string Command => "create";
        public string[] Aliases => new[] { "cr" };
        public string Description => "Create a new duel";
        public string[] Usage => new[] { "[Room] [Type] [Mode]" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!PracticeParent.TryGetPlayer(sender, out var player, out response))
                return false;

            if (player.Role.Type != RoleTypeId.Tutorial)
            {
                response = "You must have the Tutorial role to create duels.";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Using: pc create [Room] (Type optional: Classic/Jailbird) (Mode optional: 1v1/2v2)";
                return false;
            }

            var roomArg = arguments.At(0);
            var typeArg = arguments.Count >= 2 ? arguments.At(1) : "Classic";
            var modeArg = arguments.Count >= 3 ? arguments.At(2) : "1v1";

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

            var duel = DuelManager.CreateDuel(room, type, mode, player, out var error);

            if (duel == null)
            {
                response = error;
                return false;
            }

            response =
                $"Duel for player '{player.Nickname}' was created. Room: {duel.Room}, Type: {duel.DuelType}, Mode: {duel.Mode}. " +
                $"Waiting for players 1/{duel.RequiredPlayers}.";

            return true;
        }
    }
}