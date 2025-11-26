using Exiled.API.Interfaces;
using System.Collections.Generic;

namespace Practice
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public List<ClassicLoadout> ClassicLoadouts { get; set; } = new()
        {
            new ClassicLoadout
            {
                Items = new List<ClassicLoadoutItem>
                {
                    new ClassicLoadoutItem { Item = ItemType.GunCrossvec},
                    new ClassicLoadoutItem { Item = ItemType.ArmorCombat},
                    new ClassicLoadoutItem { Item = ItemType.Medkit},
                    new ClassicLoadoutItem { Item = ItemType.KeycardO5},
                }
            },
            new ClassicLoadout
            {
                Items = new List<ClassicLoadoutItem>
                {
                    new ClassicLoadoutItem { Item = ItemType.GunE11SR},
                    new ClassicLoadoutItem { Item = ItemType.ArmorHeavy},
                    new ClassicLoadoutItem { Item = ItemType.Medkit},
                    new ClassicLoadoutItem { Item = ItemType.KeycardO5},
                }
            },
            new ClassicLoadout
            {
                Items = new List<ClassicLoadoutItem>
                {
                    new ClassicLoadoutItem { Item = ItemType.GunFSP9},
                    new ClassicLoadoutItem { Item = ItemType.ArmorLight},
                    new ClassicLoadoutItem { Item = ItemType.Medkit},
                    new ClassicLoadoutItem { Item = ItemType.KeycardO5},
                }
            }
        };
    }
}