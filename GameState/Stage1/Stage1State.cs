using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FastLoopExample.GameState.Stage1;
using Tao.OpenGl;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;


namespace FastLoopExample
{
    public class Stage1State : IGameObject
    {
        //int totolTick = 0;
        //int tick = 0;
        //int interval = 60;
        //System.IO.StreamWriter writer ;
        //List<string> datas= new List<string>();
        //void toolupdate()
        //{
        //    totolTick++;
        //    tick++;
        //    if (tick == interval)
        //    {
        //        tick = 0;
        //        datas.Add("tick:"+totolTick.ToString()+ "   "+ GamePlayer.Position.ToString());
        //    }

        //}



        public static Renderer _renderer = new OffsetRenderder(-100,-225); // 着色顶点进行了偏移
        public static Renderer _renderer_normal = new Renderer();          // 普通的着色器

        StateSystem _system;                        // 状态机

        public static TextureManager _textureManager;             // 纹理控制器

        public static SoundManagerEx _soundmanager;               // 声音控制器

        BackGround background;                      // 背景

        Particle.Particles _particles;               // 粒子效果

        Particle.BreakPointsParticles _breakParticles;         // 擦弹溅射粒子效果

        ForeTable foretable;                        // 前景桌面

        GameView gameview;                          // 游戏视图

        Form1 form;                                 // 具有管理整个程序的权限

        int hp = 3;                                 //血量
        int booms = 3;                              //booms数量
        int power = 0;                              //能量数值
        int graze = 0;                              //擦弹数

        public Player GamePlayer;                   // 游戏玩家

        public static List<Enemy> Enemys = new List<Enemy>();                  // 敌人列表
        public static List<Enemy> Enemys_ToRemove = new List<Enemy>();          //敌人的删除列表
        public static List<Enemy> Enemys_ToAdd = new List<Enemy>();             //敌人的添加列表

        public static List<Bullet> Enemys_Bullets_toAdd = new List<Bullet>();   // 添入列表
        public static List<Bullet> Enemys_Bullets = new List<Bullet>();         // 敌人的子弹列表
        public static List<Bullet> Enemys_Bullets_toRemove = new List<Bullet>();// 子弹的删除列表

        public static List<Bullet> Player_Bullets_toAdd = new List<Bullet>();    // 添入列表
        public static List<Bullet> Player_Bullets = new List<Bullet>();          // 自机的子弹列表
        public static List<Bullet> Player_Bullets_toRemove = new List<Bullet>(); // 子弹的删除列表

        public static List<Item> Items_toAdd = new List<Item>();                 //道具的添加列表 
        public static List<Item> Items = new List<Item>();                       //道具列表
        public static List<Item> Items_toRemove = new List<Item>();              //道具的删除列表

        public static List<IScore> ScoreItems= new List<IScore>();               //储存所显示的分数的道具
        public static List<IScore> ScoreItems_ToRemove = new List<IScore>();     //储存所显示的分数的道具的删除列表

        public static double GameTime;             //当前的游戏时间

        public Sprite Logo;       //游戏LOGO
        public FastLoopExample.Extra.VariableSprite  test;  //可变Sprite


        FontAciHuaKang fonc; //文字库（测试）
        FontWriterTool TextWriter;//  文字库使用工具（测试）

        /// <summary>
        /// 游戏中，所进行的游戏的所有任务（什么时候出现怪，什么时候进入boss模式等等）;
        /// 在关卡设计中，极为重要的一个设置参数，它直接决定了游戏的趣味性以及难度;
        /// 内容包括音乐控制，敌机控制，模式转换，子弹&&特效控制，背景控制，存储控制等等核心内容.
        /// Stage1 的核心任务决定器
        /// </summary>
        List<TCset> Stage1_SystemCommands = new List<TCset>();      

        public Stage1State(StateSystem system , TextureManager texturemanager , SoundManagerEx soundmanager ,Form1 form = null)
        {
            _system = system;
            _textureManager = texturemanager;
            _soundmanager = soundmanager;
            if(form != null)
            this.form = form;

            GamePlayer = new Player();
            Datas.CurrentPlayer = GamePlayer;

            gameview = new GameView(texturemanager, soundmanager, 5 , GamePlayer); //中间层着色，注册角色
            GamePlayer.RegisteredRCharactors(Datas.Charactors[Datas.Fore_People_Select],
              Datas.Charactors[Datas.Fore_People_Select]);//  Form1.Charactors[Datas.Back_People_Select]);
            GamePlayer.Position.Y = 30;
            GamePlayer.RenderSpeed = 0.075f;                            //人物帧动画渲染速度

            foretable = new ForeTable(texturemanager, 10);          //最前层着色

            foretable.HighScores = 0;
            foretable.Power = 0;
            foretable.Graze = 0;
            foretable.CurrentPoint = 0;
            foretable.LevelPoint = 1550;
            foretable.CurrentTims = 0;
            foretable.TotleTims = 6400;
            foretable.GameLevel = 1;

            Logo = new Sprite();
            Logo.Texture = texturemanager.Get("logo1");
            Logo.SetWidth(192);
            Logo.SetHeight(192);
            Logo.SetPosition(210, -130);

            test = new Extra.VariableSprite();
            test.BindSprite(Logo);
            test.SetAttribute(210, -130, 0, 192, 192);


            fonc = new FontAciHuaKang(texturemanager);
            TextWriter = new FontWriterTool(fonc);
        }

        public int Hp
        {
            get { return hp; }
            set
            {
                hp = value;
                Datas.Hp = hp;
                foretable.PlayerHelp = hp;
            }
        }
        public int Booms
        {
            get { return booms; }
            set
            {
                booms = value;
                Datas.booms = booms;
                foretable.Spell = value;
            }
        }
        public int Power
        {
            get { return power; }
            set
            {
                power = value;
                Datas.PowerPoint = power;
                foretable.Power = power;
            }
        }
        public int Graze
        {
            get { return graze; }
            set
            {
                graze = value;
                foretable.Graze = value;
            }
        }

        List<TCset> commands = new List<TCset>();

        public void Start()
        {
            _soundmanager.Play("rj");
            _particles = new Particle.FlyingFlowersParticles(_textureManager);
            _breakParticles = new Particle.BreakPointsParticles(_textureManager);
            background = new BackGround(_textureManager, 0, _particles);        //着色等级为0级（顶层着色）
            background.BindParticle(_breakParticles);            //绑定粒子

            background.Start();
            foretable.Start();
            gameview.Start();
            GamePlayer.Start();

            Hp = Datas.Hp_Start;
            Datas.Hp = Hp;
            Booms = Datas.booms;

            Datas.CurrentEnemys = Enemys;                      //将全局的CurrentEnemys指向此场景上（便于）
            Datas.CurrentItemAdd = Items_toAdd;                //将全局道具列表切换到此状态
            Datas.CurrentPlayer = GamePlayer;                  //当前玩家

            GameTime = 0;                                      //初始化游戏时间
            
            //重播模式
            if (Datas.ReFre)
            {
                #region
                Datas.ReadData();
                commands.Clear();
                foreach (string str in Datas.Commands)
                {
                    char[] arr = str.ToCharArray();
                    int st =0, ct=0;
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i] == '$')
                            st = i;
                        if (arr[i] == '#')
                            ct = i;
                    }
                    string name = str.Substring(st+1, ct - st - 1);
                    string value = str.Substring(ct + 1);
                    double time = Convert.ToDouble(value);
                    switch (name)
                    {
                        case "Seed":
                            int seed = (int)time;
                            Datas.GameRandom = new GRandom(seed);
                            break;
                        case "LeftDown" :
                            TCset tcset = new TCset();
                            tcset.Name = "LeftDown";
                            tcset.caculateTime = time;
                            commands.Add(tcset);
                            break;
                        case "LeftUp":
                            tcset = new TCset();
                            tcset.Name = "LeftUp";
                            tcset.caculateTime = time;
                            commands.Add(tcset);
                            break;
                        case "RightDown":
                            tcset = new TCset();
                            tcset.Name = "RightDown";
                            tcset.caculateTime = time;
                            commands.Add(tcset);
                            break;
                        case "RightUp":
                            tcset = new TCset();
                            tcset.Name = "RightUp";
                            tcset.caculateTime = time;
                            commands.Add(tcset);
                            break;
                        case "UpUp":
                            tcset = new TCset();
                            tcset.Name = "UpUp";
                            tcset.caculateTime = time;
                            commands.Add(tcset);
                            break;
                        case "UpDown":
                            tcset = new TCset();
                            tcset.Name = "UpDown";
                            tcset.caculateTime = time;
                            commands.Add(tcset);
                            break;
                        case "DownDown":
                            tcset = new TCset();
                            tcset.Name = "DownDown";
                            tcset.caculateTime = time;
                            commands.Add(tcset);
                            break;
                        case "DownUp":
                            tcset = new TCset();
                            tcset.Name = "DownUp";
                            tcset.caculateTime = time;
                            commands.Add(tcset);
                            break;
                        case "ShiftUp":
                            tcset = new TCset();
                            tcset.Name = "ShiftUp";
                            tcset.caculateTime = time;
                            commands.Add(tcset);
                            break;
                        case "ShiftDown":
                            tcset = new TCset();
                            tcset.Name = "ShiftDown";
                            tcset.caculateTime = time;
                            commands.Add(tcset);
                            break;
                        case "ZDown":
                            tcset = new TCset();
                            tcset.Name = "ZDown";
                            tcset.caculateTime = time;
                            commands.Add(tcset);
                            break;
                        case "ZUp":
                            tcset = new TCset();
                            tcset.Name = "ZUp";
                            tcset.caculateTime = time;
                            commands.Add(tcset);
                            break;
                    }
                #endregion
                }
            }
            //非重播模式
            else
            {
                Datas.GameRandom = new GRandom();
                Datas.Commands = new List<string>();
                Datas.Commands.Add("$Seed#" + Datas.GameRandom.Seed.ToString());
            }

            GameTime = 0;
            Stage1_SystemCommands.Clear();
            AddCommands();                                   //添加按键命令

            background.SpeedY = 0.01f;     //初始速度

            gameview.BeginStart();        //开始执行 
            //gameview.ShowBGM("BGM:Silent Story（發熱巫女～ず）", 0.4f);      //显示BGM, 字体倍率为0.5f
            _soundmanager.SetVolumChannel("Graze", 0.2f);   
        }

        //加载任务（游戏主核心）
        void AddCommands()
        {
            TCset leftOut_120 = new TCset(120 + 300,"leftOut_120",false);    
            Stage1_SystemCommands.Add(leftOut_120);
            TCset rightOut_120 = new TCset(240 + 300+80, "rightOut_120", false);
            Stage1_SystemCommands.Add(rightOut_120);
            TCset leftOut_240 = new TCset(240 + 300, "leftOut_240", false);
            Stage1_SystemCommands.Add(leftOut_240);
            TCset rightOut_240 = new TCset(240 + 300+40, "rightOut_240", false);
            Stage1_SystemCommands.Add(rightOut_240);

            TCset left_loop = new TCset(1200,2000, "leftloop", false, true);
            Stage1_SystemCommands.Add(left_loop);
            TCset right_loop = new TCset(1200, 2000, "rightloop", false, true);
            Stage1_SystemCommands.Add(right_loop);

            //120帧后显示BGM
            TCset ShowBGM = new TCset(120, "ShowBGM1", false);
            Stage1_SystemCommands.Add(ShowBGM);
            TCset BackSpeedChange1 = new TCset(350, "BackSpeedChange1", false);
            Stage1_SystemCommands.Add(BackSpeedChange1);
            TCset BackSpeedChange2 = new TCset(700, "BackSpeedChange2", false);
            Stage1_SystemCommands.Add(BackSpeedChange2);

        }

        double score = 0;
        bool[] keydowning = new bool[4];
        const int Up = 0, Left = 1, Down = 2, Right = 3;
        Vector2D direction = new Vector2D(0,0);
        bool neednormalize = false;           

        void ItemsUpdate(double elapsedTime)
        {
            foretable.Update(elapsedTime);
            foretable.Scores = (long)score;
            background.Update(elapsedTime);
            gameview.Update(elapsedTime);

            ///////////////////////////测试/////////////////////////////////////
           // toolupdate();
            ///////////////////////////////////////////////////////////////////
        }

        //键盘任务
        void KeyDownEvent()
        {
            # region KeyDownEvents
            if (Input.getKeyDown("Left"))    
            {
                neednormalize = true;
                keydowning[Left] = true;
                direction.X = -1;
                Datas.Commands.Add("$LeftDown#" + GameTime.ToString());
            }
            if (Input.getKeyUp("Left"))
            {
                neednormalize = true;
                keydowning[Left] = false;
                if (keydowning[Right])
                    direction.X = 1;
                else
                    direction.X = 0;
                Datas.Commands.Add("$LeftUp#" + GameTime.ToString());
            }
            if (Input.getKeyDown("Right"))
            {
                neednormalize = true;
                keydowning[Right] = true;
                direction.X = 1;
                Datas.Commands.Add("$RightDown#" + GameTime.ToString());
            }
            if (Input.getKeyUp("Right"))
            {
                neednormalize = true;
                keydowning[Right] = false;
                if (keydowning[Left])
                    direction.X = -1;
                else
                    direction.X = 0;
                Datas.Commands.Add("$RightUp#" + GameTime.ToString());
            }
            if (Input.getKeyDown("Up"))
            {
                neednormalize = true;
                keydowning[Up] = true;
                direction.Y = 1;
                Datas.Commands.Add("$UpDown#" + GameTime.ToString());
            }
            if (Input.getKeyUp("Up"))
            {
                neednormalize = true;
                keydowning[Up] = false;
                if (keydowning[Down])
                    direction.Y = -1;
                else
                    direction.Y = 0;
                Datas.Commands.Add("$UpUp#" + GameTime.ToString());
            }
            if (Input.getKeyDown("Down"))
            {
                neednormalize = true;
                keydowning[Down] = true;
                direction.Y = -1;
                Datas.Commands.Add("$DownDown#" + GameTime.ToString());
            }
            if (Input.getKeyUp("Down"))
            {
                neednormalize = true;
                keydowning[Down] = false;
                if (keydowning[Up])
                    direction.Y = 1;
                else
                    direction.Y = 0;
                Datas.Commands.Add("$DownUp#" + GameTime.ToString());
            }
            if (Input.getKeyDown("Shift"))
            {
                GamePlayer.IsFast = false;
                Datas.Commands.Add("$ShiftDown#" + GameTime.ToString());
            }
            if (Input.getKeyUp("Shift"))
            {
                GamePlayer.IsFast = true;
                Datas.Commands.Add("$ShiftUp#" + GameTime.ToString());
            }
            if (neednormalize)
            {
                GamePlayer.Direction = direction.GetNormalize();
                neednormalize = false;
            }
            if (Input.getKeyDown("X"))
            {
                Booms--;
                GamePlayer.Attack();
            }
            if (Input.getKeyDown("Z"))
            {
                GamePlayer.StartFire();
               // GamePlayer.Attack();
                Datas.Commands.Add("$ZDown#" + GameTime.ToString());
            }
            if (Input.getKeyUp("Z"))
            {
                GamePlayer.StopFire();
                Datas.Commands.Add("$ZUp#" + GameTime.ToString());
            }
            if (Input.getKeyDown("Escape"))
            {
                Datas.PrintData();
                //writer = new System.IO.StreamWriter("data.txt");
                //writer.WriteLine(DateTime.Now.ToString());
                //foreach (string s in datas)
                //{
                //    writer.WriteLine(s);
                //}
                //writer.Close();
                //writer.Dispose();

            }
            if (Input.getKeyDown("H"))
            {
                double x = GamePlayer.Position.X;
                double y = GamePlayer.Position.Y;
                int w = 120, h = 120;
                int lenth = w * h * 3;
                x = x + 220 - w / 2; y = y + 15 - h / 2;
                if (x < 0) x = 0; if (x > 280) x = 280;
                if (y < 20) y = 20; if (y > 350) y = 350;
                byte[] dd = new byte[w * h * 3];
                Gl.glReadPixels((int)x, (int)y, w, h, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, dd);
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(w, h);
                PointBitmap lockbmp = new PointBitmap(bmp);
                //锁定Bitmap,通过Pixel访问颜色
                lockbmp.LockBits();
                //设定颜色（也可是获取颜色）
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        byte r = dd[(h - 1 - i) * w * 3 + j * 3];
                        byte g = dd[(h - 1 - i) * w * 3 + j * 3 + 1];
                        byte b = dd[(h - 1 - i) * w * 3 + j * 3 + 2];
                        System.Drawing.Color c = System.Drawing.Color.FromArgb(r, g, b);
                        lockbmp.SetPixel(j, i, c);
                    }
                }
                //从内存中解锁Bitmap
                lockbmp.UnlockBits();
                bmp.Save("aaa.bmp");
            }
            #endregion
        }

        //自动实现按键命令（方向的定义需要集中进行，否则同帧任务操作的时候会出现bug）
        void aotoKeyEvent()
        {
            bool[] DirCommands = new bool[8];
            for (int i = 0; i < 8; i++)
                DirCommands[i] = false;

                for (int i = 0; i < commands.Count; i++)
                {
                    if (commands[i].useable)
                    {
                        if (GameTime == commands[i].caculateTime)
                        {
                            TCset cmd = commands[i];
                            if (cmd.useable)
                            {
                                #region 命令控制
                                switch (cmd.Name)
                                {
                                    case "LeftDown":
                                        neednormalize = true;
                                        DirCommands[0] = true;
                                        break;
                                    case "LeftUp": neednormalize = true;
                                        DirCommands[1] = true;
                                        break;
                                    case "RightDown": neednormalize = true;
                                        DirCommands[2] = true;
                                        break;
                                    case "RightUp": neednormalize = true;
                                        DirCommands[3] = true;
                                        break;
                                    case "UpDown": neednormalize = true;
                                        DirCommands[4] = true;
                                        break;
                                    case "UpUp": neednormalize = true;
                                        DirCommands[5] = true;
                                        break;
                                    case "DownDown": neednormalize = true;
                                        DirCommands[6] = true;
                                        break;
                                    case "DownUp":
                                        neednormalize = true;
                                        DirCommands[7] = true;
                                        break;
                                    case "ShiftDown": GamePlayer.IsFast = false;
                                        break;
                                    case "ShiftUp": GamePlayer.IsFast = true;
                                        break;
                                    case "ZUp": GamePlayer.StopFire();
                                        break;
                                    case "ZDown": GamePlayer.StartFire();
                                        break;
                                    default:
                                        break;
                                }
                                #endregion
                                cmd.useable = false;
                            }
                        }
                    }
                }

                if (neednormalize)
                {
                    if (DirCommands[0])
                    {
                        keydowning[Left] = true;
                        direction.X = -1;
                    }
                    if (DirCommands[1])
                    {
                        keydowning[Left] = false;
                        if (keydowning[Right])
                            direction.X = 1;
                        else
                            direction.X = 0;
                    }
                    if (DirCommands[2])
                    {
                        keydowning[Right] = true;
                        direction.X = 1;
                    }
                    if (DirCommands[3])
                    {
                        keydowning[Right] = false;
                        if (keydowning[Left])
                            direction.X = -1;
                        else
                            direction.X = 0;
                    }
                    if (DirCommands[4])
                    {
                        keydowning[Up] = true;
                        direction.Y = 1;
                    }
                    if (DirCommands[5])
                    {
                        keydowning[Up] = false;
                        if (keydowning[Down])
                            direction.Y = -1;
                        else
                            direction.Y = 0;
                    }
                    if (DirCommands[6])
                    {
                        keydowning[Down] = true;
                        direction.Y = -1;
                    }
                    if (DirCommands[7])
                    {
                        keydowning[Down] = false;
                        if (keydowning[Up])
                            direction.Y = 1;
                        else
                            direction.Y = 0;
                    }
                    GamePlayer.Direction = direction.GetNormalize();
                    neednormalize = false;
                }
        }

        /// <summary>
        /// 运行任务队列————关卡的核心设计
        /// </summary>
        void RunCommands()
        {
            foreach (TCset tcs in Stage1_SystemCommands)
            {

                if (!tcs.UseTick)
                {
                    #region UnUseTick Commands    不使用计时器的TCset
                    if (tcs.interval == GameTime && !tcs.useable)
                    {
                        tcs.useable = true;
                    }
                    if (tcs.useable)
                    {
                        if (tcs.Name == "ShowBGM1")
                        {
                            gameview.ShowBGM("BGM:不安定的神", 0.5f);  
                        }
                        if (tcs.Name == "leftOut_120")
                        {
                            Enemy e = new Stage1.Enemy01(_textureManager, -75, 450);
                            Enemys_ToAdd.Add(e);
                        }
                        else if (tcs.Name == "rightOut_120")
                        {
                            Enemy e = new Stage1.Enemy02_2(_textureManager, 125, 500, false);
                            Enemys_ToAdd.Add(e);
                        }
                        else if (tcs.Name == "leftOut_240")
                        {
                            Enemy e = new Stage1.Enemy02_2(_textureManager, -70, 500,false);
                            Enemys_ToAdd.Add(e);
                        }
                        else if (tcs.Name == "rightOut_240")
                        {
                            Enemy e = new Stage1.Enemy02_2(_textureManager, 125, 500, true);
                            Enemys_ToAdd.Add(e);
                        }
                        else if (tcs.Name == "BackSpeedChange1")
                        {
                            background.SpeedY = 0.04f;
                        }
                        else if (tcs.Name == "BackSpeedChange2")
                        {
                            background.SpeedY = 0.06f;
                        }
                        tcs.useable = false; //一次性任务
                    }
                    #endregion
                }
                else
                {
                    #region  UseTick Commands      使用计时器的TCset
                    if (tcs.interval == GameTime)
                    {
                        tcs.useable = true;
                    }
                    if (tcs.useable)
                    {
                        if (tcs.Name == "leftloop")
                        {
                            tcs.TickRun();
                            if (tcs.Tick01 == 150)
                            {
                                float offset_x = (float)Datas.GameRandom.NextDouble() * 40 - 20;
                                Enemy e = new Stage01_E01(_textureManager, -75+offset_x, 450);
                                Enemys_ToAdd.Add(e);
                                tcs.Tick01 = 0;
                            }
                        }
                        if (tcs.Name == "rightloop")
                        {
                            tcs.TickRun();
                            if (tcs.Tick01 == 150)
                            {
                                float offset_x = (float)Datas.GameRandom.NextDouble() * 40 - 20;
                                Enemy e = new Stage01_E01(_textureManager, 75 + offset_x, 450);
                                Enemys_ToAdd.Add(e);
                                tcs.Tick01 = 0;
                            }
                        }
                    }
                    if (tcs.caculateTime == GameTime)
                    {
                        tcs.useable = false;
                    }
                    #endregion
                }

            }
        }

        //所有游戏对象的逻辑更新
        void EntitiesUpdate(double elapsedTime)
        {
            //玩家逻辑更新
            GamePlayer.Update(elapsedTime);
            //玩家的子弹的队列装填  （从GamePlayer中获取子弹）
            foreach (Bullet b in GamePlayer.bullet_toAdd)
            {
                GamePlayer.Bullets.Add(b);
                Player_Bullets_toAdd.Add(b);
            }
            GamePlayer.bullet_toAdd.Clear();
            //添加玩家子弹进全局
            foreach (Bullet b in Player_Bullets_toAdd)
            {
                Player_Bullets.Add(b);
            }
            Player_Bullets_toAdd.Clear();
            //子弹逻辑更新
            foreach (Bullet b in Player_Bullets)
            {
                b.Update(elapsedTime);
                if (b.disabled)
                {
                    Player_Bullets_toRemove.Add(b);
                }
            }
            //删除无效子弹
            foreach (Bullet b in Player_Bullets_toRemove)
            {
                Player_Bullets.Remove(b);
            }
            Player_Bullets_toRemove.Clear();
            //自机子弹的碰撞判定
            {
                foreach (Bullet b in Player_Bullets)
                {
                    foreach (Enemy e in Enemys)
                    {
                        if (e.living)
                        if (b.Collision(e))
                        {
                               b.Hit(e);     //引发击中事件
                        }
                    }
                }
            }
           
            //敌人逻辑更新
            foreach (Enemy e in Enemys_ToAdd)
            {
                Enemys.Add(e);
            }
            Enemys_ToAdd.Clear();
            for (int i = 0; i < Enemys.Count; i++)
            {
                Enemy e = Enemys[i];
                e.Update(elapsedTime);

                //将敌人的子弹队列填充到整个游戏的子弹队列中
                foreach (Bullet b in e.bullet_toAdd)
                {
                    e.Bullets.Add(b);
                    Enemys_Bullets_toAdd.Add(b);
                }
                e.bullet_toAdd.Clear();
                //消除无效敌机
                if (e.disabled)
                {
                    Enemys_ToRemove.Add(e);
                }
            }
            //删除无效敌机
            foreach (Enemy e in Enemys_ToRemove)
            {
                Enemys.Remove(e);
            }
            Enemys_ToRemove.Clear();

            //添加敌机子弹
            foreach (Bullet b in Enemys_Bullets_toAdd)
            {
                Enemys_Bullets.Add(b);
            }
            Enemys_Bullets_toAdd.Clear();
            //敌人的子弹逻辑更新
            foreach (Bullet b in Enemys_Bullets)
            {
                b.Update(elapsedTime);
                //消除无效子弹
                if (b.disabled)
                {
                    Enemys_Bullets_toRemove.Add(b);
                }
            }
            //删除敌机子弹
            foreach (Bullet b in Enemys_Bullets_toRemove)
            {
                Enemys_Bullets.Remove(b);
            }
            Enemys_Bullets_toRemove.Clear();

            //敌人子弹的碰撞判定 && 技能检测
            foreach (Bullet b in Enemys_Bullets)
            {
                //先判定擦弹，再判定碰撞
                Vector2D vct = new Vector2D(0,0);                //获取的方向矢量
                Item itm = new Item();                           //获取的道具
                if (b.MissBullet(GamePlayer, ref vct, ref itm))  //擦弹判定
                {
                    GamePlayer.MissBullet(b);           //玩家也会方向的触发擦弹状态（因此产生擦弹特效）
                    Items_toAdd.Add(itm);
                    _breakParticles.AddParticle(GamePlayer.Position,1);     //溅射效果
                }
                //碰撞检测
                if (b.Collision(GamePlayer))
                {
                    b.Hit(GamePlayer);
                    if (GamePlayer.Hitted())
                    {
                        Hp--;
                        Booms = 3;
                        Datas.Hp = Hp;
                        Datas.booms = Booms;
                    }
                }
                //玩家技能检测(ComponentMessage)
                MessageManager msg = new MessageManager();
                if (GamePlayer.AttackCollision(b, ref msg))
                {
                    CmpMessage[] mss = msg.SelectMessages(CmpMessage.CollisionMasseageNormal);
                    foreach (CmpMessage ms in mss)
                    {
                        b.AcceptMessage(ms);
                    }
                }

                if (b.disabled == true)                 //子弹销毁
                {
                    Enemys_Bullets_toRemove.Add(b);
                }
            }
            //道具逻辑的更新
            foreach (Item item in Items_toAdd)
            {
                Items.Add(item);
            }
            Items_toAdd.Clear();
            foreach (Item item in Items)
            {
                item.Update(elapsedTime);
                //碰撞判定
                if (item.Collision(GamePlayer))
                {
                    if (item.Type == Item.P_Point)
                    {
                        Power++;
                        item.disabled = true;
                    }
                    if (item.Type == Item.BigP_Point)          //大P点+10w分
                    {
                        Power+=10;
                        item.disabled = true;
                        //获取分数后添加一个分数道具
                        ScoreItems.Add(new IScore(item.Position.X, item.Position.Y + 10, 100000));
                    }
                    if (item.Type == Item.Blue_Point)
                    {
                        itemdata.BluePoint mp = (itemdata.BluePoint)item.ItemData;
                        int score = mp.getsocre();
                        this.score += score;
                        //获取分数后添加一个分数道具
                        ScoreItems.Add(new IScore(item.Position.X, item.Position.Y + 10, score));
                        item.disabled = true;
                    }
                    if (item.Type == Item.Boom_Point)
                    {
                        Booms++;
                        item.disabled = true;
                    }
                    if (item.Type == Item.Help_Point)
                    {
                        Hp++;
                        item.disabled = true;
                    }

                }

                //“擦弹，满力攻击” 不需要经过碰撞判定，直接逻辑处理
                if (item.Type == Item.Miss_Point)
                {
                    if (item.working)
                    {
                        itemdata.MissPoint mp = (itemdata.MissPoint)item.ItemData;
                        int score = mp.getsocre();
                        this.score += score;
                        int point = mp.getPoint();
                        this.Graze += point;
                        //获取分数后添加一个分数道具
                        //ScoreItems.Add(new IScore(item.Position.X, item.Position.Y + 10, score));
                        item.working = false;          //仅工作一次
                        item.disabled = true;          //不显示
                        _soundmanager.Play("Graze");           //设置一个功能，防止过快的重复播放从而影响运行速度
                    }
                }
                if (item.disabled)
                {
                    Items_toRemove.Add(item);
                }
            }
            foreach (Item item in Items_toRemove)
            {
                Items.Remove(item);
            }
            Items_toRemove.Clear();

            //分数道具逻辑
            foreach(IScore i in ScoreItems)
            {
                i.Update(elapsedTime);
                if (i.disabled)
                {
                    ScoreItems_ToRemove.Add(i);
                }
            }
            foreach (IScore i in ScoreItems_ToRemove)
            {
                ScoreItems.Remove(i);
            }
            ScoreItems_ToRemove.Clear();

        }

        public void Update(double elapsedTime)
        {
            score += 100 * elapsedTime;
            ItemsUpdate(elapsedTime);
            //是否为重播模式，非重播模式下就记录好IO命令
            GameTime ++ ;                 //游戏时间计时（帧数记）
            if (!Datas.ReFre)
                KeyDownEvent();
            else
            {
                aotoKeyEvent();                  //自动实现命令
                ///////////////
                //if (Input.getKeyDown("Escape"))
                //{
                //    writer = new System.IO.StreamWriter("data.txt");
                //    writer.WriteLine(DateTime.Now.ToString());
                //    foreach (string s in datas)
                //    {
                //        writer.WriteLine(s);
                //    }
                //    writer.Close();
                //    writer.Dispose();
                //}
                ///////////////
            }

            RunCommands();

            EntitiesUpdate(elapsedTime);
           
            GamePlayer.particle_collider.Collition(_particles, elapsedTime);     //粒子的碰撞


            //calc++;
            //percent = calc / 360;
            //if (percent > 1)
            //   percent = 1;
            //test.SetAttribute(210, -130, percent);
            //test.SetColorVol(new Color(1, 1, 1, 1), new Color(1, 1, 1, percent * percent * percent));
        }
        float calc = 0;
        float percent = 0;
        
        public void Render()
        {
            Gl.glShadeModel(Gl.GL_LINE_SMOOTH);
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            for (int i = 0; i <= 10; i++)
            {
                if (background.getLevel() == i)
                {
                    background.RenderByLevel();
                }
                if (foretable.getLevel() == i)
                {
                    foretable.RenderByLevel();
                }
                if (gameview.getLevel() == i)
                {
                    gameview.RenderByLevel();
                }
            }

            //绘画logo
            //_renderer_normal.DrawSprite(Logo);

            _renderer_normal.DrawSprite(test.GetSprite());

            //写文字（测试）
            //if (percent <= 0.5)
            //{
            //    TextWriter.DrawString(_renderer, "操作键说明:Z键攻击     ",
            //        -100, 200, 0.7f, 300, new Color(1, 1, 1, 1), 2 * percent);
            //}
            //else
            //{
            //    TextWriter.DrawString(_renderer, "操作键说明:Z键攻击     ",
            //        -100, 200, 0.7f, 300, new Color(1, 1, 1, 1), 2 * (1-percent));
            //}
            //TextWriter.DrawString(_renderer, GamePlayer.Position.ToString(),
            //        -100, 200, 0.7f, 300, new Color(1, 1, 1, 1), 1);
            Gl.glFinish();
        }
    }


    public class LockBitmap
    {
        Bitmap source = null;
        IntPtr Iptr = IntPtr.Zero;
        BitmapData bitmapData = null;

        public byte[] Pixels { get; set; }
        public int Depth { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public LockBitmap(Bitmap source)
        {
            this.source = source;
        }

        /// <summary>
        /// Lock bitmap data
        /// </summary>
        public void LockBits()
        {
            try
            {
                // Get width and height of bitmap
                Width = source.Width;
                Height = source.Height;

                // get total locked pixels count
                int PixelCount = Width * Height;

                // Create rectangle to lock
                System.Drawing.Rectangle rect = new  System.Drawing.Rectangle(0, 0, Width, Height);

                // get source bitmap pixel format size
                Depth = System.Drawing.Bitmap.GetPixelFormatSize(source.PixelFormat);

                // Check if bpp (Bits Per Pixel) is 8, 24, or 32
                if (Depth != 8 && Depth != 24 && Depth != 32)
                {
                    throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                }

                // Lock bitmap and return bitmap data
                bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
                                             source.PixelFormat);

                // create byte array to copy pixel values
                int step = Depth / 8;
                Pixels = new byte[PixelCount * step];
                Iptr = bitmapData.Scan0;

                // Copy data from pointer to array
                Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Unlock bitmap data
        /// </summary>
        public void UnlockBits()
        {
            try
            {
                // Copy data from byte array to pointer
                Marshal.Copy(Pixels, 0, Iptr, Pixels.Length);

                // Unlock bitmap data
                source.UnlockBits(bitmapData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public System.Drawing.Color GetPixel(int x, int y)
        {
            System.Drawing.Color clr = System.Drawing.Color.Empty;

            // Get color components count
            int cCount = Depth / 8;

            // Get start index of the specified pixel
            int i = ((y * Width) + x) * cCount;

            if (i > Pixels.Length - cCount)
                throw new IndexOutOfRangeException();

            if (Depth == 32) // For 32 bpp get Red, Green, Blue and Alpha
            {
                byte b = Pixels[i];
                byte g = Pixels[i + 1];
                byte r = Pixels[i + 2];
                byte a = Pixels[i + 3]; // a
                clr = System.Drawing.Color.FromArgb(a, r, g, b);
            }
            if (Depth == 24) // For 24 bpp get Red, Green and Blue
            {
                byte b = Pixels[i];
                byte g = Pixels[i + 1];
                byte r = Pixels[i + 2];
                clr = System.Drawing.Color.FromArgb(r, g, b);
            }
            if (Depth == 8)
            // For 8 bpp get color value (Red, Green and Blue values are the same)
            {
                byte c = Pixels[i];
                clr = System.Drawing.Color.FromArgb(c, c, c);
            }
            return clr;
        }

        /// <summary>
        /// Set the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void SetPixel(int x, int y, System.Drawing.Color color)
        {
            // Get color components count
            int cCount = Depth / 8;

            // Get start index of the specified pixel
            int i = ((y * Width) + x) * cCount;

            if (Depth == 32) // For 32 bpp set Red, Green, Blue and Alpha
            {
                Pixels[i] = color.B;
                Pixels[i + 1] = color.G;
                Pixels[i + 2] = color.R;
                Pixels[i + 3] = color.A;
            }
            if (Depth == 24) // For 24 bpp set Red, Green and Blue
            {
                Pixels[i] = color.B;
                Pixels[i + 1] = color.G;
                Pixels[i + 2] = color.R;
            }
            if (Depth == 8)
            // For 8 bpp set color value (Red, Green and Blue values are the same)
            {
                Pixels[i] = color.B;
            }
        }
    }

    public class PointBitmap
    {
        Bitmap source = null;
        IntPtr Iptr = IntPtr.Zero;
        BitmapData bitmapData = null;

        public int Depth { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public PointBitmap(Bitmap source)
        {
            this.source = source;
        }

        public void LockBits()
        {
            try
            {
                // Get width and height of bitmap
                Width = source.Width;
                Height = source.Height;

                // get total locked pixels count
                int PixelCount = Width * Height;

                // Create rectangle to lock
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, Width, Height);

                // get source bitmap pixel format size
                Depth = System.Drawing.Bitmap.GetPixelFormatSize(source.PixelFormat);

                // Check if bpp (Bits Per Pixel) is 8, 24, or 32
                if (Depth != 8 && Depth != 24 && Depth != 32)
                {
                    throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                }

                // Lock bitmap and return bitmap data
                bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
                                             source.PixelFormat);

                //得到首地址
                unsafe
                {
                    Iptr = bitmapData.Scan0;
                    //二维图像循环

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UnlockBits()
        {
            try
            {
                source.UnlockBits(bitmapData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Drawing.Color GetPixel(int x, int y)
        {
            unsafe
            {
                byte* ptr = (byte*)Iptr;
                ptr = ptr + bitmapData.Stride * y;
                ptr += Depth * x / 8;
                System.Drawing.Color c = System.Drawing.Color.Empty;
                if (Depth == 32)
                {
                    int a = ptr[3];
                    int r = ptr[2];
                    int g = ptr[1];
                    int b = ptr[0];
                    c = System.Drawing.Color.FromArgb(a, r, g, b);
                }
                else if (Depth == 24)
                {
                    int r = ptr[2];
                    int g = ptr[1];
                    int b = ptr[0];
                    c = System.Drawing.Color.FromArgb(r, g, b);
                }
                else if (Depth == 8)
                {
                    int r = ptr[0];
                    c = System.Drawing.Color.FromArgb(r, r, r);
                }
                return c;
            }
        }

        public void SetPixel(int x, int y, System.Drawing.Color c)
        {
            unsafe
            {
                byte* ptr = (byte*)Iptr;
                ptr = ptr + bitmapData.Stride * y;
                ptr += Depth * x / 8;
                if (Depth == 32)
                {
                    ptr[3] = c.A;
                    ptr[2] = c.R;
                    ptr[1] = c.G;
                    ptr[0] = c.B;
                }
                else if (Depth == 24)
                {
                    ptr[2] = c.R;
                    ptr[1] = c.G;
                    ptr[0] = c.B;
                }
                else if (Depth == 8)
                {
                    ptr[2] = c.R;
                    ptr[1] = c.G;
                    ptr[0] = c.B;
                }
            }
        }
    }

}


