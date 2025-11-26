using Exiled.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Practice
{
    public class Duels
    {
        public string Name;

        public Player Owner;

        public DuelRoom Room;
        public DuelType DuelType;
        public DuelMode Mode;
        public DuelState State;

        public bool IsRoomLocked;

        public DateTime CreatedAt;
        public DateTime? CountdownStart;

        public int CountdownDuration = 10;

        public List<Player> GetPlayers() => Players.Where(p => p != null && p.IsConnected).ToList();
        public List<Player> Players { get; } = new();
        public List<Player> TeamA { get; } = new();
        public List<Player> TeamB { get; } = new();

        public ClassicLoadout SelectedClassicLoadout;

        public RoleTypeId TeamARole;
        public RoleTypeId TeamBRole;
        
        public bool ContainsPlayer(Player player) => Players.Contains(player);
        public bool IsFull => CurrentPlayersCount >= RequiredPlayers;
        
        public int RequiredPlayers => Mode == DuelMode.TwoVsTwo ? 4 : 2;
        public int CurrentPlayersCount => Players.Count(p => p != null && p.IsConnected);

        public void AddPlayer(Player player)
        {
            if (player != null && !Players.Contains(player))
                Players.Add(player);
        }
    }
}