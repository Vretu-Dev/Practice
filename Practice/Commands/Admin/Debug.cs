using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using UnityEngine;

namespace Practice.Commands
{
    public class DebugPos : ICommand, IUsageProvider
    {
        public string Command => "debugpos";
        public string[] Aliases => new[] { "dp" };
        public string Description => "Show local coords and rotation in current room (admin only)";
        public string[] Usage => Array.Empty<string>();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!PracticeParent.TryGetPlayer(sender, out var player, out response))
                return false;

            if (!player.CheckPermission("practice.admin"))
            {
                response = "You don't have permission: practice.admin";
                return false;
            }

            var room = player.CurrentRoom;
            if (room == null)
            {
                response = "You are not in any known room.";
                return false;
            }

            // globalcords and rotation
            Vector3 globalPos = player.Position;
            Vector3 globalRot = player.Rotation.eulerAngles;

            // localcords and rotation
            Vector3 localPos = room.Transform.InverseTransformPoint(globalPos);
            Quaternion localRotQuat = Quaternion.Inverse(room.Transform.rotation) * player.Rotation;
            Vector3 localRot = localRotQuat.eulerAngles;

            response =
                $"\n<color=yellow>Debug position:</color>\n" +
                $"RoomType: <b>{room.Type}</b>\n" +
                $"Global Pos: <b>{globalPos.x:F3}, {globalPos.y:F3}, {globalPos.z:F3}</b>\n" +
                $"Global Rot (Y): <b>{globalRot.y:F1}</b>\n" +
                $"Local Pos: <b>{localPos.x:F3}, {localPos.y:F3}, {localPos.z:F3}</b>\n" +
                $"Local Rot (Y): <b>{localRot.y:F1}</b>\n\n";

            return true;
        }
    }
}