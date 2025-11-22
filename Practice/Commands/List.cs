using CommandSystem;
using System;
using System.Text;

namespace Practice.Commands
{
    public class List : ICommand
    {
        public string Command => "list";
        public string[] Aliases => new[] { "ls" };
        public string Description => "List of created duels";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!PracticeParent.TryGetPlayer(sender, out _, out response))
                return false;

            var sb = new StringBuilder("\nDuels List:\n");

            foreach (var duel in DuelManager.Duels)
            {
                if (duel.State == DuelState.Finished)
                    continue;

                string color = duel.State switch
                {
                    DuelState.Waiting => "green",
                    DuelState.CountingDown => "yellow",
                    DuelState.InProgress => "red",
                    _ => "white"
                };

                sb.AppendLine($"<color={color}>{duel.Owner?.Nickname}</color> - Room: {duel.Room} | Type: {duel.DuelType} | State: {duel.State}");
            }

            response = sb.ToString();
            return true;
        }
    }
}