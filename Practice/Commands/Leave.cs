using CommandSystem;
using Exiled.API.Features;
using PlayerRoles;
using System;

namespace Practice.Commands
{
    public class Leave : ICommand, IUsageProvider
    {
        public string Command => "leave";
        public string[] Aliases => new[] { "lv" };
        public string Description => "Leave your current duel";
        public string[] Usage => Array.Empty<string>();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!PracticeParent.TryGetPlayer(sender, out Player player, out response))
                return false;

            if (player.Role.Type != RoleTypeId.Tutorial)
            {
                response = "You must have the Tutorial role to use this command.";
                return false;
            }

            if (!DuelManager.LeaveDuel(player, out var msg))
            {
                response = msg;
                return false;
            }

            response = msg;
            return true;
        }
    }
}