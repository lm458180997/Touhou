using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{
    public struct AttackValue
    {
        public float attack;
        public AttackValue(float atk)
        {
            attack = atk;
        }
    }

    public class Bullet : Entity ,RenderLevel,Collider
    {

        protected float attack = 10;     //攻击力
        protected int renderlevel = 0;   //绘画等级
        public BulletBody bulletbody;    //子弹的身体（纹理）
        protected float speed = 50;      //子弹的飞行速度（默认）
        public bool working = true;      //是否处于正常工作
        public bool disabled = false;    //是否应该被销毁
        public bool fromenemy = true;    //是否属于敌人的子弹
        protected int MissTick = 60;     //计算擦弹的计时周期
        protected int MissRefresh = 60;  //擦弹的计时周期刷新时间

        public int getLevel()
        {
            return renderlevel;
        }

        public void setLevel(int i)
        {
            renderlevel = i;
        }

        public virtual void RenderByLevel()
        {
            Render();
        }

        //总体上的逻辑更新（包括一些共有的逻辑任务）
        public override void Update(double ElapsedTime)
        {
            MissTick++;             //擦弹计时器增加
            update(ElapsedTime);
        }
        //供给派生子弹的逻辑更新，多为特定操作（非共有操作）
        public virtual void update(double ElapsedTime)
        {
        }
        //击中目标后的反应
        public virtual void Hit(Entity loader){}

        //碰撞判定
        public virtual bool Collision(Collider c)
        {
            return false;
        }

        //擦弹判定(如果存在擦弹则返回一个坐标信息【道具出现的坐标信息】，返回的道具结果)
        public bool MissBullet(Player p, ref Vector2D position , ref Item itm)
        {
            if (MissTick < MissRefresh)
                return false;
            double x = p.GetX(); double y = p.GetY();    //获取到玩家坐标
            double MissJudgeDistance = 15;                //擦弹判定距离
            if (p.CurrentCharactor.Name == "da")         //不同人物具有不同的擦弹判定距离
            {
                MissJudgeDistance = 25;
            }
            if (MissJudge(MissJudgeDistance, x, y ,ref position , ref itm))
            {
                MissTick = 0;              //擦弹计时器清零（重新进入计时周期）
                return true;
            }

            return false;
        }
        /// <summary>
        /// 可重载的对擦弹判定的描述
        /// </summary>
        /// <param name="missDistance">擦弹判定中心的圆形判定距离</param>
        /// <param name="x">擦弹中心的x坐标</param>
        /// <param name="y">擦弹中心的y坐标</param>
        /// <param name="postion">返回擦弹获得的点数坐标</param>
        /// <returns></returns>
        protected virtual bool MissJudge(double missDistance, double x, double y , ref Vector2D postion , ref Item itm) 
        {
            return false;
        }

    }

    public class Blue_Ray : Bullet
    {
        List<Sprite> Rays_sprites = new List<Sprite>();
        List<Vector2D> positions = new List<Vector2D>();
        public const float per_512 = 0.015625f, per_256 = 0.03125f , dividecount = 64;
        private float radius = 0;           //单位节点的判定半径
        private float distance = 3;         //节点间的间隔距离

        public Blue_Ray(TextureManager _t , Vector2D position , Vector2D direction = null, bool goright = true)
        {
            right = goright;
            Direction = new Vector2D(0, -1);
            speed = 100;
            Texture texture = _t.Get("Rays");
            this.Position = position;        //头坐标
            if (direction != null)
                this.Direction = direction;
             Sprite sp;
             for (int i = 0; i < dividecount; i++)
            {
                //切分为dividecount块
                positions.Add(new Vector2D(position.X, position.Y ));
                sp = new Sprite();
                sp.Texture = texture;
                sp.SetUVs(i / dividecount, per_256 *16, (i + 1) / dividecount, per_256 * 18);
                sp.SetWidth(256/dividecount*1.5f);
                sp.SetHeight(16/2);
                Rays_sprites.Add(sp);
            }
             radius = 8;

        }
        public override void Render()
        {
            //for (int i = 1; i < dividecount; i++)
            //{
            //    Rays_sprites[i].SetPosition(positions[i].X, positions[i].Y);
            //    //方向定义为指向前一个块
            //    Vector2D vct = new Vector2D(positions[i].X - positions[i - 1].X,
            //        positions[i].Y - positions[i - 1].Y);
            //    vct.Normalize();
            //    ag = vct.getcurve();
            //    Stage1State._renderer.DrawSprite(Rays_sprites[i], positions[i].X, positions[i].Y, ag + 90);
            //}
            float ag = 0;
            for (int i = ((int)dividecount-1); i > 0; i--)
            {
                Rays_sprites[i].SetPosition(positions[i].X, positions[i].Y);
                //方向定义为指向前一个块
                Vector2D vct = new Vector2D(positions[i].X - positions[i - 1].X,
                    positions[i].Y - positions[i - 1].Y);
                //如果点没有发生变化，则跳过本次循环
                if (vct.X == 0 && vct.Y == 0)
                    continue;
                vct.Normalize();
                ag = vct.getcurve();
                double x = positions[i].X;
                double y = positions[i].Y;
                if (x < 200 && x > -200 && y > -10 && y < 450)
                    Stage1State._renderer.DrawSprite(Rays_sprites[i], (float)positions[i].X, (float)positions[i].Y, ag + 90);
            }

            ag = Direction.getcurve();
            Rays_sprites[0].SetPosition(positions[0].X, positions[0].Y);
            Stage1State._renderer.DrawSprite(Rays_sprites[0], (float)positions[0].X, (float)positions[0].Y, ag - 90);

        }

        double l_x = 0, l_y = 0;
        bool right = true;
        double angle = 0;
        double timecaculate = 0;
        bool gow = false;
        Random random = new Random();

        public override void Update(double elapsedTime)
        {
            timecaculate += elapsedTime;
            if (timecaculate > 1)
                gow = true;
            if (gow&&timecaculate<10)
            {
                angle += elapsedTime * 360 / 2*((float)random.NextDouble());
                if (angle > 360)
                {
                    angle -= 360;
                    right = !right;
                }
                if (right)
                    Direction.rotate((float)elapsedTime * 360 / 4);
                else
                    Direction.rotate(-(float)elapsedTime * 360 / 4);      //如果想要实现回放功能，则不能使用含有不确定因素的rotate，因为无法插值
            }

            double dx = elapsedTime * Direction.X * speed;
            double dy = elapsedTime * Direction.Y * speed;
            double tx = Position.X + dx;
            double ty = Position.Y + dy;
            Position.X = tx;
            Position.Y = ty;

            if (l_x != tx || l_y != ty)
            {
                if (((l_x - tx) * (l_x - tx) + (l_y - ty)*(l_y - ty)) > distance * distance)
                {
                    //全部向前移
                    for (int i = positions.Count - 1; i > 0; i--)
                    {
                        positions[i].X = positions[i - 1].X;
                        positions[i].Y = positions[i - 1].Y;
                    }
                    positions[0].X = Position.X;
                    positions[0].Y = Position.Y;

                    l_x = tx; l_y = ty;
                }
            }

            //越界后，注销
            float bd = dividecount * distance;
            if (Position.X < -500 || Position.X > 500 || Position.Y < -500 || Position.Y > 1000)
            {
                working = false;
                disabled = true;
            }

        }

        public override bool Collision(Collider c)
        {
            if (c is RoundCollider)
            {
                RoundCollider cc = (RoundCollider)c;
                double r = cc.GetRadius();
                double x = cc.GetX();
                double y = cc.GetY();
                for (int i = 8; i < dividecount-8; i++)
                {
                    if (((x - positions[i].X) * (x - positions[i].X) + (y - positions[i].Y) * (y - positions[i].Y))
                          < (r + radius) * (r + radius))
                    {
                        return true;
                    }
                }

            }

            return false;
        }

    }

    //直线行走的蓝色刀子弹
    public class Direct_BlueKnife :Bullet
    {
        float radius = 5;
        public Direct_BlueKnife(TextureManager texturemanager , Vector2D direction)
        {
            bulletbody = new BlueKnife(texturemanager);
            direction.Normalize();
            this.Direction.X = direction.X;
            this.Direction.Y = direction.Y;
            attack = 20;
            speed = 300;
        }
        public override void Render()
        {
            float ag = Direction.getcurve();
            bulletbody.sprite.SetPosition(Position.X, Position.Y);
            Stage1State._renderer.DrawSprite(bulletbody.sprite, Position.X, Position.Y, ag);
        }

        public override void Update(double elapsedTime)
        {
            base.Update(elapsedTime);
            double dx = elapsedTime * Direction.X * speed;
            double dy = elapsedTime * Direction.Y * speed;
            double tx = Position.X + dx;
            double ty = Position.Y + dy;
            Position.X = tx;
            Position.Y = ty;

            if (Position.X < -200 || Position.X > 200 || Position.Y < -18 || Position.Y > 500)
            {
                speed = 0;
                working = false;
                disabled = true;
            }

        }

        public override bool Collision(Collider c)
        {
            if(working)
            if (c is RoundCollider)
            {
                RoundCollider rd = (RoundCollider)c;
                double r = rd.GetRadius();
                double x = rd.GetX();
                double y = rd.GetY();
                if (((x - Position.X) * (x - Position.X) + (y - Position.Y) * (y - Position.Y))
                    < (r + radius) * (r + radius))
                {
                    working = false;            //停止工作
                    disabled = true;            //允许销毁
                    return true;
                }
            }
            return false;
        }

    }


    //子弹的身体（纹理）基类
    public class BulletBody
    {
        public Sprite sprite;                    //敌人图片所对应的核心sprite
        public const float per_512 = 0.015625f, per_256 = 0.03125f;
        protected string name = "default";
        public MultiSprite multiSprite;
        public string Name
        {
            get { return name; }
        }
        public virtual void Update(double elapsedTime)
        {
            if(multiSprite!=null)
                multiSprite.Update(elapsedTime);
        }
        public virtual void Render() { }

    }

    public class BlueKnife: BulletBody
    {
        public BlueKnife(TextureManager _t)
        {
            Texture texture = _t.Get("etama1");
            name = "blueknife";
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(32);
            sprite.SetHeight(32);
            sprite.SetUVs(0, 18 * per_256, 4 * per_256, 22 * per_256);
        }
        public override void Update(double elapsedTime) {}
    }

    namespace BulletBodys
    {
        /// <summary>
        /// 刀子，皮肤
        /// </summary>
        public class Knife : BulletBody
        {
            public int color;
            public Knife(TextureManager _t, int color)
            {
                Texture texture = _t.Get("etama1");
                name = "Knife";
                sprite = new Sprite();
                sprite.Texture = texture;
                sprite.SetWidth(32);
                sprite.SetHeight(32);
                if (color > 7)
                    color = 7;
                if (color < 0)
                    color = 0;
                sprite.SetUVs(color * 4 * per_256, 8 * per_256, (color + 1) * 4 * per_256, 12 * per_256);

                this.color = color;
            }
        }
        //米弹的皮肤
        public class Rice : BulletBody
        {
            public Rice(TextureManager _t, int color)
            {
                Texture texture = _t.Get("etama1");
                name = "Rice";
                sprite = new Sprite();
                sprite.Texture = texture;
                sprite.SetWidth(16);
                sprite.SetHeight(16);
                if (color > 15)
                    color = 15;
                if (color < 0)
                    color = 0;
                sprite.SetUVs(color * 2 * per_256, 8 * per_256, (color + 1) * 2 * per_256, 10 * per_256);
            }
        }

        //箭头的皮肤
        public class Arrow : BulletBody
        {
            public Arrow(TextureManager _t, int color)
            {
                Texture texture = _t.Get("etama1");
                name = "Arrow";
                sprite = new Sprite();
                sprite.Texture = texture;
                sprite.SetWidth(16);
                sprite.SetHeight(16);
                if (color > 15)
                    color = 15;
                if (color < 0)
                    color = 0;
                sprite.SetUVs(color * 2 * per_256, 10 * per_256, (color + 1) * 2 * per_256, 12 * per_256);
            }
        }

        //闪光的米弹
        public class Rice_R : BulletBody
        {
            public Rice_R(TextureManager _t, int color)
            {
                Texture texture = _t.Get("etama1");
                name = "Rice_R";
                sprite = new Sprite();
                sprite.Texture = texture;
                sprite.SetWidth(16);
                sprite.SetHeight(16);
                if (color > 15)
                    color = 15;
                if (color < 0)
                    color = 0;
                sprite.SetUVs(color * 2 * per_256, 12 * per_256, (color + 1) * 2 * per_256, 14 * per_256);
            }
        }

        //圆形带环子弹
        public class RoundB_A : BulletBody
        {
            public int color;
            public RoundB_A(TextureManager _t, int color)
            {
                this.color = color;
                Texture texture = _t.Get("etama1");
                name = "RoundB_A";
                sprite = new Sprite();
                sprite.Texture = texture;
                sprite.SetWidth(16);
                sprite.SetHeight(16);
                if (color > 15)
                    color = 15;
                if (color < 0)
                    color = 0;
                sprite.SetUVs(color * 2 * per_256, 4 * per_256, (color + 1) * 2 * per_256, 6 * per_256);
            }
        }

        //圆形不带环子弹
        public class RoundB_B : BulletBody
        {
            public int color;
            public RoundB_B(TextureManager _t, int color)
            {
                this.color = color;
                name = "RoundB_B";
                Texture texture = _t.Get("etama1");
                sprite = new Sprite();
                sprite.Texture = texture;
                sprite.SetWidth(16);
                sprite.SetHeight(16);
                if (color > 15)
                    color = 15;
                if (color < 0)
                    color = 0;
                sprite.SetUVs(color * 2 * per_256, 6 * per_256, (color + 1) * 2 * per_256, 8 * per_256);
            }
        }

        //双层米弹
        public class Rice_Double : BulletBody
        {
            public int color;
            public Rice_Double(TextureManager _t, int color)
            {
                this.color = color;
                name = "Rice_Double";
                Texture texture = _t.Get("etama1");
                sprite = new Sprite();
                sprite.Texture = texture;
                sprite.SetWidth(16);
                sprite.SetHeight(16);
                if (color > 15)
                    color = 15;
                if (color < 0)
                    color = 0;
                sprite.SetUVs(color * 2 * per_256, 2 * per_256, (color + 1) * 2 * per_256, 4 * per_256);
            }
        }

        /// <summary>
        /// 大玉
        /// </summary>
        public class BigJade : BulletBody
        {
            public int color;
            public BigJade(TextureManager _t, int color)
            {
                this.color = color;
                name = "BigJade";
                Texture texture = _t.Get("etama1");
                sprite = new Sprite();
                sprite.Texture = texture;
                sprite.SetWidth(32);
                sprite.SetHeight(32);
                if (color > 7)
                    color = 7;
                if (color < 0)
                    color = 0;
                sprite.SetUVs(color * 4 * per_256, 14 * per_256, (color + 1) * 4 * per_256, 18 * per_256);
            }
        }

        /// <summary>
        /// 大蝴蝶
        /// </summary>
        public class BigButterfly : BulletBody
        {
            public int color;
            public BigButterfly(TextureManager _t, int color)
            {
                this.color = color;
                name = "BigButterfly";
                Texture texture = _t.Get("etama1");
                sprite = new Sprite();
                sprite.Texture = texture;
                sprite.SetWidth(32);
                sprite.SetHeight(32);
                if (color > 7)
                    color = 7;
                if (color < 0)
                    color = 0;
                sprite.SetUVs(color * 4 * per_256, 22 * per_256, (color + 1) * 4 * per_256, 26 * per_256);
            }
        }

        /// <summary>
        /// 小玉
        /// </summary>
        public class LittleJade : BulletBody
        {
            public int color;
            public LittleJade(TextureManager _t, int color)
            {
                this.color = color;
                name = "LittleJade";
                Texture texture = _t.Get("etama1");
                sprite = new Sprite();
                sprite.Texture = texture;
                sprite.SetWidth(32);
                sprite.SetHeight(32);
                if (color > 7)
                    color = 7;
                if (color < 0)
                    color = 0;
                sprite.SetUVs(color * 4 * per_256, 26 * per_256, (color + 1) * 4 * per_256, 30 * per_256);
            }
        }

        /// <summary>
        /// 迷你米弹
        /// </summary>
        public class miniRice : BulletBody
        {
            public int color;
            public miniRice(TextureManager _t, int color)
            {
                this.color = color;
                name = "miniRice";
                if (color > 15)
                    color = 15;
                if (color < 0)
                    color = 0;
                Texture texture = _t.Get("etama1");
                sprite = new Sprite();
                sprite.Texture = texture;
                sprite.SetWidth(8);
                sprite.SetHeight(8);
                if (color <= 7)
                {
                    sprite.SetUVs(color * per_256, 30 * per_256, (color + 1) * per_256, 31 * per_256);
                }
                else
                {
                    color = color - 8;
                    sprite.SetUVs(color * per_256, 31 * per_256, (color + 1) * per_256, 32 * per_256);
                }
            }
        }

        /// <summary>
        /// 中型圆弹
        /// </summary>
        public class MiddleRound : BulletBody
        {
            public int color;
            public MiddleRound(TextureManager _t, int color)
            {
                this.color = color;
                name = "MiddleRound";
                Texture texture = _t.Get("etama1");
                sprite = new Sprite();
                sprite.Texture = texture;
                sprite.SetWidth(16);
                sprite.SetHeight(16);
                if (color > 9)
                    color = 9;
                if (color < 0)
                    color = 0;
                sprite.SetUVs((color + 4) * 2 * per_256, 30 * per_256, (color + 5) * 2 * per_256, 32 * per_256);
            }
        }

        /// <summary>
        /// 大星星
        /// </summary>
        public class BigStar : BulletBody
        {
            public int color;
            public BigStar(TextureManager _t, int color)
            {
                name = "BigStar";
                Texture texture = _t.Get("etama6");
                sprite = new Sprite();
                sprite.Texture = texture;
                sprite.SetWidth(32);
                sprite.SetHeight(32);
                if (color > 7)
                    color = 7;
                if (color < 0)
                    color = 0;
                sprite.SetUVs(color * 4 * per_256, 0, (color + 1) * 4 * per_256, 4 * per_256);
                this.color = color;
            }
        }

        /// <summary>
        /// 小星星
        /// </summary>
        public class littleStar : BulletBody
        {
            public int color;
            public littleStar(TextureManager _t, int color)
            {
                name = "littleStar";
                Texture texture = _t.Get("etama6");
                sprite = new Sprite();
                sprite.Texture = texture;
                sprite.SetWidth(16);
                sprite.SetHeight(16);
                if (color > 15)
                    color = 15;
                if (color < 0)
                    color = 0;
                sprite.SetUVs(color * 2 * per_256, 0, (color + 1) * 2 * per_256, 2 * per_256);
                this.color = color;
            }
        }


    }


}
