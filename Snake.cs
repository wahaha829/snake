using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace snake
{
    class Snake
    {
        private int length = 20; // 蛇初始長度
        private char direction = 'D'; //蛇初始行進方向
        // 蛇的身體跟食物都是用Grid這個類產生的，List方便Add跟Remove
        public List<Grid> sbody = new List<Grid>(); 
        Grid abandon_tail = new Grid(-1,-1);  //要刪掉的尾巴，隨便設(-1-1)
        public bool iseating = false;  // 蛇是否吃到食物
        public Snake()
        {
            // 產生蛇
            for(int i=0; i<length; i++)
            {
                sbody.Add(new Grid(0, i));
            }
        }

        public void change_direction(char d)
        {
            direction = d;
        }
        public void Move(Graphics g)
        {
            // 依據direction變數值去把現在蛇頭的下一格新增到sbody作為新的蛇頭
            // 再把原來的蛇尾刪除，製造出蛇移動的效果
            abandon_tail = sbody[0];
            switch (direction)
            {
                case 'W':
                { 
                    sbody.Add(new Grid(sbody[sbody.Count - 1].row-1, sbody[sbody.Count - 1].col));
                }
                    break;
                case 'A':
                {
                    sbody.Add(new Grid(sbody[sbody.Count - 1].row, sbody[sbody.Count - 1].col - 1));
                }
                    break;
                case 'S':
                {
                    sbody.Add(new Grid(sbody[sbody.Count - 1].row+1, sbody[sbody.Count - 1].col ));
                }
                    break;
                case 'D':
                {
                    sbody.Add(new Grid(sbody[sbody.Count - 1].row, sbody[sbody.Count - 1].col + 1));
                }
                    break;
                default:
                {
                    //不會有非WASD的情況
                }
                    break;
            }
            if(this.iseating==true)
            {
                // 若碰到食物，不刪除蛇尾巴
                this.iseating = false;
            }
            else
            {
                // 否則刪除蛇尾巴
                sbody.Remove(sbody[0]); 
            }

            // 把蛇畫出來
            foreach (Grid sb in sbody)
            {
                g.FillRectangle(new SolidBrush(Color.Black), sb.x, sb.y, Grid.width, Grid.width);
            }
            // 蛇尾畫白色
            g.FillRectangle(new SolidBrush(Color.White), abandon_tail.x, abandon_tail.y, Grid.width, Grid.width);

        }

    }
}
