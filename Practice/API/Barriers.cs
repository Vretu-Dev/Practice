using AdminToys;
using Exiled.API.Features.Toys;
using System.Collections.Generic;
using UnityEngine;

namespace Practice
{
    public static class Barriers
    {
        private static readonly List<Primitive> Spawned = new();

        public static void SpawnAll()
        {
            ClearAll();

            foreach (var arena in Arenas.AllArenas)
            {
                foreach (var barrier in arena.Barriers)
                    SpawnBarrier(barrier);
            }
        }

        public static void ClearAll()
        {
            foreach (var prim in Spawned)
            {
                if (prim.GameObject == null)
                    continue;

                prim?.Destroy();
            }

            Spawned.Clear();
        }

        private static void SpawnBarrier(ArenaPoint barrierPoint)
        {
            var center = barrierPoint.GetWorldPosition();
            var baseRot = barrierPoint.GetWorldRotation();
            SpawnX(center, baseRot);
        }

        private static void SpawnX(Vector3 center, Quaternion rotation)
        {
            const float length = 2f;
            const float height = 0.05f;
            const float thickness = 0.05f;

            var scale = new Vector3(length, height, thickness);
            var flags = PrimitiveFlags.Collidable | PrimitiveFlags.Visible;

            var rot1 = rotation * Quaternion.Euler(0f, 45f, 0f);
            var prim1 = Primitive.Create(
                primitiveType: PrimitiveType.Cube,
                flags: flags,
                position: center,
                rotation: rot1.eulerAngles,
                scale: scale,
                spawn: true,
                color: Color.red
            );

            var rot2 = rotation * Quaternion.Euler(0f, -45f, 0f);
            var prim2 = Primitive.Create(
                primitiveType: PrimitiveType.Cube,
                flags: flags,
                position: center,
                rotation: rot2.eulerAngles,
                scale: scale,
                spawn: true,
                color: Color.red
            );

            Spawned.Add(prim1);
            Spawned.Add(prim2);
        }
    }
}