using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace snake
{
    public partial class Form1 : Form
    {
        Graphics graphics; // 宣告畫布物件
        Snake snake1 = new Snake(); // 蛇物件
        List<Grid> food_list = new List<Grid>(); // 食物物件
        public Random rnd = new Random(); // 亂數物件
        public System.Timers.Timer t = new System.Timers.Timer(30); // 計時器物件
        public Form1()
        {
            InitializeComponent();
            // 在pictureBox1上建立畫布
            graphics = pictureBox1.CreateGraphics();
            // Show()方法顯示窗口空間。必須讓窗口立即顯示，因為在其顯示之前不能作任何工作。
            // 即在其顯示之前畫什麼都是無用的。
            // 如果要在Form的建構子裡面畫圖就要加show()
            //this.Show();
            comboBox1.SelectedIndex = 0; // 設定comboBox的預設值為第0個選項
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // 畫圖的程式碼如果放在Form1_Load會像是什麼事都沒發生，網上查到下面解釋
            /* 
             * Load事件是在窗体首次显示时发生的，在Windows系统中，
             * 窗体的Load事件执行完毕后，系统才开始绘制窗体并显示在屏幕上。
             * 可以理解为，在Form_Load过程中，这个Form里所有需要在屏幕上呈现的东西都还没开始绘制，
             * 所以你在Load中去绘制东西是看不到的。
            */

            // SerialPort設定
            serialPort1.BaudRate = 9600; // arduino已設定9600
            serialPort1.Parity = Parity.None;
            serialPort1.DataBits = 8;
            serialPort1.StopBits = StopBits.One;
            
            String[] portnames = SerialPort.GetPortNames();  // 讀到的SerialPort放到combobox
            foreach (String item in portnames)
            {
                comboBox2.Items.Add(item);
            }
            if(portnames.Length==0)
            {
                button2.Enabled = false;
            }

        }



        public bool is_collide_wall() // 判斷蛇是否撞到牆
        {
            if (snake1.sbody[snake1.sbody.Count-1].row <= -1 || 
                snake1.sbody[snake1.sbody.Count-1].row >= 61 ||
                snake1.sbody[snake1.sbody.Count-1].col <= -1 ||
                snake1.sbody[snake1.sbody.Count-1].col >= 81 )
            {
                return true;
            }
            else
                return false;
        }

        public bool is_collide_self() // 判斷蛇是否撞到自己
        {
            for(int i=0; i<snake1.sbody.Count-1;i++)
            {
                if(snake1.sbody[snake1.sbody.Count-1].row == snake1.sbody[i].row &&
                   snake1.sbody[snake1.sbody.Count-1].col == snake1.sbody[i].col)
                {
                    return true;
                }
            }
            return false;
        }

        public void create_foodlist(int n)  // 產生食物list
        {
            food_list = new List<Grid>(); // 每次產生都要先重置list
            for (int i = 0; i < n; i++)
            {
                food_list.Add(new Grid(i));
                while(isfood_in_food(i)==true)
                {
                    food_list[i].row = rnd.Next(2, 58); //產生亂數
                    food_list[i].col = rnd.Next(2, 78); //產生亂數
                }

            }

        }


        public int isfood_in_snake() // 判斷蛇是否碰到任意一個食物
        {
            foreach(Grid fd in food_list)
            {
                foreach (Grid sb in snake1.sbody) // 檢查蛇的每一格row、col
                {
                    if (sb.row == fd.row && sb.col == fd.col)
                    {
                        return fd.foodindex; //回傳第幾個食物碰到蛇
                    }
                }
            }
            
            return -1;  //設-1為蛇跟食物沒有碰到
        }
        public bool isfood_in_food(int index) // 判斷這個食物是否碰到其他食物
        {
            for(int i =0; i<food_list.Count; i++)
            {
                if (index == i)
                    continue;
                if(food_list[index].row==food_list[i].row && food_list[index].col == food_list[i].col)
                {
                    return true;
                }
            }
            return false;
        }
        /*
        public bool is_eating_food()  // 判斷蛇是否吃到食物
        {
            // 檢查食物座標是否跟蛇的頭座標一樣
            if (snake1.sbody[snake1.sbody.Count-1].row == food.row && snake1.sbody[snake1.sbody.Count-1].col == food.col)
            {
                return true;
            }
            else
                return false;
        }
        */

        public void create_food(int index)  // 產生食物
        {
            int isidentical = 0;
            // 新產生的食物座標不可以跟蛇的身體或是其他食物的座標一樣，否則重新產生
            while (isidentical==0 || isidentical == 1 || isidentical == 2 || isidentical == 3 || isfood_in_food(index) == true)
            {
                food_list[index].row = rnd.Next(2, 58); //產生亂數
                food_list[index].col = rnd.Next(2, 78); //產生亂數
                isidentical = isfood_in_snake();
            }
            food_list[index].Draw_food(graphics);
        }


        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

            label1.Text = "操作方法 : \nW上 \nA左 \nS下 \nD右\n";
            // KeyPress控制蛇的行進方向
            if (e.KeyChar == 'w' || e.KeyChar == 'W')
            {
                label1.Text += "你按下了W";
                snake1.change_direction('W');
            }
            else if (e.KeyChar == 'a' || e.KeyChar == 'A')
            {
                label1.Text += "你按下了A";
                snake1.change_direction('A');
            }
            else if (e.KeyChar == 's' || e.KeyChar == 'S')
            {
                label1.Text += "你按下了S";
                snake1.change_direction('S');
            }
            else if (e.KeyChar == 'd' || e.KeyChar == 'D')
            {
                label1.Text += "你按下了D";
                snake1.change_direction('D');
            }
            else
                label1.Text += "你按下了非WASD鍵";
        }

        public void restart()
        {
            //重開一局
            graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, 802, 602); // 畫面清空
            snake1 = new Snake();
            food_list = new List<Grid>();
            create_foodlist(comboBox1.SelectedIndex + 1);
            t.Enabled = true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if(button1.Text=="遊戲開始")
            {
                timer_start();
                button1.Text = "重新開始";
            }
            else
                restart();


        }



        private void show_info()
        {
            string ss = "蛇的長度 : " + snake1.sbody.Count + "\n";
            /*
            for (int i = 0; i < snake1.sbody.Count; i++)
            {
                ss += "sbody[" + i + "]的index = [" + snake1.sbody[i].row + "," + snake1.sbody[i].col + "]\n";
            }
            */
            ss += "食物數量 : " + food_list.Count;
            label2.Text = (ss);
        }

        /* 計時器 & 執行緒委派
         * System.Timers名稱空間下的Timer類，使用Elapsed事件另開一個執行緒。
         * 定義一個System.Timers.Timer物件，然後繫結Elapsed事件，
         * 通過Start()方法來啟動計時，通過Stop()方法或者Enable=false停止計時
        */
        private void timer_start()
        {
            //t.Interval = 30; //多久執行一次
            t.Enabled = true; //是否執行System.Timers.Timer.Elapsed事件
            t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
            t.AutoReset = true; //每到指定时间Elapsed事件是触发一次(false)还是一直触发(預設)(true)
            t.Start();
        }

        private void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Game();
        }
        private void Game()
        {
            //判斷這個物件是否在同一個執行緒上
            if (this.InvokeRequired)
            {
                // 當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派(調用Invoke方法)
                // 移至Action定義，寫道 public delegate void Action();
                Action invokeAction = new Action(Game); //遞迴??
                this.Invoke(invokeAction);
                Console.WriteLine("Action invoke");
            }
            else
            {
                // 表示在同一個執行緒上了，所以可以正常的呼叫到這個Label物件
                // 遊戲主程式放這
                foreach (Grid fd in food_list)
                {
                    fd.Draw_food(graphics);
                    
                }
                if (isfood_in_snake() != -1) //-1是食物沒碰到蛇
                {
                    snake1.iseating = true;
                    create_food(isfood_in_snake());
                }
                if (is_collide_wall() == true || is_collide_self() == true)
                {
                    //MessageBox放在t.stop()上面會視窗炸彈
                    //因為MessageBox視窗還沒按下確定之前，不會執行到t.stop()
                    //MessageBox.Show("遊戲結束");
                    t.Enabled = false; //計時器停止呼叫System.Timers.Timer.Elapsed事件
                    Console.WriteLine("遊戲結束");
                    MessageBox.Show("遊戲結束");
                }
                else
                {
                    snake1.Move(graphics);
                    show_info();
                    Console.WriteLine("遊戲進行中");
                }
                    
                
                
            }
            
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            // 移動速度
            if(trackBar1.Value==0)
            {
                t.Interval = 140;
            }
            else if(trackBar1.Value == 1)
            {
                t.Interval = 70;
            }
            else if (trackBar1.Value == 2)
            {
                t.Interval = 30;
            }
            else if (trackBar1.Value == 3)
            {
                t.Interval = 16;
            }
            else
            {
                t.Interval = 10;
            }


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 設定食物數量
            create_foodlist(comboBox1.SelectedIndex+1);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 設定serialPort1
            serialPort1.PortName = comboBox2.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(comboBox2.SelectedIndex >= 0) // 有選擇時才執行，不然open()會跳錯誤
            {
                if (button2.Text == "連接SerialPort")
                {
                    serialPort1.Open();  // 連接serialPort
                    button2.Text = "斷開SerialPort";
                    label6.Text = "SerialPort已連接";
                }
                else
                {
                    serialPort1.Close();  // 斷開serialPort
                    button2.Text = "連接SerialPort";
                    label6.Text = "SerialPort未連接";
                }
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // 從serialPort1讀入字串
            String data = serialPort1.ReadExisting();
            snake1.change_direction(data[0]); // 取第一個字元去控制蛇的方向
            Console.WriteLine("收到字元" + data[0]);
        }
    }
}
