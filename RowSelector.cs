using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Numerics;
using JewelJam.engine;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace JewelJam
{
    class RowSelector : SpriteGameObj
    {
        private int selectedRow;
        private JewelGrid grid;
        
        public RowSelector(JewelGrid grid) : base("img/spr_selector_frame")
        {
            this.grid = grid;
            selectedRow = 0;
            _origin = new Vector2(25.0f, 10.0f);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            // move the row selector up or down
            if (inputHelper.KeyPressed(Keys.Up))
                selectedRow--;
            else if (inputHelper.KeyPressed(Keys.Down))
                selectedRow++;
            
            selectedRow = Math.Clamp(selectedRow, 0, grid.YGrids - 1);
            PosLocal = grid.GetCellPos(0, selectedRow);
            
            // shift jewels left or right
            if (inputHelper.KeyPressed(Keys.Right))
                grid.ShiftColumns(selectedRow, false);
            if (inputHelper.KeyPressed(Keys.Left))
                grid.ShiftColumns(selectedRow, true);
        }
    }
    
}