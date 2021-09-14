
namespace JewelJam
{
    class Jewel : SpriteGameObj
    {
        public int Indx { get; private set; }
        public Jewel(int indx) : base("img/spr_single_jewel" + (indx + 1))
        {
            Indx = indx;
        }
    }
}
