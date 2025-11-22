using System;
using System.Linq;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Practice.Helpers;
using MEC;

namespace Practice
{
    public static class DuelManager
    {
        public static List<Duels> Duels { get; } = new();

        public static Duels CreateDuel(DuelRoom room, DuelType type, DuelMode mode, Player owner, out string error)
        {
            error = null;

            if (Duels.Any(d => d.Owner == owner && d.State != DuelState.Finished))
            {
                error = "You already have a duel created. First, cancel or end it.";
                return null;
            }

            if (!DuelHelper.IsRoomFree(room))
            {
                error = "This room is currently occupied!";
                return null;
            }

            var duel = new Duels
            {
                Name = owner.Nickname,
                Room = room,
                DuelType = type,
                Mode = mode,
                Owner = owner,
                State = DuelState.Waiting,
                CreatedAt = DateTime.UtcNow,
                IsRoomLocked = false
            };

            duel.AddPlayer(owner);
            duel.TeamA.Add(owner);

            Duels.Add(duel);

            return duel;
        }

        public static bool CancelDuel(string ownerName, Player owner, out string message)
        {
            var duel = DuelHelper.GetDuelByOwnerName(ownerName);

            if (duel == null)
            {
                message = $"Duel player '{ownerName}' not exists.";
                return false;
            }

            if (duel.Owner != owner)
            {
                message = "You are not owner of this duel.";
                return false;
            }

            if (duel.State == DuelState.Finished)
            {
                message = "This duel has been ended.";
                return false;
            }

            duel.State = DuelState.Finished;
            duel.IsRoomLocked = false;

            foreach (var player in duel.GetPlayers())
            {
                if (player != null && player.IsConnected)
                    DuelHelper.PlayerHub(player);
            }

            message = $"Your duel was canceled.";
            return true;
        }

        public static bool LeaveDuel(Player player, out string message)
        {
            var duel = DuelHelper.GetDuelByPlayer(player);

            if (duel == null)
            {
                message = "You are not in any duel.";
                return false;
            }

            if (duel.Owner == player)
            {
                message = "You are the owner of this duel. Use .pc cancel.";
                return false;
            }

            if (duel.State == DuelState.Finished)
            {
                message = "This duel has already finished.";
                return false;
            }

            duel.Players.Remove(player);
            duel.TeamA.Remove(player);
            duel.TeamB.Remove(player);

            DuelHelper.PlayerHub(player);

            message = "You left the duel.";
            return true;
        }

        public static void StartDuel(Duels duel)
        {
            if (!Arenas.TryGetArena(duel.Room, out var arena))
                return;

            var players = duel.GetPlayers();
            if (players.Count < 2)
                return;

            DuelHelper.TeleportPlayers(duel, arena.P1, arena.P2);

            foreach (var p in players)
            {
                DuelHelper.GiveLoadout(p, duel.DuelType);
                p.EnableEffect(EffectType.Ensnared, duel.CountdownDuration + 2);
            }

            duel.CountdownStart = DateTime.UtcNow;
            duel.State = DuelState.CountingDown;
        }

        public static void FinishDuel(Duels duel)
        {
            duel.State = DuelState.Finished;
            duel.IsRoomLocked = false;

            Timing.CallDelayed(3f, () =>
            {
                DuelHelper.CloseArenaDoors(duel.Room);

                foreach (var player in duel.GetPlayers())
                {
                    if (player != null && player.IsConnected)
                        DuelHelper.PlayerHub(player);
                }

                Map.CleanAllItems();
                Map.CleanAllRagdolls();
            });
        }
    }
}