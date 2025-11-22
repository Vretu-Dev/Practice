using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RemoteAdmin;
using System;
using static Broadcast;

namespace Practice.Commands
{
    public class CancelFor : ICommand, IUsageProvider
    {
        public string Command => "cancelfor";
        public string[] Aliases => new[] { "cf" };
        public string Description => "Cancel duel for another player (admin only)";
        public string[] Usage => new[] { "[OwnerNickname]" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender plSender)
            {
                var admin = Player.Get(plSender);
                if (admin == null)
                {
                    response = "Command only for player.";
                    return false;
                }

                if (!admin.CheckPermission("practice.admin"))
                {
                    response = "You don't have permission: practice.admin";
                    return false;
                }
            }

            if (arguments.Count < 1)
            {
                response = "Using: pc cancelfor [OwnerNickname]";
                return false;
            }

            var ownerNameArg = arguments.At(0);

            var ownerPlayer = Player.Get(ownerNameArg);

            if (ownerPlayer == null)
            {
                response = $"Player '{ownerNameArg}' not found.";
                return false;
            }

            if (!DuelManager.CancelDuel(ownerPlayer.Nickname, ownerPlayer, out var msg))
            {
                response = msg;
                return false;
            }

            response = $"Duel of player '{ownerPlayer.Nickname}' has been canceled. ({msg})";

            ownerPlayer.Broadcast(5, "<color=yellow>Admin canceled your duel.</color>", BroadcastFlags.Normal, true);

            return true;
        }
    }
}