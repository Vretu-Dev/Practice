using Exiled.API.Features;
using Exiled.API.Features.Doors;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Broadcast;
using Random = UnityEngine.Random;

namespace Practice.Helpers
{
    public static class DuelHelper
    {
        public static void PlayerHub(Player player) => player.Role.Set(RoleTypeId.Tutorial, RoleSpawnFlags.All);

        public static bool IsRoomFree(DuelRoom room) => DuelManager.Duels.All(d => d.Room != room || d.State == DuelState.Finished);

        public static Duels GetDuelByOwnerName(string ownerName) => DuelManager.Duels.FirstOrDefault(d => d.Owner != null && d.Owner.Nickname.Equals(ownerName, StringComparison.OrdinalIgnoreCase) && d.State != DuelState.Finished);
        
        public static Duels GetDuelByPlayer(Player player) => DuelManager.Duels.FirstOrDefault(d => d.State != DuelState.Finished && d.GetPlayers().Contains(player));

        public static void ShowBroadcastAndFinish(Duels duel, IEnumerable<Player> winners, IEnumerable<Player> losers)
        {
            foreach (var winner in winners.Where(p => p != null && p.IsConnected))
                winner.Broadcast(5, "<color=green>You won the duel!</color>", BroadcastFlags.Normal, true);

            foreach (var loser in losers.Where(p => p != null && p.IsConnected))
                loser.Broadcast(5, "<color=red>You lost the duel!</color>", BroadcastFlags.Normal, true);

            DuelManager.FinishDuel(duel);
        }

        public static void StartCountdown(Duels duel)
        {
            duel.State = DuelState.CountingDown;
            duel.IsRoomLocked = true;
        }

        public static void TeleportPlayers(Duels duel, ArenaPoint ap1, ArenaPoint ap2)
        {
            var players = duel.GetPlayers();

            if (players.Count < 2)
                return;

            SetPlayerRoles(duel);

            SetPlayerPositions(duel, ap1, ap2);
        }

        public static void SetPlayerRoles(Duels duel)
        {
            bool teamAIsScientists = Random.Range(0, 2) == 0;

            duel.TeamARole = teamAIsScientists ? RoleTypeId.Scientist : RoleTypeId.ClassD;
            duel.TeamBRole = teamAIsScientists ? RoleTypeId.ClassD : RoleTypeId.Scientist;
        }

        public static void SetPlayerPositions(Duels duel, ArenaPoint ap1, ArenaPoint ap2)
        {
            var teamA = duel.TeamA.Where(p => p != null && p.IsConnected).ToArray();
            var teamB = duel.TeamB.Where(p => p != null && p.IsConnected).ToArray();

            if (teamA.Length == 0 || teamB.Length == 0)
                return;

            bool teamAOnP1 = Random.Range(0, 2) == 0;
            var sideTeamA = teamAOnP1 ? ap1 : ap2;
            var sideTeamB = teamAOnP1 ? ap2 : ap1;

            if (duel.Mode == DuelMode.OneVsOne)
            {
                var a = teamA[0];
                var b = teamB[0];

                PlaceSinglePlayerOnPoint(a, sideTeamA, duel.TeamARole);
                PlaceSinglePlayerOnPoint(b, sideTeamB, duel.TeamBRole);
            }

            if (duel.Mode == DuelMode.TwoVsTwo)
            {
                PlaceTeamOnSide(teamA, sideTeamA, duel.TeamARole);
                PlaceTeamOnSide(teamB, sideTeamB, duel.TeamBRole);
            }
        }

        private static void PlaceSinglePlayerOnPoint(Player player, ArenaPoint point, RoleTypeId role)
        {
            var pos = point.GetWorldPosition();
            var rot = point.GetWorldRotation();

            player.Position = pos;
            player.Rotation = rot;
            player.Role.Set(role, RoleSpawnFlags.None);
        }

        private static void PlaceTeamOnSide(Player[] team, ArenaPoint sidePoint, RoleTypeId role)
        {
            if (team.Length == 0)
                return;

            var basePos = sidePoint.GetWorldPosition();
            var baseRot = sidePoint.GetWorldRotation();

            var right = baseRot * Vector3.right;
            const float offset = 2f;

            var posCenter = basePos;
            var posLeft = basePos - right * (offset / 2f);
            var posRight = basePos + right * (offset / 2f);

            if (team.Length == 1)
            {
                var p = team[0];
                p.Position = posCenter;
                p.Rotation = baseRot;
                p.Role.Set(role, RoleSpawnFlags.None);
            }
            else
            {
                var p1 = team[0];
                var p2 = team[1];

                p1.Position = posLeft;
                p1.Rotation = baseRot;
                p1.Role.Set(role, RoleSpawnFlags.None);

                p2.Position = posRight;
                p2.Rotation = baseRot;
                p2.Role.Set(role, RoleSpawnFlags.None);
            }
        }

        public static void GiveLoadout(Player player, DuelType type)
        {
            player.ClearInventory();

            switch (type)
            {
                case DuelType.Classic:
                    player.AddItem(ItemType.GunCrossvec);
                    player.AddItem(ItemType.ArmorCombat);
                    player.AddItem(ItemType.Medkit);
                    player.AddItem(ItemType.KeycardO5);
                    player.AddItem(ItemType.Ammo9x19, 10);
                    break;

                case DuelType.Jailbird:
                    player.AddItem(ItemType.Jailbird);
                    player.AddItem(ItemType.KeycardO5);
                    break;

                case DuelType.ParticleDisruptor:
                    player.AddItem(ItemType.ParticleDisruptor);
                    player.AddItem(ItemType.KeycardO5);
                    break;
            }
        }

        public static void CloseArenaDoors(DuelRoom duelRoom)
        {
            if (!Arenas.TryGetArena(duelRoom, out var arena))
                return;

            var roomType = arena.P1.RoomType;

            var rooms = Room.List.Where(r => r.Type == roomType);

            foreach (var room in rooms)
            {
                foreach (var door in Door.List)
                {
                    if (door.Rooms.Contains(room))
                    {
                        door.IsOpen = false;
                    }
                }
            }
        }
    }
}