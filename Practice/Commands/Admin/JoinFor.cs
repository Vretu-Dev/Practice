using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RemoteAdmin;
using System;
using Practice.Helpers;
using PlayerRoles;
using static Broadcast;

namespace Practice.Commands
{
    public class JoinFor : ICommand, IUsageProvider
    {
        public string Command => "joinfor";
        public string[] Aliases => new[] { "jf" };
        public string Description => "Force a player to join an existing duel (admin only)";
        public string[] Usage => new[] { "[TargetPlayer] [OwnerNickname]" };

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
                response = "Using: pc joinfor [TargetPlayer] [OwnerNickname]";
                return false;
            }

            var targetArg = arguments.At(0);
            var ownerArg = arguments.At(1);

            var target = Player.Get(targetArg);

            if (target == null)
            {
                response = $"Player '{targetArg}' not found.";
                return false;
            }

            if (target.Role.Type != RoleTypeId.Tutorial)
            {
                response = $"Player '{target.Nickname}' must have Tutorial role to join a duel.";
                return false;
            }

            var duel = DuelHelper.GetDuelByOwnerName(ownerArg);

            if (duel == null)
            {
                response = $"The duel of player '{ownerArg}' was not found!";
                return false;
            }

            if (duel.State != DuelState.Waiting)
            {
                response = "This duel does not wait for players.";
                return false;
            }

            if (duel.ContainsPlayer(target))
            {
                response = $"Player '{target.Nickname}' is already in this duel.";
                return false;
            }

            if (duel.IsFull)
            {
                response = "This duel already has all required players.";
                return false;
            }

            duel.AddPlayer(target);

            if (duel.Mode == DuelMode.OneVsOne)
            {
                duel.TeamB.Add(target);
            }

            if (duel.Mode == DuelMode.TwoVsTwo)
            {
                if (duel.TeamA.Count < 2)
                    duel.TeamA.Add(target);
                else
                    duel.TeamB.Add(target);
            }

            if (duel.IsFull)
            {
                DuelHelper.StartCountdown(duel);
                DuelManager.StartDuel(duel);

                response =
                    $"Player '{target.Nickname}' joined duel of player '{duel.Owner.Nickname}'. Players: {duel.CurrentPlayersCount}/{duel.RequiredPlayers}. " +
                    $"Countdown started!";
            }
            else
            {
                response =
                    $"Player '{target.Nickname}' joined duel of player '{duel.Owner.Nickname}'. Players: {duel.CurrentPlayersCount}/{duel.RequiredPlayers}. " +
                    $"Waiting for more players.";
            }

            duel.Owner.Broadcast(5, $"<color=yellow>Player {target.Nickname} was added to your duel ({duel.CurrentPlayersCount}/{duel.RequiredPlayers}).</color>", BroadcastFlags.Normal, true);
            target.Broadcast(5, $"<color=yellow>You were added to the duel of player {duel.Owner.Nickname}.</color>", BroadcastFlags.Normal, true);

            return true;
        }
    }
}