using System;
using CommandSystem;
using Practice.Helpers;
using PlayerRoles;
using static Broadcast;

namespace Practice.Commands
{
    public class Join : ICommand, IUsageProvider
    {
        public string Command => "join";
        public string[] Aliases => new[] { "j" };
        public string Description => "Join to existing duel";
        public string[] Usage => new[] { "[OwnerNickname]" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!PracticeParent.TryGetPlayer(sender, out var player, out response))
                return false;

            if (player.Role.Type != RoleTypeId.Tutorial)
            {
                response = "You must have the Tutorial role to join a duel.";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Using: pc join [OwnerNickname]";
                return false;
            }

            var ownerName = arguments.At(0);
            var duel = DuelHelper.GetDuelByOwnerName(ownerName);

            if (duel == null)
            {
                response = $"The duel of player ‘{ownerName}’ was not found!";
                return false;
            }

            if (duel.State != DuelState.Waiting)
            {
                response = "This duel does not wait for players.";
                return false;
            }

            if (duel.Owner == player || duel.ContainsPlayer(player))
            {
                response = "You are already in this duel.";
                return false;
            }

            if (duel.IsFull)
            {
                response = "This duel already has all required players.";
                return false;
            }

            duel.AddPlayer(player);

            if (duel.Mode == DuelMode.OneVsOne)
            {
                duel.TeamB.Add(player);
            }

            if (duel.Mode == DuelMode.TwoVsTwo)
            {
                if (duel.TeamA.Count < 2)
                    duel.TeamA.Add(player);
                else
                    duel.TeamB.Add(player);
            }

            if (duel.IsFull)
            {
                DuelHelper.StartCountdown(duel);
                DuelManager.StartDuel(duel);

                response =
                    $"You joined duel of player '{duel.Owner.Nickname}'. Players: {duel.CurrentPlayersCount}/{duel.RequiredPlayers}. " +
                    $"Countdown started!";
            }
            else
            {
                response =
                    $"You joined duel of player '{duel.Owner.Nickname}'. Players: {duel.CurrentPlayersCount}/{duel.RequiredPlayers}. " +
                    $"Waiting for more players.";
            }

            duel.Owner.Broadcast(5, $"<color=yellow>Player {player.Nickname} joined your duel ({duel.CurrentPlayersCount}/{duel.RequiredPlayers}).</color>", BroadcastFlags.Normal, true);

            return true;
        }
    }
}