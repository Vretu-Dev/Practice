using Exiled.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEC;

namespace Practice.Helpers
{
    public static class Overlay
    {
        private static CoroutineHandle overlay;

        public static void EnableOverlay()
        {
            overlay = Timing.RunCoroutine(PlayerOverlay());
        }

        public static void DisableOverlay()
        {
            if (overlay.IsRunning)
                Timing.KillCoroutines(overlay);
        }

        private static IEnumerator<float> PlayerOverlay()
        {
            while (true)
            {
                var now = DateTime.UtcNow;

                // Hints for players on arena – countdown
                foreach (var duel in DuelManager.Duels.ToList())
                {
                    if (duel.State == DuelState.CountingDown && duel.CountdownStart.HasValue)
                    {
                        var elapsed = (now - duel.CountdownStart.Value).TotalSeconds;
                        var remaining = duel.CountdownDuration - elapsed;

                        foreach (var p in duel.GetPlayers())
                        {
                            if (p != null && p.IsConnected)
                                p.ShowHint($"<color=yellow>The fight will begin in {Math.Max(0, (int)remaining)}s</color>", 1.15f);
                        }

                        if (elapsed >= duel.CountdownDuration)
                        {
                            duel.State = DuelState.InProgress;
                            duel.CountdownStart = null;

                            foreach (var p in duel.GetPlayers())
                            {
                                if (p != null && p.IsConnected)
                                    p.ShowHint("<color=black>S</color>T<color=black>A</color>R<color=black>T</color>!", 2f);
                            }
                        }
                    }
                }

                // Hints for players in hub – list of duels
                foreach (var player in Player.List)
                {
                    if (player.Role.Type != RoleTypeId.Tutorial)
                        continue;

                    var sb = new StringBuilder();
                    sb.AppendLine("<size=40> <color=#70EE9C><b>SCP:SL Practice</b></color> \n" +
                                  "<color=yellow>by <b>Vretu</b></color> \n \n" +
                                  " Type <b>.practice</b> in console for more info </size> \n \n" +
                                  " <b>Duels:</b>");

                    foreach (var d in DuelManager.Duels.Where(d => d.State != DuelState.Finished))
                    {
                        string color = d.State switch
                        {
                            DuelState.Waiting => "green",
                            DuelState.CountingDown => "yellow",
                            DuelState.InProgress => "red",
                            _ => "white"
                        };

                        sb.AppendLine(
                            $"<color={color}>{d.Name}</color> - {d.Room} - {d.DuelType} - {d.State} " +
                            $"({d.CurrentPlayersCount}/{d.RequiredPlayers})");
                    }

                    player.ShowHint(sb.ToString(), 1.15f);
                }

                yield return Timing.WaitForSeconds(1f);
            }
        }
    }
}
