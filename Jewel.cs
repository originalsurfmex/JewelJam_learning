using JewelJam.engine;

namespace JewelJam
{
    internal class Jewel : SpriteGameObj
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private int Type { get; set; }
        public Jewel(int index) : base("img/spr_single_jewel" + (index + 1))
        {
            Type = index;
        }
    }
}