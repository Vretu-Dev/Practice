using Exiled.API.Enums;
using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Practice
{
    /// <summary>
    /// LocalPoint (spawn / barier) in specify RoomType.
    /// </summary>
    public class ArenaPoint(RoomType roomType, Vector3 localPosition, Quaternion localRotation)
    {
        public RoomType RoomType { get; } = roomType;
        public Vector3 LocalPosition { get; } = localPosition;
        public Quaternion LocalRotation { get; } = localRotation;

        public Vector3 GetWorldPosition()
        {
            var room = Room.List.FirstOrDefault(r => r.Type == RoomType);

            if (room == null)
                return LocalPosition;

            return room.Transform.TransformPoint(LocalPosition);
        }

        public Quaternion GetWorldRotation()
        {
            var room = Room.List.FirstOrDefault(r => r.Type == RoomType);

            if (room == null)
                return LocalRotation;

            return room.Transform.rotation * LocalRotation;
        }
    }

    public class ArenaDefinition(ArenaPoint p1, ArenaPoint p2, List<ArenaPoint> barriers = null)
    {
        public ArenaPoint P1 { get; } = p1;
        public ArenaPoint P2 { get; } = p2;
        public List<ArenaPoint> Barriers { get; } = barriers ?? new List<ArenaPoint>();
    }

    /// <summary>
    /// Storage of all Arenas | DuelRoom defined RoomType, positions, rotations and barriers.
    /// </summary>
    public static class Arenas
    {
        public static IEnumerable<ArenaDefinition> AllArenas => _arenas.Values;

        public static bool TryGetArena(DuelRoom room, out ArenaDefinition arena) => _arenas.TryGetValue(room, out arena);
        
        private static readonly Dictionary<DuelRoom, ArenaDefinition> _arenas = new()
        {
            {
                DuelRoom.HczCheckpointA,
                new ArenaDefinition(
                    new ArenaPoint(
                        roomType: RoomType.HczEzCheckpointA,
                        localPosition: new Vector3(-6.125f, 1f, 0f),
                        localRotation: Quaternion.Euler(0f, 90f, 0f)
                    ),
                    new ArenaPoint(
                        roomType: RoomType.EzCheckpointHallwayA,
                        localPosition: new Vector3(0f, 1f, -6.4f),
                        localRotation: Quaternion.Euler(0f, 0f, 0f)
                    ),
                    new List<ArenaPoint>
                    {
                        new ArenaPoint(
                            roomType: RoomType.HczEzCheckpointA,
                            localPosition: new Vector3(-7.5f, 1.5f, 0f),
                            localRotation: Quaternion.Euler(0f, 0f, 90f)
                        ),
                        new ArenaPoint(
                            roomType: RoomType.EzCheckpointHallwayA,
                            localPosition: new Vector3(0f, 1.5f, -7.5f),
                            localRotation: Quaternion.Euler(0f, 90f, 90f)
                        )
                    }
                )
            },
            {
                DuelRoom.HczCheckpointB,
                new ArenaDefinition(
                    new ArenaPoint(
                        roomType: RoomType.HczEzCheckpointB,
                        localPosition: new Vector3(-6.125f, 1f, 0f),
                        localRotation: Quaternion.Euler(0f, 90f, 0f)
                    ),
                    new ArenaPoint(
                        roomType: RoomType.EzCheckpointHallwayB,
                        localPosition: new Vector3(0f, 1f, -6.4f),
                        localRotation: Quaternion.Euler(0f, 0f, 0f)
                    ),
                    new List<ArenaPoint>
                    {
                        new ArenaPoint(
                            roomType: RoomType.HczEzCheckpointB,
                            localPosition: new Vector3(-7.5f, 1.5f, 0f),
                            localRotation: Quaternion.Euler(0f, 0f, 90f)
                        ),
                        new ArenaPoint(
                            roomType: RoomType.EzCheckpointHallwayB,
                            localPosition: new Vector3(0f, 1.5f, -7.5f),
                            localRotation: Quaternion.Euler(0f, 90f, 90f)
                        )
                    }
                )
            },
            {
                DuelRoom.LczCheckpointA,
                new ArenaDefinition(
                    new ArenaPoint(
                        roomType: RoomType.LczCheckpointA,
                        localPosition: new Vector3(12f, 1f, 0f),
                        localRotation: Quaternion.Euler(0f, 270f, 0f)
                    ),
                    new ArenaPoint(
                        roomType: RoomType.LczCheckpointA,
                        localPosition: new Vector3(-6.4f, 1f, 0f),
                        localRotation: Quaternion.Euler(0f, 90f, 0f)
                    ),
                    new List<ArenaPoint>
                    {
                        new ArenaPoint(
                            roomType: RoomType.LczCheckpointA,
                            localPosition: new Vector3(-7.5f, 1.5f, 0f),
                            localRotation: Quaternion.Euler(0f, 0f, 90f)
                        )
                    }
                )
            },
            {
                DuelRoom.LczCheckpointB,
                new ArenaDefinition(
                    new ArenaPoint(
                        roomType: RoomType.LczCheckpointB,
                        localPosition: new Vector3(12f, 1f, 0f),
                        localRotation: Quaternion.Euler(0f, 270f, 0f)
                    ),
                    new ArenaPoint(
                        roomType: RoomType.LczCheckpointB,
                        localPosition: new Vector3(-6.4f, 1f, 0f),
                        localRotation: Quaternion.Euler(0f, 90f, 0f)
                    ),
                    new List<ArenaPoint>
                    {
                        new ArenaPoint(
                            roomType: RoomType.LczCheckpointB,
                            localPosition: new Vector3(-7.5f, 1.5f, 0f),
                            localRotation: Quaternion.Euler(0f, 0f, 90f)
                        )
                    }
                )
            },
            {
                DuelRoom.GateA,
                new ArenaDefinition(
                    new ArenaPoint(
                        roomType: RoomType.EzGateA,
                        localPosition: new Vector3(0f, 1f, 7f),
                        localRotation: Quaternion.Euler(0f, 180f, 0f)
                    ),
                    new ArenaPoint(
                        roomType: RoomType.EzGateA,
                        localPosition: new Vector3(0f, 1f, -9.15f),
                        localRotation: Quaternion.Euler(0f, 360f, 0f)
                    ),
                    new List<ArenaPoint>
                    {
                        new ArenaPoint(
                            roomType: RoomType.EzGateA,
                            localPosition: new Vector3(0f, 1.5f, 7.5f),
                            localRotation: Quaternion.Euler(0f, 90f, 90f)
                        )
                    }
                )
            },
            {
                DuelRoom.GateB,
                new ArenaDefinition(
                    new ArenaPoint(
                        roomType: RoomType.EzGateB,
                        localPosition: new Vector3(-4.7f, 1f, -12.6f),
                        localRotation: Quaternion.Euler(0f, 360f, 0f)
                    ),
                    new ArenaPoint(
                        roomType: RoomType.EzGateB,
                        localPosition: new Vector3(0.5f, 1f, 6f),
                        localRotation: Quaternion.Euler(0f, 270f, 0f)
                    ),
                    new List<ArenaPoint>
                    {
                        new ArenaPoint(
                            roomType: RoomType.EzGateB,
                            localPosition: new Vector3(0f, 1.5f, 7.36f),
                            localRotation: Quaternion.Euler(0f, 90f, 90f)
                        )
                    }
                )
            },
            {
                DuelRoom.Glassroom,
                new ArenaDefinition(
                    new ArenaPoint(
                        roomType: RoomType.LczGlassBox,
                        localPosition: new Vector3(-6.7f, 1f, 0f),
                        localRotation: Quaternion.Euler(0f, 90f, 0f)
                    ),
                    new ArenaPoint(
                        roomType: RoomType.LczGlassBox,
                        localPosition: new Vector3(8.8f, 1.5f, 0f),
                        localRotation: Quaternion.Euler(0f, 270f, 0f)
                    ),
                    new List<ArenaPoint>
                    {
                        new ArenaPoint(
                            roomType: RoomType.LczGlassBox,
                            localPosition: new Vector3(-7.5f, 1.5f, 0f),
                            localRotation: Quaternion.Euler(0f, 0f, 90f)
                        )
                    }
                )
            },
            {
                DuelRoom.Chamber049,
                new ArenaDefinition(
                    new ArenaPoint(
                        roomType: RoomType.Hcz049,
                        localPosition: new Vector3(-20f, 90f, 10f),
                        localRotation: Quaternion.Euler(0f, 90f, 0f)
                    ),
                    new ArenaPoint(
                        roomType: RoomType.Hcz049,
                        localPosition: new Vector3(26.8f, 95f, 10.4f),
                        localRotation: Quaternion.Euler(0f, 270f, 0f)
                    )
                )
            },
        };
    }
}