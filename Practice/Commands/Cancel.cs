using CommandSystem;
using Exiled.API.Features;
using System;

namespace Practice.Commands
{
    public class Cancel : ICommand, IUsageProvider
    {
        public string Command => "cancel";
        public string[] Aliases => new[] { "c" };
        public string Description => "Cancel your duel";
        public string[] Usage => Array.Empty<string>();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!PracticeParent.TryGetPlayer(sender, out Player player, out response))
                return false;

            // Cancel for player who typed the command
            if (!DuelManager.CancelDuel(player.Nickname, player, out var msg))
            {
                response = msg;
                return false;
            }

            response = msg;
            return true;
        }
    }
}