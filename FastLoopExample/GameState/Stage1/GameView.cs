using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.GameState.Stage1
{

    public class GameView : IGameObject,RenderLevel
    {
        int rendererlevel;
        TextureManager texturemanager;
        SoundManagerEx soundmanager;
        Renderer GameEntityRenderer;       //仅管理游戏对象的renderer
        Renderer GameItem;                 //管理画面前景显示栏目的
        Player player;
        ScoreItem ScoreRenderer;           //分数的着色器
        Particle.Particles _Particles;     //粒子效果
        LeftBottomBox leftBottomBox;       //左下角计分盒
        GameViewComponent.TitleStartBox TitleStartBox;
        GameViewComponent.BGMChangeBox BGMChangeBox;

        public GameView(TextureManager _t, SoundManagerEx _s , int level , Player p)
        {
            texturemanager = _t;
            soundmanager = _s;
            rendererlevel = level;
            this.player = p;
            GameEntityRenderer = Stage1State._renderer;          //获取控制器处的renderer
            // _Particles = new Particle.FlyingFlowersParticles(texturemanager);
            ScoreRenderer = new ScoreItem(_t);

            leftBottomBox = new LeftBottomBox(texturemanager);
            TitleStartBox = new GameViewComponent.TitleStartBox(texturemanager);
            BGMChangeBox = new GameViewComponent.BGMChangeBox(texturemanager);
        }

        public void Start()
        {
            ScoreRenderer = new ScoreItem(texturemanager);
            leftBottomBox = new LeftBottomBox(texturemanager);

            leftBottomBox = new LeftBottomBox(texturemanager);
            TitleStartBox = new GameViewComponent.TitleStartBox(texturemanager,0,220);
            TitleStartBox.BindStage(GameViewComponent.TitleStartBox.Stage1);      //绑定为Stage1

            BGMChangeBox = new GameViewComponent.BGMChangeBox(texturemanager,0,420); //绑定BGM盒

        }

        public void ShowBGM(string name,float percent=0.5f)
        {
            BGMChangeBox.Start(name,percent);
        }

        //显示正式开始
        public void BeginStart()
        {
            TitleStartBox.Start();           //中间标题显示
        }

        public void Update(double elapsedTime)
        {
          //  _Particles.Update(elapsedTime);
            leftBottomBox.Update(elapsedTime);
            TitleStartBox.Update(elapsedTime);
            BGMChangeBox.Update(elapsedTime);
        }

        public void Render()
        {
           // _Particles.Render(Stage1State._renderer);      

            //玩家子弹的渲染
            foreach (Bullet e in Stage1State.Player_Bullets)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (e.getLevel() == i)
                        e.RenderByLevel();
                }
            }
            //玩家自身渲染
            player.Render();
            
            //画敌人
            for (int i = 0; i < 3; i++)
            {
                foreach (Enemy e in Stage1State.Enemys)
                {
                    if (e.getLevel() == i)
                        e.RenderByLevel();
                }
            }

            //画敌人的子弹
            foreach (Bullet b in Stage1State.Enemys_Bullets)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (b.getLevel() == i)
                        b.RenderByLevel();
                }
            }

            //画分数
            foreach (IScore i in Stage1State.ScoreItems)
            {
                ScoreRenderer.DrawNumber(Stage1State._renderer, i, 8);
            }

            //画道具
            foreach (Item i in Stage1State.Items)
            {
                i.Render();
            }

            leftBottomBox.Render(Stage1State._renderer);
            TitleStartBox.Render(Stage1State._renderer);
            BGMChangeBox.Render(Stage1State._renderer);
        }

        public int getLevel()
        {
            return rendererlevel;
        }

        public void setLevel(int i)
        {
            rendererlevel = i;
        }

        public void RenderByLevel()
        {
            Render();
        }
    }

    /// <summary>
    /// 左下角的虫点计分以及bonus得分计数，参照风神录
    /// </summary>
    public class LeftBottomBox
    {
        public Vector2D Position= new Vector2D();

        public int Chong_Points;    //虫点计数
        public int Current_Bonus;   //当前Bonus能够获得的积分
        public int Totle_Bunus;     //当前Bonus获得积分的上限额度
        TextureManager texturemanager;
        const int State_Begin = 0, State_Show = 1, State_Runing = 2, State_Leave = 3;

        Bonus_Font bonusFont;       //Bonus字体工具

        float Alpha = 1;            //透明度                

        public LeftBottomBox(TextureManager texturemanager)
        {
            this.texturemanager = texturemanager;
            bonusFont = new Bonus_Font(texturemanager);
            Position.X = -150;
            Position.Y = 20;
        }
        
        //显示（出场动画）
        public void Show()
        {

        }

        //隐藏（隐藏动画）
        public void Hide()
        {

        }

        //逻辑更新
        public void Update(double ElapsedTime)
        {

        }

        //着色
        public void Render(Renderer _renderer)
        {
            Bonus_FontWriter.WriteText(_renderer, bonusFont, "+99999", Position.X, Position.Y,Alpha,6);
        }

    }

    /// <summary>
    /// 游戏视图左下角的技能能量槽
    /// </summary>
    public class SkillPowerBox
    {
         public Vector2D Position= new Vector2D();

        public int Chong_Points;    //虫点计数
        public int Current_Bonus;   //当前Bonus能够获得的积分
        public int Totle_Bunus;     //当前Bonus获得积分的上限额度
        TextureManager texturemanager;
        const int State_Begin = 0, State_Show = 1, State_Runing = 2, State_Leave = 3;

        Bonus_Font bonusFont;       //Bonus字体工具

        float Alpha = 1;            //透明度                

        public SkillPowerBox(TextureManager texturemanager)
        {
            this.texturemanager = texturemanager;
            bonusFont = new Bonus_Font(texturemanager);
            Position.X = -150;
            Position.Y = 20;
        }
        
        //显示（出场动画）
        public void Show()
        {

        }

        //隐藏（隐藏动画）
        public void Hide()
        {

        }

        //逻辑更新
        public void Update(double ElapsedTime)
        {

        }

        //着色
        public void Render(Renderer _renderer)
        {
            Bonus_FontWriter.WriteText(_renderer, bonusFont, "+99999", Position.X, Position.Y,Alpha,6);
        }
    }


    namespace GameViewComponent
    {
        /// <summary>
        /// 关卡的标题显示
        /// </summary>
        public class TitleStartBox
        {
            public const int Stage1 = 1, Stage2 = 2, Stage3 = 3;//所绑定的状态
            string selectComp = "st01logo";                     //所选择的绑定图
            public Dictionary<string, Sprite> MainSprites;      //容纳核心的着色精灵
            public Dictionary<string, Sprite> BackGround;       //背景图片（待用）
            public TextureManager texturemanager;
            public Vector2D Position;
            public const float per_128 = 0.0625f, per_512 = 0.015625f, per_256 = 0.03125f, PI = 3.1415f;
            
            float Text_Alpha=1;                           //文字的透明度
            float Text_OffsetX = 50;                     //文字X上的偏移量
            float Text_OffsetXMax = 50;                  //最大偏移量
            float Back_Alpha=1;                           //背景的透明度
            float Back_HeightPercent = 1;                 //背景的高度百分比
            float Back_HeightMax= 32 , Back_WidthMax = 352;   //最大大小
            float BackOffsetX = 0;                        //X上偏移量
            bool Working = false;                         //工作是否进行

            public TitleStartBox(TextureManager _t)
            {
                texturemanager = _t;
                Position = new Vector2D(0, 0);
                MainSprites = new Dictionary<string, Sprite>();
                BackGround = new Dictionary<string, Sprite>();
                InvalidateSprites();
            }
            public TitleStartBox(TextureManager _t,double x, double y)
            {
                texturemanager = _t;
                Position = new Vector2D(x, y);
                MainSprites = new Dictionary<string, Sprite>();
                BackGround = new Dictionary<string, Sprite>();
                InvalidateSprites();
            }

            public void BindStage(int stage)
            {
                if (stage <= 1)
                {
                    selectComp = "st01logo";
                }
                else if (stage == 2)
                {
                    //...
                }
                else
                {
                    selectComp = "st01logo";
                }
            }

            void InvalidateSprites()
            {
                Sprite sp = new Sprite();
                Texture texture = texturemanager.Get("st01logo");
                sp.Texture = texture;
                sp.SetWidth(13 * 8);
                sp.SetHeight(2 * 8);
                sp.SetUVs(29 * per_512, 4 * per_128, 42 * per_512, 6 * per_128);
                MainSprites.Add("st01logo", sp);
                sp = new Sprite();
                sp.Texture = texturemanager.Get("st01logo");
                sp.SetUVs(2 * per_512, 6 * per_128, 46 * per_512, 10 * per_128);
                BackGround.Add("st01logo", sp);
            }

            public void Start()
            {
                Working = true;
                InvalidateCommands();
            }

            public void Render(Renderer renderer)
            {
                if (!Working)
                    return;
                //背景
                Sprite sp = BackGround[selectComp];
                sp.SetColor(new Color(1, 1, 1, Back_Alpha*0.7f));
                sp.SetWidth(Back_WidthMax);
                sp.SetHeight(Back_HeightPercent * Back_HeightMax);
                sp.SetPosition(Position.X + BackOffsetX, Position.Y);
                renderer.DrawSprite(sp);
                //文字
                sp = MainSprites[selectComp];
                sp.SetColor(new Color(1, 1, 1, Text_Alpha));
                MainSprites[selectComp].SetPosition(Position.X+Text_OffsetX, Position.Y);
                renderer.DrawSprite(sp);
            }

            ////////// 任务处理结构 ，  同样的结构可以重用于各个模块 ////////////
            /// <summary>
            /// 还可以进一步优化，将一个大队列优化成stack，需要执行时出栈，
            /// 并在执行期间加入正在运行队列，执行结束后从执行队列里删除即可
            /// </summary>

            int Time_caculate = 0;                               //计时器
            List<TCset> Commands = new List<TCset>();            //命令序列
            void InvalidateCommands()
            {
                Time_caculate = 0;
                Commands.Clear();
                Text_Alpha = 0;  //文字的透明度
                Text_OffsetX = 100;   //文字X上的偏移量
                Back_Alpha = 1;  //背景的透明度
                Back_HeightPercent = 0;      //背景的高度百分比
                Back_HeightMax = 32;
                Back_WidthMax = 352;   //最大大小
                BackOffsetX = 0;           //X上偏移量
                AddCommands();
            }
            void AddCommands()
            {
                //back
                TCset Back_AlphaChange = new TCset(150, 200, "Back_AlphaChange", false, true);
                Commands.Add(Back_AlphaChange);
                TCset Back_HeightChange = new TCset(150, 200, "Back_HeightChange", false, true);
                Commands.Add(Back_HeightChange);

                TCset Back_AlphaLeave = new TCset(400, 450, "Back_AlphaLeave", false, true);
                Commands.Add(Back_AlphaLeave);
                TCset Back_HeightLeave = new TCset(400, 450, "Back_HeightLeave", false, true);
                Commands.Add(Back_HeightLeave);

                //front
                TCset Text_AlphaChange = new TCset(100, 200, "Text_AlphaChange", false, true);
                Commands.Add(Text_AlphaChange);
                TCset Text_OffsetChange = new TCset(100, 150, "Text_OffsetChange", false, true);
                Commands.Add(Text_OffsetChange);

                TCset Text_AlphaLeave = new TCset(400, 450, "Text_AlphaLeave", false, true);
                Commands.Add(Text_AlphaLeave);
                TCset Text_OffsetLeave = new TCset(400, 450, "Text_OffsetLeave", false, true);
                Commands.Add(Text_OffsetLeave);

                //Stop
                TCset Stop = new TCset(501, 501, "Stop", false, false);
                Commands.Add(Stop);

            }
            void RunCommands()
            {
                Time_caculate++;
                foreach (TCset tcs in Commands)
                {
                    if (!tcs.UseTick)
                    {
                        #region UnUseTick Commands    不使用计时器的TCset
                        if (tcs.interval == Time_caculate && !tcs.useable)
                        {
                            tcs.useable = true;
                        }
                        if (tcs.useable)
                        {
                            if (tcs.Name == "Stop")
                            {
                                Working = false;
                            }
                            tcs.useable = false; //一次性任务
                        }
                        #endregion
                    }
                    else
                    {
                        #region  UseTick Commands      使用计时器的TCset
                        if (tcs.interval == Time_caculate)
                        {
                            tcs.useable = true;
                        }
                        if (tcs.useable)
                        {
                            if (tcs.Name == "Text_AlphaChange")        //文字的透明度渐变
                            {
                                tcs.TickRun();
                                Text_Alpha = tcs.Tick01 / (float)(tcs.caculateTime - tcs.interval);
                            }
                            if (tcs.Name == "Text_AlphaLeave")         //文字离开时候的透明度渐变
                            {
                                tcs.TickRun();
                                Text_Alpha = 1 - tcs.Tick01 / (float)(tcs.caculateTime - tcs.interval);
                            }
                            if (tcs.Name == "Text_OffsetChange")       //文字x轴上的偏移量变化
                            {
                                tcs.TickRun();
                                float percent =1- tcs.Tick01 / (float)(tcs.caculateTime - tcs.interval);
                                Text_OffsetX = Text_OffsetXMax * percent;
                            }
                            if (tcs.Name == "Text_OffsetLeave")        //文字离开时的x轴上的偏移量变化
                            {
                                tcs.TickRun();
                                float percent = tcs.Tick01 / (float)(tcs.caculateTime - tcs.interval);
                                Text_OffsetX = Text_OffsetXMax * percent;
                            }
                            if (tcs.Name == "Back_AlphaChange")        //背景色的透明值变化
                            {
                                tcs.TickRun();
                                float percent = tcs.Tick01 / (float)(tcs.caculateTime - tcs.interval);
                                Back_Alpha = percent;
                            }
                            if (tcs.Name == "Back_AlphaLeave")         //离开时的背景透明值变化
                            {
                                tcs.TickRun();
                                float percent = 1 - tcs.Tick01 / (float)(tcs.caculateTime - tcs.interval);
                                Back_Alpha = percent;
                            }
                            if (tcs.Name == "Back_HeightChange")       //背景高度变化
                            {
                                tcs.TickRun();
                                float percent = tcs.Tick01 / (float)(tcs.caculateTime - tcs.interval);
                                Back_HeightPercent = percent;
                            }
                            if (tcs.Name == "Back_HeightLeave")       //背景高度变化
                            {
                                tcs.TickRun();
                                float percent = tcs.Tick01 / (float)(tcs.caculateTime - tcs.interval);
                                Back_HeightPercent = 1 - percent;
                            }
                        }
                        if (tcs.caculateTime == Time_caculate)
                        {
                            tcs.useable = false;
                        }
                        #endregion
                    }

                }
            }

            ////////// 任务处理结构 ，  同样的结构可以重用于各个模块 ////////////

            public void Update(double elapsedTime)
            {
                if (!Working)
                    return;
                RunCommands();
            }

        }

        public class BGMChangeBox
        {
            string Name = "Null";
            public Dictionary<string, Sprite> MainSprites;      //容纳核心的着色精灵
            public Dictionary<string, Sprite> BackGround;       //背景图片（待用）
            public TextureManager texturemanager;
            public Vector2D Position;
            public const float per_128 = 0.0625f, per_512 = 0.015625f, per_256 = 0.03125f, PI = 3.1415f;
            
            bool Working = false;                         //工作是否进行

            FontWriterTool writer;
            FontAciHuaKang font;

            float showpercent = 1;             //显示的大小百分比
            const float constOffsetX = 173;
            float offsetX_Max = 200;
            float offsetX = 200;
            float alpha = 0;

            public BGMChangeBox(TextureManager _t)
            {
                texturemanager = _t;
                Position = new Vector2D(0, 0);
                MainSprites = new Dictionary<string, Sprite>();
                BackGround = new Dictionary<string, Sprite>();
                InvalidateSprites();
            }
            public BGMChangeBox(TextureManager _t, double x, double y)
            {
                texturemanager = _t;
                Position = new Vector2D(x, y);
                MainSprites = new Dictionary<string, Sprite>();
                BackGround = new Dictionary<string, Sprite>();

                writer = new FontWriterTool();
                font = new FontAciHuaKang(_t);
                writer.BindFont(font);
                InvalidateSprites();
            }

            void InvalidateSprites()
            {

            }

            public void Start(string name,float percent)
            {
                this.Name = name;
                Working = true;
                this.showpercent = percent;
                offsetX_Max = name.Length * 16 * showpercent;
                offsetX = offsetX_Max;
                InvalidateCommands();
            }

            public void Render(Renderer renderer)
            {
                if (!Working)
                    return;
                int length = Name.Length;
                writer.DrawString(renderer, Name,
                Position.X - offsetX + constOffsetX, Position.Y, showpercent, 200, new Color(1, 1, 1, alpha));
            }

            ////////// 任务处理结构 ，  同样的结构可以重用于各个模块 ////////////
            /// <summary>
            /// 还可以进一步优化，将一个大队列优化成stack，需要执行时出栈，
            /// 并在执行期间加入正在运行队列，执行结束后从执行队列里删除即可
            /// </summary>

            int Time_caculate = 0;                               //计时器
            List<TCset> Commands = new List<TCset>();            //命令序列
            void InvalidateCommands()
            {
                Time_caculate = 0;       //时间置0
                Commands.Clear();
                offsetX_Max = Name.Length * 17 * showpercent;
                offsetX = offsetX_Max;
                alpha = 0;
                AddCommands();
            }
            void AddCommands()
            {
                //back
                TCset ShowAlpha = new TCset(150, 200, "ShowAlpha", false, true);
                Commands.Add(ShowAlpha);
                TCset LeaveALpha = new TCset(600, 650, "LeaveALpha", false, true);
                Commands.Add(LeaveALpha);
                TCset ShowMove = new TCset(150, 200, "ShowMove", false, true);
                Commands.Add(ShowMove);
                TCset LeaveMove = new TCset(600, 650, "LeaveMove", false, true);
                Commands.Add(LeaveMove);

                //Stop
                TCset Stop = new TCset(700, 700, "Stop", false, false);
                Commands.Add(Stop);

            }
            void RunCommands()
            {
                Time_caculate++;
                foreach (TCset tcs in Commands)
                {
                    if (!tcs.UseTick)
                    {
                        #region UnUseTick Commands    不使用计时器的TCset
                        if (tcs.interval == Time_caculate && !tcs.useable)
                        {
                            tcs.useable = true;
                        }
                        if (tcs.useable)
                        {
                            if (tcs.Name == "Stop")
                            {
                                Working = false;
                            }
                            tcs.useable = false; //一次性任务
                        }
                        #endregion
                    }
                    else
                    {
                        #region  UseTick Commands      使用计时器的TCset
                        if (tcs.interval == Time_caculate)
                        {
                            tcs.useable = true;
                        }
                        if (tcs.useable)
                        {
                            if (tcs.Name == "ShowAlpha")       
                            {
                                tcs.TickRun();
                                float percent = tcs.Tick01 / (float)(tcs.caculateTime - tcs.interval);
                                float percent2 = 1 - percent;
                                percent2 = percent2 * percent2;
                                percent = 1 - percent2;
                                alpha = percent;
                            }
                            if (tcs.Name == "LeaveALpha")
                            {
                                tcs.TickRun();
                                float percent =1- tcs.Tick01 / (float)(tcs.caculateTime - tcs.interval);
                                float percent2 = 1 - percent;
                                percent2 = percent2 * percent2;
                                percent = 1 - percent2;
                                alpha = percent;
                            }
                            if (tcs.Name == "ShowMove")
                            {
                                tcs.TickRun();
                                float percent = tcs.Tick01 / (float)(tcs.caculateTime - tcs.interval);
                                float percent2 = 1 - percent;
                                percent2 = percent2 * percent2;
                                percent = 1 - percent2;
                                offsetX = offsetX_Max * 0.5f + offsetX_Max * percent * 0.5f;
                            }
                            if (tcs.Name == "LeaveMove")
                            {
                                tcs.TickRun();
                                float percent =1- tcs.Tick01 / (float)(tcs.caculateTime - tcs.interval);
                                float percent2 = 1 - percent;
                                percent2 = percent2 * percent2;
                                percent = 1 - percent2;
                                offsetX =offsetX_Max*0.8f+ offsetX_Max * percent*0.2f;
                            }
                        }
                        if (tcs.caculateTime == Time_caculate)
                        {
                            tcs.useable = false;
                        }
                        #endregion
                    }

                }
            }

            ////////// 任务处理结构 ，  同样的结构可以重用于各个模块 ////////////

            public void Update(double elapsedTime)
            {
                if (!Working)
                    return;
                RunCommands();
            }

        }

        
        public class BGMChangeBoxEx
        {
            string Name = "Null";
            public Dictionary<string, Sprite> MainSprites;      //容纳核心的着色精灵
            public Dictionary<string, Sprite> BackGround;       //背景图片（待用）
            public TextureManager texturemanager;
            public Vector2D Position;
            public const float per_128 = 0.0625f, per_512 = 0.015625f, per_256 = 0.03125f, PI = 3.1415f;
            
            bool Working = false;                         //工作是否进行

            //显示文字部分
            FontWriterTool writer;
            FontAciHuaKang font;
            double MessagePercent = 0;
            float MessageCaculate = 0;
            float MessageShowTotleTime = 120;
            bool LoadingMessage = false;
            //显示文字部分

            float showpercent = 1;             //显示的大小百分比
            const float constOffsetX = 173;
            float offsetX_Max = 200;
            float offsetX = 200;
            float alpha = 0;

            public BGMChangeBoxEx(TextureManager _t)
            {
                texturemanager = _t;
                Position = new Vector2D(0, 0);
                MainSprites = new Dictionary<string, Sprite>();
                BackGround = new Dictionary<string, Sprite>();
                InvalidateSprites();
            }
            public BGMChangeBoxEx(TextureManager _t, double x, double y)
            {
                texturemanager = _t;
                Position = new Vector2D(x, y);
                MainSprites = new Dictionary<string, Sprite>();
                BackGround = new Dictionary<string, Sprite>();

                writer = new FontWriterTool();
                font = new FontAciHuaKang(_t);
                writer.BindFont(font);
                InvalidateSprites();
            }

            void InvalidateSprites()
            {

            }

            public void Start(string name, float percent)
            {
                this.Name = name;
                Working = true;
                this.showpercent = percent;
                offsetX_Max = name.Length * 16 * showpercent;
                offsetX = offsetX_Max;
                InvalidateCommands();

                LoadingMessage = true;      //开启读取
                MessageCaculate = 0;        //读取计时清0
            }

            public void Render(Renderer renderer)
            {
                if (!Working)
                    return;
                int length = Name.Length;
                writer.DrawString(renderer, Name,
                Position.X - offsetX + constOffsetX, Position.Y, showpercent, 200,
                new Color(1, 1, 1, alpha),(float)MessagePercent);
            }

            ////////// 任务处理结构 ，  同样的结构可以重用于各个模块 ////////////
            /// <summary>
            /// 还可以进一步优化，将一个大队列优化成stack，需要执行时出栈，
            /// 并在执行期间加入正在运行队列，执行结束后从执行队列里删除即可
            /// </summary>

            int Time_caculate = 0;                               //计时器
            List<TCset> Commands = new List<TCset>();            //命令序列
            void InvalidateCommands()
            {
                Time_caculate = 0;       //时间置0
                Commands.Clear();
                offsetX_Max = Name.Length * 17 * showpercent;
                offsetX = offsetX_Max;
                alpha = 0;
                AddCommands();
            }
            void AddCommands()
            {
                //back
                TCset ShowAlpha = new TCset(150, 200, "ShowAlpha", false, true);
                Commands.Add(ShowAlpha);
                TCset LeaveALpha = new TCset(600, 650, "LeaveALpha", false, true);
                Commands.Add(LeaveALpha);
                TCset ShowMove = new TCset(150, 200, "ShowMove", false, true);
                Commands.Add(ShowMove);
                TCset LeaveMove = new TCset(600, 650, "LeaveMove", false, true);
                Commands.Add(LeaveMove);

                //Stop
                TCset Stop = new TCset(700, 700, "Stop", false, false);
                Commands.Add(Stop);

            }
            void RunCommands()
            {
                Time_caculate++;
                foreach (TCset tcs in Commands)
                {
                    if (!tcs.UseTick)
                    {
                        #region UnUseTick Commands    不使用计时器的TCset
                        if (tcs.interval == Time_caculate && !tcs.useable)
                        {
                            tcs.useable = true;
                        }
                        if (tcs.useable)
                        {
                            if (tcs.Name == "Stop")
                            {
                                Working = false;
                            }
                            tcs.useable = false; //一次性任务
                        }
                        #endregion
                    }
                    else
                    {
                        #region  UseTick Commands      使用计时器的TCset
                        if (tcs.interval == Time_caculate)
                        {
                            tcs.useable = true;
                        }
                        if (tcs.useable)
                        {
                            if (tcs.Name == "ShowAlpha")
                            {
                                tcs.TickRun();
                                float percent = tcs.Tick01 / (float)(tcs.caculateTime - tcs.interval);
                                float percent2 = 1 - percent;
                                percent2 = percent2 * percent2;
                                percent = 1 - percent2;
                                alpha = percent;
                            }
                            if (tcs.Name == "LeaveALpha")
                            {
                                tcs.TickRun();
                                float percent = 1 - tcs.Tick01 / (float)(tcs.caculateTime - tcs.interval);
                                float percent2 = 1 - percent;
                                percent2 = percent2 * percent2;
                                percent = 1 - percent2;
                                alpha = percent;
                            }
                            if (tcs.Name == "ShowMove")
                            {
                                tcs.TickRun();
                                float percent = tcs.Tick01 / (float)(tcs.caculateTime - tcs.interval);
                                float percent2 = 1 - percent;
                                percent2 = percent2 * percent2;
                                percent = 1 - percent2;
                                offsetX = offsetX_Max * 0.5f + offsetX_Max * percent * 0.5f;
                            }
                            if (tcs.Name == "LeaveMove")
                            {
                                tcs.TickRun();
                                float percent = 1 - tcs.Tick01 / (float)(tcs.caculateTime - tcs.interval);
                                float percent2 = 1 - percent;
                                percent2 = percent2 * percent2;
                                percent = 1 - percent2;
                                offsetX = offsetX_Max * 0.8f + offsetX_Max * percent * 0.2f;
                            }
                        }
                        if (tcs.caculateTime == Time_caculate)
                        {
                            tcs.useable = false;
                        }
                        #endregion
                    }

                }
            }

            ////////// 任务处理结构 ，  同样的结构可以重用于各个模块 ////////////

            public void Update(double elapsedTime)
            {
                if (!Working)
                    return;
                if (LoadingMessage)
                {
                    MessageCaculate++;
                    MessagePercent = MessageCaculate / MessageShowTotleTime;
                    if (MessageShowTotleTime > 1)
                    {
                        MessageShowTotleTime = 1;
                        LoadingMessage = false;
                    }
                }
                RunCommands();
            }

        }

    }

    public class Bonus_FontWriter
    {
        /// <summary>
        /// 通过Bonus_Font进行书写文字
        /// </summary>
        /// <param name="renderer">着色器</param>
        /// <param name="font">Bonus_Font工具</param>
        /// <param name="str">需要书写的字符串</param>
        /// <param name="x">字符串中心的坐标x位置</param>
        /// <param name="y">字符串中心的坐标y位置</param>
        /// <param name="alpha">显示透明度</param>
        /// <param name="dis">字体间的间距</param>
        public static void WriteText(Renderer renderer,Bonus_Font font, string str, 
            double x, double y, float alpha=1,float dis = 8)
        {
            char[] Arr = str.ToCharArray();
            int lenth = Arr.Length;
            float offset_x = -(lenth / 2) * dis;           //坐标偏移量
            for (int i = 0; i < lenth; i++)
            {
                
                Sprite sp = font.GetChar(Arr[i].ToString());
                if (Arr[i] != '/')
                {
                    sp.SetPosition(x + dis * i + offset_x, y);
                }
                else
                    sp.SetPosition(x + dis * i + offset_x, y+4);
                sp.SetColor(new Color(1, 1, 1, alpha));
                renderer.DrawSprite(sp);
            }
        }


    }

    public class Bonus_Font
    {
        public const float per_512 = 0.015625f, per_256 = 0.03125f, PI = 3.1415f;
        TextureManager texturemanager;
        Dictionary<string, Sprite> Sprites;

        public Bonus_Font(TextureManager _t)
        {
            texturemanager = _t;
            Sprites = new Dictionary<string, Sprite>();

            Sprite sprite;
            Texture texture = _t.Get("fontex");

            for (int i = 0; i < 10; i++)
            {
                sprite = new Sprite();
                sprite.Texture = texture;
                sprite.SetWidth(8);
                sprite.SetHeight(16);
                sprite.SetUVs(i * per_256, 26 * per_256, (i + 1) * per_256, 28 * per_256);
                Sprites.Add(i.ToString(), sprite);
            }
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(8);
            sprite.SetHeight(16);
            sprite.SetUVs(10 * per_256, 26 * per_256, 11 * per_256, 28 * per_256);
            Sprites.Add("%", sprite);

            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(8);
            sprite.SetHeight(16);
            sprite.SetUVs(11 * per_256, 26 * per_256, 12 * per_256, 28 * per_256);
            Sprites.Add(".", sprite);

            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(8);
            sprite.SetHeight(16);
            sprite.SetUVs(12 * per_256, 26 * per_256, 13 * per_256, 28 * per_256);
            Sprites.Add("-", sprite);

            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(8);
            sprite.SetHeight(16);
            sprite.SetUVs(13 * per_256, 26 * per_256, 14 * per_256, 28 * per_256);
            Sprites.Add("+", sprite);

            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(8);
            sprite.SetHeight(16);
            sprite.SetUVs(14 * per_256, 26 * per_256, 15 * per_256, 28 * per_256);
            Sprites.Add("(", sprite);

            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(8);
            sprite.SetHeight(16);
            sprite.SetUVs(15 * per_256, 26 * per_256, 16 * per_256, 28 * per_256);
            Sprites.Add(")", sprite);

            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(8);
            sprite.SetHeight(16);
            sprite.SetUVs(17 * per_256, 24 * per_256, 18 * per_256, 26 * per_256);
            Sprites.Add("/", sprite);
        }

        public Sprite GetChar(string key)
        {
            return Sprites[key];
        }

    }

    class PlayerOne : Entity, RectCollider
    {

        private const float per_512 = 0.015625f , per_256 = 0.03125f;
        Vector2D position;
        public MultiSprite multiSprite;
        Renderer renderer;

        public PlayerOne(TextureManager _t , Renderer renderer)
        {
            this.renderer = renderer;
            Width = 32;
            Height = 48;

            position = new Vector2D(0, 50);

            //Texture texture = _t.Get("Player00");
            //sprite = new Sprite();
            //sprite.Texture = texture;
            //sprite.SetWidth(Width);
            //sprite.SetHeight(Height);
            //sprite.SetPosition(position.X, position.Y);
            //multiSprite = new MultiSprite(sprite);
            //RecTangleF[] rects = new RecTangleF[8];
            //for (int i = 0; i < 8;i++ )
            //    rects[i] = new RecTangleF(0 + i * 4 * per_256, 0, 4 * per_256 + i * 4 * per_256, 6 * per_256);
            //multiSprite.RegeditState(0, rects);      //将此rectangleF注册到0号state
            //multiSprite.State = 0;                   //将状态切换为0号state

            Texture texture = _t.Get("Player2");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(Width);
            sprite.SetHeight(Height);
            sprite.SetPosition(position.X, position.Y);
            multiSprite = new MultiSprite(sprite);

            RecTangleF[] rects = new RecTangleF[4];
            for (int i = 0; i < 4; i++)
                rects[i] = new RecTangleF(0 + i * 4 * per_256, 0, 4 * per_256 + i * 4 * per_256, 6 * per_256);
            multiSprite.RegeditState(0, rects);      //将此rectangleF注册到0号state

            rects = new RecTangleF[3];
            for (int i = 0; i < 3; i++)
                rects[i] = new RecTangleF( i * 4 * per_256, 6 * per_256,
                    4 * per_256 + i * 4 * per_256, 12 * per_256);
            multiSprite.RegeditState(1, rects);      //将此rectangleF注册到1号state (过渡移动)

            rects = new RecTangleF[4];
            for (int i = 0; i < 4; i++)
                rects[i] = new RecTangleF(12*per_256 + i * 4 * per_256, 6*per_256, 
                   12*per_256+ 4 * per_256 + i * 4 * per_256, 12 * per_256);
            multiSprite.RegeditState(2, rects);      //将此rectangleF注册到2号state (移动)

            rects = new RecTangleF[3];
            for (int i = 0; i < 3; i++)
                rects[i] = new RecTangleF(4 * per_256 + i * 4 * per_256, 6 * per_256,
                   i * 4 * per_256, 12 * per_256);
            multiSprite.RegeditState(3, rects);      //将此rectangleF注册到3号state (过渡移动，右)

            rects = new RecTangleF[4];
            for (int i = 0; i < 4; i++)
                rects[i] = new RecTangleF(12 * per_256 + 4 * per_256 + i * 4 * per_256, 6 * per_256,
                12 * per_256 + i * 4 * per_256 , 12 * per_256);
            multiSprite.RegeditState(4, rects);      //将此rectangleF注册到4号state (移动)

            multiSprite.RegeditCollection(1, 2);     //将1号动画连接到2号动画上
            multiSprite.RegeditCollection(3, 4);     //将3号动画连接到4号动画上
            multiSprite.State = 1;                   //将状态切换为1号state
            

        }

        public override void Start()
        {
           
        }
        public override void Render()
        {

            renderer.DrawSprite(sprite);

        }
        public override void Update(double elapsedTime)
        {
            multiSprite.Update(elapsedTime);   //动态纹理也需要随时更新

        }

        public double GetX() { return position.X; }
        public double GetY() { return position.Y; }
        public double GetWidth() { return Width; }
        public double GetHeight() { return Height; }
        //碰撞判定
        public bool Collision(Collider c)
        {
            //矩形碰撞体
            if (c is RectCollider)
            {

            }
            return false;
        }
    }


}
