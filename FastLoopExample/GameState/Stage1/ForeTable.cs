using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.GameState.Stage1
{
    class ForeTable : IGameObject , RenderLevel
    {
        int level = 0;
        Renderer renderer;
        TextureManager texturemanager;
        Sprite[] tableback;
        GameProperties gameproperties;

        public long Scores
        {
            get { return gameproperties.Scores; }
            set { gameproperties.Scores = value; }
        }
        public long HighScores
        {
            get { return gameproperties.HighScores; }
            set { gameproperties.HighScores = value; }
        }

        public int PlayerHelp
        {
            get { return gameproperties.Player; }
            set { gameproperties.Player = value; }
        }

        public int Spell
        {
            get { return gameproperties.Spell; }
            set { gameproperties.Spell = value; }
        }

        public int Power
        {
            get { return gameproperties.Power; }
            set { gameproperties.Power = value; }
        }

        public int Graze
        {
            get { return gameproperties.Graze; }
            set { gameproperties.Graze = value; }
        }

        public int CurrentPoint
        {
            get { return gameproperties.CurrentPoint;}
            set { gameproperties.CurrentPoint = value; }
        }

        public int LevelPoint
        {
            get { return gameproperties.LevelPoint; }
            set { gameproperties.LevelPoint = value; }
        }

        public int CurrentTims
        {
            get { return gameproperties.CurrentTims; }
            set { gameproperties.CurrentTims = value; }
        }

        public int TotleTims
        {
            get { return gameproperties.CurrentTims; }
            set { gameproperties.TotleTims = value; }
        }

        public int GameLevel
        {
            get { return gameproperties.GameLevel; }
            set { gameproperties.GameLevel = value; }
        }

        public ForeTable(TextureManager _t , int level)
        {
            this.level = level;
            texturemanager = _t;
            renderer = new Renderer();
            tableback = new Sprite[4];       //四周的遮掩图块
            Texture texture = _t.Get("foretable1");//("table1");
            tableback[0] = new Sprite();     // 0 上，  1 左， 2 下， 3 右
            tableback[0].Texture = texture;
            tableback[0].SetWidth(640);
            tableback[0].SetHeight(15);
            tableback[0].SetPosition(0, 232.5f  );       //去除顶上的白线（有bug隐患）
            tableback[0].SetUVs(0, 0, 1, 15 / 480.0f);

            tableback[1] = new Sprite();
            tableback[1].Texture = texture;
            tableback[1].SetWidth(30);
            tableback[1].SetHeight(450);
            tableback[1].SetPosition(-306,0);
            tableback[1].SetUVs(0, 15 / 480.0f, 28 / 640.0f, 1 - 15 / 480.0f);

            tableback[2] = new Sprite();
            tableback[2].Texture = texture;
            tableback[2].SetWidth(640);
            tableback[2].SetHeight(15+2);
            tableback[2].SetPosition(0, -232.5f + 1);
            tableback[2].SetUVs(0, 1 - 15 / 480.0f, 1, 1);

            tableback[3] = new Sprite();
            tableback[3].Texture = texture;
            tableback[3].SetWidth(225);
            tableback[3].SetHeight(450);
            tableback[3].SetPosition(209.5f -2,0);
            tableback[3].SetUVs(415 / 640.0f, 15 / 480.0f, 1, 1 - 15 / 480.0f);

            gameproperties = new GameProperties(texturemanager, new Renderer(), 150, 180);
            gameproperties.Scores = 34567890;
        }

        public void Start()
        {

        }

        public void Update(double elapsedTime)
        {
            gameproperties.Update(elapsedTime);

        }

        public void Render()
        {
            for (int i = 0; i < 4;i++ )
                renderer.DrawSprite(tableback[i]);
            gameproperties.Render();
        }

        public int getLevel()
        {
            return level;
        }

        public void setLevel(int i)
        {
            level = i;
        }

        public void RenderByLevel()
        {
            Render();
        }
    }


    //显示游戏数据模块(读取的风神录)
    class GameProperties : IGameObject
    {
        private long highscores;      //最高分
        private long scores;          //当前分
        private int player = 3;       //玩家生命数量
        private int spell = 3;        //Boom数量
        private int power;            //P点数量（表示攻击力，用P点来区分不同层次的火力）
        private int graze;            //擦弹数量
        private int currentpoint;     //当前的蓝点数量
        private int levelpoint;       //当前阶段所需要的总的蓝点数
        private int currenttims;      //阶段收点当前值
        private int totletims;        //阶段收点的目标值
        private int gamelevel;        //游戏难度  0-easy 1-normal 2-hard 3-lunatic
        private float fps;            //屏幕刷新率
        private const float per_512 = 0.015625f;
        private float charactorwidth = 14;  //每个字符的宽度
        private float charactorheight = 14; //每个字符的高度

        private Renderer renderer;                     //着色
        private TextureManager texturemanager;         //纹理管理
        private Point position;                            //坐标（顶部，居中）

        //Dictionary<string, Sprite> NumCharactors;

        Sprite hiScoreSprite;         //hiScore 的Sprite
        float hisc_x = 0, hisc_y = 0; //hiScore 的相对坐标
        Color hisc_c = new Color(1, 1, 1, 1); //hiScore的相对颜色

        Sprite ScSp;           //Score的Sprite
        float Sc_x = 13, Sc_y = -25;
        Color Sc_c = new Color(1, 1, 1, 1);

        Sprite PlSp;           //Player 的Sprite
        float Pl_x = 10, Pl_y = -60;
        Color Pl_c = new Color(1, 1, 1, 1);

        Sprite SpellSp;        //Spell 的Sprite
        float SplSp_x=15, SplSp_y = -85;
        Color SplSp_c = new Color(1, 1, 1, 1);

        Sprite PowSp;          //Power的Sprite
        float Pow_x = 8, Pow_y = -115;
        Color Pow_c = new Color(1, 1, 1, 1);

        Sprite HelpStar;       //表示生命值的星星
        Color HelpStarColor = new Color(1, 1, 1, 1);   //星星的颜色数据

        Sprite SpellStar;       //表示Boom数的星星
        Color SpellColor = new Color(1, 1, 1, 1);      //颜色数据

        Sprite GrazeSp;        //擦弹数量
        float GraSp_x = 12, GraSp_y = -135;
        Color GraSp_c = new Color(1, 1, 1, 1);

        Sprite PoSp;           //蓝点数统计
        float PoSp_x = 12, PoSp_y = -155;
        Color PoSp_c = new Color(1, 1, 1, 1);

        Sprite TimSp;          //收点数统计
        float TimSp_x = 12, TimSp_y = -175;
        Color TimSp_c = new Color(1, 1, 1, 1);

        Sprite[] RankLevel;    //难度选择
        float Ranklevel_x = 120, Ranklevel_y = -200;
        Color Rank_c = new Color(1, 1, 1, 1);

        CharFont textfont;      //字符纹理


        //最高分
        public long HighScores
        {
            get { return highscores; }
            set { highscores = value; }
        }
        //分数
        public long Scores
        {
            get { return scores; }
            set { scores = value; }
        }
        //生命数量
        public int Player
        {
            get { return player; }
            set { player = value; }
        }
        //Boom数量
        public int Spell
        {
            get { return spell; }
            set { spell = value; }
        }
        //P点数量
        public int Power
        {
            get { return power; }
            set { power = value; }
        }
        //擦弹数量
        public int Graze
        {
            get { return graze; }
            set { graze = value; }
        }
        //当前的蓝点数量
        public int CurrentPoint
        {
            get { return currentpoint; }
            set { currentpoint = value; }
        }
        //当前阶段所需要的总的蓝点数
        public int LevelPoint
        {
            get { return levelpoint; }
            set { levelpoint = value; }
        }
        //阶段收点当前值
        public int CurrentTims
        {
            get { return currenttims; }
            set { currenttims = value; }
        }
        //当前所需要的总收点数
        public int TotleTims
        {
            get { return totletims; }
            set { totletims = value; }
        }
        //游戏难度
        public int GameLevel
        {
            get { return gamelevel; }
            set { gamelevel = value; }
        }
        //FPS
        public float FPS
        {
            get { return fps; }
            set { fps = value; }
        }
        //坐标
        public Point Position
        {
            get { return position; }
        }
        public float X
        {
            get { return position.X;}
            set { position.X = value; }
        }
        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public GameProperties(TextureManager _t , Renderer renderer,Point position)
        {
            this.texturemanager = _t;
            this.renderer = renderer;
            this.position = new Point(position.X , position.Y);
            initialize();
        }
        public GameProperties(TextureManager _t, Renderer renderer, float x , float y)
        {
            this.texturemanager = _t;
            this.renderer = renderer;
            this.position = new Point(x, y);
            initialize();
        }

        void initialize()
        {
            textfont = new TextFontEx(texturemanager);
            initializeSprite();
            

        }
        void initializeSprite()
        {
            //基于front00.png的纹理载入，在这里舍弃
            //NumCharactors = new Dictionary<string, Sprite>();
            //Texture texture = texturemanager.Get("front00");
            //NumCharactors = new Dictionary<string, Sprite>();
            //Sprite sp;
            //float _x = 0.5f+ 10*per_512;
            //float _y = 5 * per_512;
            //for (int i = 0; i < 10; i++)
            //{
            //    sp = new Sprite();
            //    sp.Texture = texture;
            //    sp.SetWidth(charactorwidth);
            //    sp.SetHeight(charactorheight);
            //    sp.SetUVs(_x + i * 2 * per_512, _y,_x + (i+1)*2*per_512, _y + 2.5f * per_512);
            //    NumCharactors.Add(i.ToString(), sp);
            //}
            float per_256 = per_512 * 2;
            Texture texture = texturemanager.Get("front00");
            Texture texture2 = texturemanager.Get("front");
            //HighScore
            hiScoreSprite = new Sprite();
            hiScoreSprite.Texture = texture2;
            hiScoreSprite.SetWidth(64);
            hiScoreSprite.SetHeight(16);
            hiScoreSprite.SetUVs(0, 10 * per_256, 8 * per_256, 12 * per_256);

            //Score
            ScSp = new Sprite();
            ScSp.Texture = texture2;
            ScSp.SetWidth(64);
            ScSp.SetHeight(16);
            ScSp.SetUVs(0, 12 * per_256, 8 * per_256, 14 * per_256);

            //Player
            PlSp = new Sprite();
            PlSp.Texture = texture2;
            PlSp.SetWidth(64);
            PlSp.SetHeight(16);
            PlSp.SetUVs(0, 14 * per_256, 8 * per_256, 16 * per_256);
            //helpstar
            HelpStar = new Sprite();
            HelpStar.Texture = texture2;
            HelpStar.SetWidth(16);
            HelpStar.SetHeight(16);
            HelpStar.SetUVs(8 * per_256, 10 * per_256, 10 * per_256, 12 * per_256);

            //Spell
            SpellSp = new Sprite();
            SpellSp.Texture = texture2;
            SpellSp.SetWidth(64);
            SpellSp.SetHeight(16);
            SpellSp.SetUVs(0, 16 * per_256, 8 * per_256, 18 * per_256);

            //spellstar
            SpellStar = new Sprite();
            SpellStar.Texture = texture2;
            SpellStar.SetWidth(16);
            SpellStar.SetHeight(16);
            SpellStar.SetUVs(10 * per_256, 10 * per_256, 12 * per_256, 12 * per_256);

            //Power
            PowSp = new Sprite();
            PowSp.Texture = texture2;
            PowSp.SetWidth(64);
            PowSp.SetHeight(16);
            PowSp.SetUVs(0, 18 * per_256, 8 * per_256, 20 * per_256);

            //Graze
            GrazeSp = new Sprite();
            GrazeSp.Texture = texture2;
            GrazeSp.SetWidth(64);
            GrazeSp.SetHeight(16);
            GrazeSp.SetUVs(0, 20 * per_256, 8 * per_256, 22 * per_256);

            //Point
            PoSp = new Sprite();
            PoSp.Texture = texture2;
            PoSp.SetWidth(64);
            PoSp.SetHeight(16);
            PoSp.SetUVs(0, 22 * per_256, 8 * per_256, 24 * per_256);

            //Tim
            TimSp = new Sprite();
            TimSp.Texture = texture2;
            TimSp.SetWidth(64);
            TimSp.SetHeight(16);
            TimSp.SetUVs(0, 24 * per_256, 8 * per_256, 26 * per_256);

            //RankLevel   难度选择(4 种难度 ， 1种特殊关卡)
            RankLevel = new Sprite[5];
            for (int i = 0; i < 5; i++)
            {
                RankLevel[i] = new Sprite();
                RankLevel[i].Texture = texture;
                RankLevel[i].SetWidth(64);
                RankLevel[i].SetHeight(16);
                RankLevel[i].SetUVs(0.5f + 1 * per_512, 38 * per_512 + i * 2 * per_512,
                    0.5f + 9 * per_512, 40 * per_512 + i * 2 * per_512);
            }


        }

        //将分数对应成字符串
        string scoresToString(long sc,int length)
        {
            string result ="";
            string num = sc.ToString();
            int len = num.Length;
            if (len > length)
            {
                for (int i = 0; i < length-1; i++)
                {
                    result += "9";
                }

                return result;
            }
            for (int i = 0; i < length - len; i++)
            {
                result += "0";
            }
            result += num;
            return result;
        }

        //     提供文本 ， 坐标 ， 字间距离
        void DrawText(string text , float _x , float _y,float dw)
        {
            int len = text.Length;
            Sprite spr;
            for (int i = 0; i < len; i++)
            {
               // spr = NumCharactors[text.Substring(i, 1)];
                spr = textfont.GetAci(text.Substring(i, 1));
                spr.SetHeight(charactorwidth);
                spr.SetWidth(charactorheight);
                spr.SetPosition(new Vector(_x + charactorwidth / 2 + i * dw, _y, 0));
                //spr.SetColor(new Color(1, 1, 1, Alpha));
                renderer.DrawSprite(spr);
            }
        }


        public void setPosition(float x, float y)
        {
            position.X = x; position.Y = y;
        }
        
        public void Start()
        {
            
        }

        public void Update(double elapsedTime)
        {
            
        }

        public void Render()
        {
            //HiScore相关
            hiScoreSprite.SetPosition(X + hisc_x, Y + hisc_y);
            hiScoreSprite.SetColor(hisc_c);
            renderer.DrawSprite(hiScoreSprite);
            DrawText(scoresToString(HighScores,10), X + 30, Y + hisc_y, 12);
            //Score相关
            ScSp.SetPosition(X + Sc_x, Y + Sc_y);
            ScSp.SetColor(Sc_c);
            renderer.DrawSprite(ScSp);
            DrawText(scoresToString(Scores,10), X + 30, Y + Sc_y, 12);
            //Player相关
            PlSp.SetPosition(X + Pl_x, Y + Pl_y);
            PlSp.SetColor(Pl_c);
            renderer.DrawSprite(PlSp);
            HelpStar.SetColor(HelpStarColor);
            for (int i = 0; i < player - 1; i++)
            {
                HelpStar.SetPosition(39 + X + i * 12.5f, Y + Pl_y);
                renderer.DrawSprite(HelpStar);
            }
            //spell
            SpellSp.SetPosition(X + SplSp_x, Y + SplSp_y);
            SpellSp.SetColor(SplSp_c);
            renderer.DrawSprite(SpellSp);
            SpellStar.SetColor(SpellColor);
            for (int i = 0; i < spell; i++)
            {
                SpellStar.SetPosition(39 + X + i * 12.5f, Y + Pl_y-25);
                renderer.DrawSprite(SpellStar);
            }

            //Power相关
            PowSp.SetPosition(X + Pow_x, Y + Pow_y);
            PowSp.SetColor(Pow_c);
            renderer.DrawSprite(PowSp);
            DrawText(power.ToString(), X + 30, Y + Pow_y, 12);

            //Graze 相关
            GrazeSp.SetPosition(X + GraSp_x, Y + GraSp_y);
            GrazeSp.SetColor(GraSp_c);
            renderer.DrawSprite(GrazeSp);
            DrawText(graze.ToString(), X + 30, Y + GraSp_y, 12);

            //Point 蓝点相关
            PoSp.SetPosition(X + PoSp_x, Y + PoSp_y);
            PoSp.SetColor(PoSp_c);
            renderer.DrawSprite(PoSp);
            DrawText(currentpoint.ToString() + "/" + levelpoint.ToString(), X + 30, Y + PoSp_y, 12);

            //Tim 相关
            TimSp.SetPosition(X + TimSp_x, Y + TimSp_y);
            TimSp.SetColor(TimSp_c);
            renderer.DrawSprite(TimSp);
            DrawText(currenttims.ToString()+ "/" + totletims.ToString(), X + 30, Y + TimSp_y, 12);

            //RankLevel 相关
            RankLevel[gamelevel].SetPosition(X + Ranklevel_x, Y + Ranklevel_y);
            RankLevel[gamelevel].SetColor(Rank_c);
            renderer.DrawSprite(RankLevel[gamelevel]);

        }


    }


}
