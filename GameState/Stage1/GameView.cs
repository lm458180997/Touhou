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
        }

        public void Start()
        {
            ScoreRenderer = new ScoreItem(texturemanager);
            leftBottomBox = new LeftBottomBox(texturemanager);
        }

        public void Update(double elapsedTime)
        {
          //  _Particles.Update(elapsedTime);
            leftBottomBox.Update(elapsedTime);

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
            Bonus_FontWriter.WriteText(_renderer, bonusFont, "+12", Position.X, Position.Y,Alpha,6);
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
