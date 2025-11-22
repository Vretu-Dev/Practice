using CommandSystem;
using Practice.Helpers;
using System;
using System.Text;

namespace Practice.Commands
{
    public class Rooms : ICommand
    {
        public string Command => "rooms";
        public string[] Aliases => new[] { "rm" };
        public string Description => "List available rooms to duel";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!PracticeParent.TryGetPlayer(sender, out _, out response))
                return false;

            var sb = new StringBuilder("\nRooms List:\n");

            foreach (DuelRoom room in Enum.GetValues(typeof(DuelRoom)))
            {
                bool free = DuelHelper.IsRoomFree(room);
                string color = free ? "green" : "red";
                sb.AppendLine($"<color={color}>{room}</color> - {(free ? "Free" : "Occupied")}");
            }

            response = sb.ToString();
            return true;
        }
    }
}