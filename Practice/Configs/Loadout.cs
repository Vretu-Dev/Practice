using System.Collections.Generic;

namespace Practice
{
    public class ClassicLoadoutItem
    {
        public ItemType Item { get; set; }
    }
    public class ClassicLoadout
    {
        public List<ClassicLoadoutItem> Items { get; set; } = new();
    }
}