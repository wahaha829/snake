using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace snake
{
    class Grid
    {
        public static int width = 10; // 方格是正方形，長寬用同個變數
        public int x;
        public int y;
        public int row;  // 初始row
        public int col;  // 初始col
        public int foodindex;

        public Grid(int foodindex)
        {
            this.foodindex = foodindex;
            this.x = col * width;
            this.y = row * width;
        }
        public Grid(int row, int col)
        {
            //建構子放參數row、col
            this.row = row;
            this.col = col;
            this.x = col * width;
            this.y = row * width;
        }
        

        public void Draw_food(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.Red), col* width, row* width, width, width);
        }
    }
}
