using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.EventArgs.Warhead;
using Interactables.Interobjects.DoorUtils;
using Practice.Helpers;

namespace Practice
{
    public static class EventHandlers
    {
        public static void RegisterEvents()
        {
            Exiled.Events.Handlers.Warhead.Starting += OnWarheadStarting;
            Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;
            Exiled.Events.Handlers.Player.Verified += OnVerified;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Player.Died += OnDied;
        }

        public static void UnregisterEvents()
        {
            OnPluginDisable();
            Exiled.Events.Handlers.Warhead.Starting -= OnWarheadStarting;
            Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawningTeam;
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Player.Died -= OnDied;
        }

        private static void OnPluginDisable()
        {
            Barriers.DeSpawnAll();

            Round.IsLocked = false;
            Map.IsDecontaminationEnabled = true;

            foreach (Lift lift in Lift.List)
                lift.ChangeLock(DoorLockReason.AdminCommand);

            foreach(var duel in DuelManager.Duels.ToList())
                DuelManager.FinishDuel(duel);
        }   
        private static void OnWarheadStarting(StartingEventArgs ev) => ev.IsAllowed = false;
        private static void OnRespawningTeam(RespawningTeamEventArgs ev) => ev.IsAllowed = false;

        private static void OnVerified(VerifiedEventArgs ev)
        {
            if (Round.IsStarted)
                DuelHelper.PlayerHub(ev.Player);
        }

        private static void OnRoundStarted()
        {
            Round.IsLocked = true;
            Map.IsDecontaminationEnabled = false;

            Barriers.SpawnAll();

            foreach (var player in Player.List)
                DuelHelper.PlayerHub(player);

            foreach (Lift lift in Lift.List)
                lift.ChangeLock(DoorLockReason.AdminCommand);

            foreach (Door door in Door.List)
                door.IsOpen = false;
        }

        private static void OnDied(DiedEventArgs ev)
        {
            var duel = DuelManager.Duels.FirstOrDefault(d => d.State == DuelState.InProgress && d.GetPlayers().Contains(ev.Player));

            if (duel == null)
                return;

            if (duel.Mode == DuelMode.OneVsOne)
            {
                var loser = ev.Player;
                var winner = duel.GetPlayers().FirstOrDefault(p => p != loser);

                if (winner == null)
                    return;

                DuelHelper.ShowBroadcastAndFinish(duel, new[] { winner }, new[] { loser });
                return;
            }

            if (duel.Mode == DuelMode.TwoVsTwo)
            {
                bool isInTeamA = duel.TeamA.Contains(ev.Player);
                bool isInTeamB = duel.TeamB.Contains(ev.Player);

                var aliveTeamA = duel.TeamA.Where(p => p != null && p.IsConnected && p.IsAlive).ToList();
                var aliveTeamB = duel.TeamB.Where(p => p != null && p.IsConnected && p.IsAlive).ToList();

                if (isInTeamA && aliveTeamA.Count == 0)
                {
                    DuelHelper.ShowBroadcastAndFinish(duel, aliveTeamB, duel.TeamA);
                    return;
                }

                if (isInTeamB && aliveTeamB.Count == 0)
                {
                    DuelHelper.ShowBroadcastAndFinish(duel, aliveTeamA, duel.TeamB);
                    return;
                }
            }
        }
    }
}