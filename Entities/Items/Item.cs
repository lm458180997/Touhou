using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{
    namespace itemdata
    {
        public class ItemData
        {
            public string name = "default";
            public int score;
            public int getsocre()
            {
                return score;
            }
        }
        //P点数据
        public class P_PointData : ItemData
        {
            public P_PointData(int score)
            {
                this.score = score;
                name = "P_PointData";
            }
        }
        //擦弹点数据
        public class MissPoint : ItemData
        {
            int count = 1;
            public MissPoint(int score,int count = 1)
            {
                this.score = score;
                name = "MissPoint";
                this.count = count;
            }
            public int getPoint()
            {
                return count;
            }
        }
        //蓝点的数据
        public class BluePoint: ItemData
        {
            public BluePoint ( int score)
            {
                this.score = score;
            }
        }

    }

    //掉落道具
    public class Item : Entity,RoundCollider
    {
        public const int P_Point = 100, BigP_Point = 101 , Miss_Point = 102 ,Blue_Point= 103
            ,Boom_Point = 104 , Help_Point = 105;   //道具的类型

        protected Vector2D OriPosition = new Vector2D();                    //起始坐标点
        protected Vector2D breakPosition = new Vector2D();                  //爆炸对应的坐标
        public float radius = 15;                //半径
        public bool working = true;              //是否处于工作状态
        public bool disabled = false;            //是否已经被注销
        public Sprite littleState;               //小型状态时的纹理
        public int Type = P_Point;               //类型
        public itemdata.ItemData ItemData;       //所携带的道具数据信息（部分对象会用到）

        public float breakLength = 50;           //弹射的距离
        public float breaklenth_max = 50;        //弹射的最大距离
        public double timecaculate = 0;          //计时器
        public float break_time = 0.2f;          //爆发的持续时间（建议按二次幂函数模拟弹射效果）
        
        bool breaking = true;                    //是否处于弹射状态

        public float dop_addspeed = 10;          //下落的加速度
        public float dropSpeed = 10;             //下落速度
        public float drop_time = 1;              //下落的加速速度（可以按二次幂函数模拟，向下加速逐渐减小）
        public const float per_512 = 0.015625f, per_256 = 0.03125f , PI = 3.1415f;

        public Item()
        {
            //从中获取随机的弹射距离和随机的方向角
            breakLength = (float)Datas.GameRandom.NextDouble() * breaklenth_max; 
            double angle = 2 * PI * Datas.GameRandom.NextDouble();
            Direction.X = (float)Math.Cos(angle);
            Direction.Y = (float)Math.Sin(angle);
            Direction.Normalize();
        }


        public double GetX()
        {
            return Position.X;
        }

        public double GetY()
        {
            return Position.Y;
        }

        public double GetRadius()
        {
            return radius;
        }

        public override void Update(double elapsedTime)
        {

            //越界则注销
            if (Position.X < -180 || Position.X > 180 || Position.Y < -8)
            {
                this.working = false;
                this.disabled = true;
            }
            //计时
            timecaculate += elapsedTime;
            //发射阶段
            if (working)
                if (breaking)
                {
                    if (timecaculate <= break_time)
                    {
                        float percent = (float)(break_time - timecaculate) / break_time;
                        percent = 1 - percent;
                        percent = percent * percent;
                        Position.X = (breakPosition.X - OriPosition.X) * percent + OriPosition.X;
                        Position.Y = (breakPosition.Y - OriPosition.Y) * percent + OriPosition.Y;
                    }
                    else
                    {
                        double dt = timecaculate - break_time;
                        Position.X = breakPosition.X;
                        Position.Y = breakPosition.Y;
                        breaking = false;
                        timecaculate = 0;
                        Update(dt);
                    }
                }
                //下落阶段
                else
                {
                    double dy = elapsedTime * dropSpeed;
                    double ty = Position.Y - dy;
                    Position.Y = ty;
                }
        }

        public virtual bool Collision(Collider c)
        {
            if (working)
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
                        Hit();
                        return true;
                    }
                }
            return false;
        }

        public virtual void Hit()
        {

        }

        public virtual void Render(Renderer renderer)
        {

        }
        
    }

    //P点
    public class PowerPoint : Item
    {
        float speed2 = 200;         //靠近玩家时自动靠近的速度
        float distance2 = 900;      //"是否靠近"玩家的判定距离，【以平方计算】

        //构造函数都需要继承基类构造函数
        public PowerPoint(TextureManager texturemanager , float X, float Y): base ()
        {
            //确定类型
            this.Type = Item.P_Point;

            //确定弹射范围s
            Position.X = X; Position.Y = Y;
            OriPosition = new Vector2D(Position.X, Position.Y);
            breakPosition = new Vector2D(Direction.X * breakLength + Position.X, Direction.Y * breakLength + Position.Y);

            //正常状态
            Texture texture = texturemanager.Get("Ef_etama2");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetHeight(16);
            sprite.SetWidth(16);
            sprite.SetUVs(0, 8 * per_256, 2 * per_256, 10 * per_256);

            //顶部状态（三角形形态）
            Texture texture2 = texturemanager.Get("Ef_etama2");
            littleState = new Sprite();
            littleState.Texture = texture2;
            littleState.SetWidth(16);
            littleState.SetHeight(16);
            littleState.SetUVs(16 * per_256, 8 * per_256, 18 * per_256, 10 * per_256);
        }
        public PowerPoint(TextureManager texturemanager, double X, double Y)
            : base()
        {
            this.Type = Item.P_Point;
            //确定弹射范围
            Position.X = X; Position.Y = Y;
            OriPosition = new Vector2D(Position.X, Position.Y);
            breakPosition = new Vector2D(Direction.X * breakLength + Position.X, Direction.Y * breakLength + Position.Y);

            //正常状态
            Texture texture = texturemanager.Get("Ef_etama2");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetHeight(16);
            sprite.SetWidth(16);
            sprite.SetUVs(0, 8 * per_256, 2 * per_256, 10 * per_256);

            //顶部状态（三角形形态）
            Texture texture2 = texturemanager.Get("Ef_etama2");
            littleState = new Sprite();
            littleState.Texture = texture2;
            littleState.SetWidth(16);
            littleState.SetHeight(16);
            littleState.SetUVs(16 * per_256, 8 * per_256, 18 * per_256, 10 * per_256);
        }

        public override void Update(double elapsedTime)
        {
            base.Update(elapsedTime);
            double x= Datas.CurrentPlayer.Position.X;
            double y = Datas.CurrentPlayer.Position.Y;
            double dis = (Position.X - x) * (Position.X - x) + (Position.Y - y) * (Position.Y - y);
            if (dis < distance2)
            {
                Vector2D vct = new Vector2D(x - Position.X,
                       y - Position.Y);
                vct.Normalize();
                x = elapsedTime * vct.X * speed2;
                y = elapsedTime * vct.Y * speed2;
                x = Position.X + x;
                y = Position.Y + y;
                Position.X = x;
                Position.Y = y;
            }

        }

        public override void Render()
        {
            if (Position.Y < 450)
            {
                sprite.SetPosition(Position.X, Position.Y);
                Stage1State._renderer.DrawSprite(sprite);
            }
            else
            {
                littleState.SetPosition(Position.X, 450);
                Stage1State._renderer.DrawSprite(littleState);
            }
        }

        public override void Render(Renderer renderer)
        {
            if (Position.Y < 450)
            {
                sprite.SetPosition(Position.X, Position.Y);
                renderer.DrawSprite(sprite);
            }
            else
            {
                littleState.SetPosition(Position.X, 450);
                renderer.DrawSprite(littleState);
            }
        }
    }

    //大P点
    public class GratePowerPoint : Item
    {
        float speed2 = 200;         //靠近玩家时自动靠近的速度
        float distance2 = 900;      //"是否靠近"玩家的判定距离，【以平方计算】

        //构造函数都需要继承基类构造函数
        public GratePowerPoint(TextureManager texturemanager, float X, float Y)
            : base()
        {
            this.Type = Item.BigP_Point;
            //确定弹射范围
            Position.X = X; Position.Y = Y;
            OriPosition = new Vector2D(Position.X, Position.Y);
            breakPosition = new Vector2D(Direction.X * breakLength + Position.X, Direction.Y * breakLength + Position.Y);

            //正常状态
            Texture texture = texturemanager.Get("Ef_etama2");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetHeight(16);
            sprite.SetWidth(16);
            sprite.SetUVs(4*per_256, 8 * per_256, 6 * per_256, 10 * per_256);

            //顶部状态（三角形形态）
            Texture texture2 = texturemanager.Get("Ef_etama2");
            littleState = new Sprite();
            littleState.Texture = texture2;
            littleState.SetWidth(16);
            littleState.SetHeight(16);
            littleState.SetUVs(20 * per_256, 8 * per_256, 22 * per_256, 10 * per_256);
        }
        //构造函数都需要继承基类构造函数
        public GratePowerPoint(TextureManager texturemanager, double X, double Y)
            : base()
        {
            this.Type = Item.BigP_Point;
            //确定弹射范围
            Position.X = X; Position.Y = Y;
            OriPosition = new Vector2D(Position.X, Position.Y);
            breakPosition = new Vector2D(Direction.X * breakLength + Position.X, Direction.Y * breakLength + Position.Y);

            //正常状态
            Texture texture = texturemanager.Get("Ef_etama2");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetHeight(16);
            sprite.SetWidth(16);
            sprite.SetUVs(4.1f * per_256, 8 * per_256, 6 * per_256, 9.9f * per_256);

            //顶部状态（三角形形态）
            Texture texture2 = texturemanager.Get("Ef_etama2");
            littleState = new Sprite();
            littleState.Texture = texture2;
            littleState.SetWidth(16);
            littleState.SetHeight(16);
            littleState.SetUVs(20 * per_256, 8 * per_256, 22 * per_256, 10 * per_256);
        }
        public override void Update(double elapsedTime)
        {
            base.Update(elapsedTime);
            double x = Datas.CurrentPlayer.Position.X;
            double y = Datas.CurrentPlayer.Position.Y;
            double dis = (Position.X - x) * (Position.X - x) + (Position.Y - y) * (Position.Y - y);
            if (dis < distance2)
            {
                Vector2D vct = new Vector2D(x - Position.X,
                       y - Position.Y);
                vct.Normalize();
                x = elapsedTime * vct.X * speed2;
                y = elapsedTime * vct.Y * speed2;
                x = Position.X + x;
                y = Position.Y + y;
                Position.X = x;
                Position.Y = y;
            }
        }

        public override void Render()
        {
            if (Position.Y < 400)
            {
                sprite.SetPosition(Position.X, Position.Y);
                Stage1State._renderer.DrawSprite(sprite);
            }
            else
            {
                sprite.SetPosition(Position.X, 400);
                Stage1State._renderer.DrawSprite(littleState);
            }
        }

        public override void Render(Renderer renderer)
        {
            if (Position.Y < 400)
            {
                sprite.SetPosition(Position.X, Position.Y);
                renderer.DrawSprite(sprite);
            }
            else
            {
                sprite.SetPosition(Position.X, 400);
                renderer.DrawSprite(littleState);
            }
        }

    }

    //蓝点
    public class GrazePoint : Item
    {
        float speed2 = 200;         //靠近玩家时自动靠近的速度
        float distance2 = 900;      //"是否靠近"玩家的判定距离，【以平方计算】
        TextureManager texturemanager;  //纹理管理

        //构造函数都需要继承基类构造函数
        public GrazePoint(TextureManager texturemanager, float X, float Y , int score)
            : base()
        {
            this.Type = Item.Blue_Point;
            //确定弹射范围
            Position.X = X; Position.Y = Y;
            OriPosition = new Vector2D(Position.X, Position.Y);
            breakPosition = new Vector2D(Direction.X * breakLength + Position.X, Direction.Y * breakLength + Position.Y);
            this.texturemanager = texturemanager;
            ItemData = new itemdata.BluePoint(score);        //携带分数信息
            initialize();
        }
        //构造函数都需要继承基类构造函数
        public GrazePoint(TextureManager texturemanager, double X, double Y,int score)
            : base()
        {
            this.Type = Item.Blue_Point;
            //确定弹射范围
            Position.X = X; Position.Y = Y;
            OriPosition = new Vector2D(Position.X, Position.Y);
            breakPosition = new Vector2D(Direction.X * breakLength + Position.X, Direction.Y * breakLength + Position.Y);
            this.texturemanager = texturemanager;
            ItemData = new itemdata.BluePoint(score);         //携带分数信息
            initialize();
        }

        void initialize()
        {
            //正常状态
            Texture texture = texturemanager.Get("Ef_etama2");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetHeight(16);
            sprite.SetWidth(16);
            sprite.SetUVs(2.1f * per_256, 8 * per_256, 3.9f * per_256, 10 * per_256);

            //顶部状态（三角形形态）
            Texture texture2 = texturemanager.Get("Ef_etama2");
            littleState = new Sprite();
            littleState.Texture = texture2;
            littleState.SetWidth(16);
            littleState.SetHeight(16);
            littleState.SetUVs(18 * per_256, 8 * per_256, 20 * per_256, 10 * per_256);
        }

        public override void Update(double elapsedTime)
        {
            base.Update(elapsedTime);
            double x = Datas.CurrentPlayer.Position.X;
            double y = Datas.CurrentPlayer.Position.Y;
            double dis = (Position.X - x) * (Position.X - x) + (Position.Y - y) * (Position.Y - y);
            if (dis < distance2)
            {
                Vector2D vct = new Vector2D(x - Position.X,
                       y - Position.Y);
                vct.Normalize();
                x = elapsedTime * vct.X * speed2;
                y = elapsedTime * vct.Y * speed2;
                x = Position.X + x;
                y = Position.Y + y;
                Position.X = x;
                Position.Y = y;
            }
        }

        public override void Render()
        {
            if (Position.Y < 400)
            {
                sprite.SetPosition(Position.X, Position.Y);
                Stage1State._renderer.DrawSprite(sprite);
            }
            else
            {
                sprite.SetPosition(Position.X, 400);
                Stage1State._renderer.DrawSprite(littleState);
            }
        }

        public override void Render(Renderer renderer)
        {
            if (Position.Y < 400)
            {
                sprite.SetPosition(Position.X, Position.Y);
                renderer.DrawSprite(sprite);
            }
            else
            {
                sprite.SetPosition(Position.X, 400);
                renderer.DrawSprite(littleState);
            }
        }
    }

    //Boom点
    public class BoomPoint : Item
    {
         float speed2 = 200;         //靠近玩家时自动靠近的速度
        float distance2 = 900;      //"是否靠近"玩家的判定距离，【以平方计算】

        //构造函数都需要继承基类构造函数
        public BoomPoint(TextureManager texturemanager, float X, float Y)
            : base()
        {
            this.Type = Item.Boom_Point;
            //确定弹射范围
            Position.X = X; Position.Y = Y;
            OriPosition = new Vector2D(Position.X, Position.Y);
            breakPosition = new Vector2D(Direction.X * breakLength + Position.X, Direction.Y * breakLength + Position.Y);

            //正常状态
            Texture texture = texturemanager.Get("Ef_etama2");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetHeight(16);
            sprite.SetWidth(16);
            sprite.SetUVs(6*per_256, 8 * per_256, 8 * per_256, 10 * per_256);

            //顶部状态（三角形形态）
            Texture texture2 = texturemanager.Get("Ef_etama2");
            littleState = new Sprite();
            littleState.Texture = texture2;
            littleState.SetWidth(16);
            littleState.SetHeight(16);
            littleState.SetUVs(22 * per_256, 8 * per_256, 24 * per_256, 10 * per_256);
        }
        //构造函数都需要继承基类构造函数
        public BoomPoint(TextureManager texturemanager, double X, double Y)
            : base()
        {
            this.Type = Item.Boom_Point;
            //确定弹射范围
            Position.X = X; Position.Y = Y;
            OriPosition = new Vector2D(Position.X, Position.Y);
            breakPosition = new Vector2D(Direction.X * breakLength + Position.X, Direction.Y * breakLength + Position.Y);
            //正常状态
            Texture texture = texturemanager.Get("Ef_etama2");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetHeight(16);
            sprite.SetWidth(16);
            sprite.SetUVs(6 * per_256, 8 * per_256, 8 * per_256, 10 * per_256);

            //顶部状态（三角形形态）
            Texture texture2 = texturemanager.Get("Ef_etama2");
            littleState = new Sprite();
            littleState.Texture = texture2;
            littleState.SetWidth(16);
            littleState.SetHeight(16);
            littleState.SetUVs(22 * per_256, 8 * per_256, 24 * per_256, 10 * per_256);
        }
        public override void Update(double elapsedTime)
        {
            base.Update(elapsedTime);
            double x = Datas.CurrentPlayer.Position.X;
            double y = Datas.CurrentPlayer.Position.Y;
            double dis = (Position.X - x) * (Position.X - x) + (Position.Y - y) * (Position.Y - y);
            if (dis < distance2)
            {
                Vector2D vct = new Vector2D(x - Position.X,
                       y - Position.Y);
                vct.Normalize();
                x = elapsedTime * vct.X * speed2;
                y = elapsedTime * vct.Y * speed2;
                x = Position.X + x;
                y = Position.Y + y;
                Position.X = x;
                Position.Y = y;
            }
        }

        public override void Render()
        {
            if (Position.Y < 400)
            {
                sprite.SetPosition(Position.X, Position.Y);
                Stage1State._renderer.DrawSprite(sprite);
            }
            else
            {
                sprite.SetPosition(Position.X, 400);
                Stage1State._renderer.DrawSprite(littleState);
            }
        }

        public override void Render(Renderer renderer)
        {
            if (Position.Y < 400)
            {
                sprite.SetPosition(Position.X, Position.Y);
                renderer.DrawSprite(sprite);
            }
            else
            {
                sprite.SetPosition(Position.X, 400);
                renderer.DrawSprite(littleState);
            }
        }
    }

    //HelpUp点
    public class HelpPoint : Item
    {
           float speed2 = 200;         //靠近玩家时自动靠近的速度
        float distance2 = 900;      //"是否靠近"玩家的判定距离，【以平方计算】

        //构造函数都需要继承基类构造函数
        public HelpPoint(TextureManager texturemanager, float X, float Y)
            : base()
        {
            this.Type = Item.Help_Point;
            //确定弹射范围
            Position.X = X; Position.Y = Y;
            OriPosition = new Vector2D(Position.X, Position.Y);
            breakPosition = new Vector2D(Direction.X * breakLength + Position.X, Direction.Y * breakLength + Position.Y);

            //正常状态
            Texture texture = texturemanager.Get("Ef_etama2");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetHeight(16);
            sprite.SetWidth(16);
            sprite.SetUVs(10*per_256, 8 * per_256, 12 * per_256, 10 * per_256);

            //顶部状态（三角形形态）
            Texture texture2 = texturemanager.Get("Ef_etama2");
            littleState = new Sprite();
            littleState.Texture = texture2;
            littleState.SetWidth(16);
            littleState.SetHeight(16);
            littleState.SetUVs(28 * per_256, 8 * per_256, 30 * per_256, 10 * per_256);
        }
        //构造函数都需要继承基类构造函数
        public HelpPoint(TextureManager texturemanager, double X, double Y)
            : base()
        {
            this.Type = Item.Help_Point;
            //确定弹射范围
            Position.X = X; Position.Y = Y;
            OriPosition = new Vector2D(Position.X, Position.Y);
            breakPosition = new Vector2D(Direction.X * breakLength + Position.X, Direction.Y * breakLength + Position.Y);
            //正常状态
            Texture texture = texturemanager.Get("Ef_etama2");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetHeight(16);
            sprite.SetWidth(16);
            sprite.SetUVs(10 * per_256, 8 * per_256, 12 * per_256, 10 * per_256);

            //顶部状态（三角形形态）
            Texture texture2 = texturemanager.Get("Ef_etama2");
            littleState = new Sprite();
            littleState.Texture = texture2;
            littleState.SetWidth(16);
            littleState.SetHeight(16);
            littleState.SetUVs(28 * per_256, 8 * per_256, 30 * per_256, 10 * per_256);
        }
        public override void Update(double elapsedTime)
        {
            base.Update(elapsedTime);
            double x = Datas.CurrentPlayer.Position.X;
            double y = Datas.CurrentPlayer.Position.Y;
            double dis = (Position.X - x) * (Position.X - x) + (Position.Y - y) * (Position.Y - y);
            if (dis < distance2)
            {
                Vector2D vct = new Vector2D(x - Position.X,
                       y - Position.Y);
                vct.Normalize();
                x = elapsedTime * vct.X * speed2;
                y = elapsedTime * vct.Y * speed2;
                x = Position.X + x;
                y = Position.Y + y;
                Position.X = x;
                Position.Y = y;
            }
        }

        public override void Render()
        {
            if (Position.Y < 400)
            {
                sprite.SetPosition(Position.X, Position.Y);
                Stage1State._renderer.DrawSprite(sprite);
            }
            else
            {
                sprite.SetPosition(Position.X, 400);
                Stage1State._renderer.DrawSprite(littleState);
            }
        }

        public override void Render(Renderer renderer)
        {
            if (Position.Y < 400)
            {
                sprite.SetPosition(Position.X, Position.Y);
                renderer.DrawSprite(sprite);
            }
            else
            {
                sprite.SetPosition(Position.X, 400);
                renderer.DrawSprite(littleState);
            }
        }
    }

    //擦弹点
    public class MissPoint : Item
    {
        float alpha = 1;         //显示时的透明度
        double time_caculate=0 ;    //计时
        float delayChange = 0.3f;   //完全颜色时所显示的时间长度
        float changeTime = 0.3f;    //消失所花的时间
        float G = 150;                     //重力加速度
        double speed_totle = 100;          //总速度
        double speed_right = 0;            //横向速度
        double speed_up = 100;             //纵向速度
        double Center_Distance = 15;       //离中心位置的距离
        double randDomain = 35;            //发射角随机域
        double randFireDomain = 15;        //发射时的x偏移随机域

        Vector2D direction = new Vector2D();         //运动方向

        public MissPoint(TextureManager texturemanager, double X, double Y,Vector2D dir,int score = 100)
        {
            //确定类型
            this.Type = Item.Miss_Point;
            working = true;

            //确定弹射范围
            Position.X = X; Position.Y = Y;
            if (dir.Y < 0)
                dir.Y = -dir.Y;
            if (dir.X >= 0)
            {
                double rad = dir.GetAngle();
                if (rad > 60)
                    rad = 60; 
                direction = new Vector2D(0, 1); 
                direction.rotate((float)(Datas.LooselyRandom.NextDouble() * (rad - 2*randDomain) + randDomain));
            }
            else
            {
                double rad = dir.GetAngle();
                rad = 360 - rad;
                if (rad > 60)
                    rad = 60;
                direction = new Vector2D(0, 1);
                direction.rotate(-((float)(Datas.LooselyRandom.NextDouble() * (rad - 2 * randDomain) + randDomain)));
            }

            speed_right = speed_totle * direction.X;
            speed_up = speed_totle * direction.Y;

            double offset_x = Center_Distance * direction.X;
            double offset_y = Center_Distance * direction.Y;
            Position.X += offset_x;
            Position.Y += offset_y;

            offset_x = Datas.LooselyRandom.NextDouble() * 2 *randFireDomain - randFireDomain;
            Position.X += offset_x;

            Texture texture = texturemanager.Get("Ef_etama2");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetHeight(16);
            sprite.SetWidth(16);
            sprite.SetUVs(14 * per_256, 8 * per_256, 16 * per_256, 10 * per_256);

            //保存了MissPoint的一些必要信息
            ItemData = new itemdata.MissPoint(score,3);
        }

        double percent = 0;
        double x,y;
        public override void Update(double elapsedTime)
        {
            time_caculate += elapsedTime;
            if (time_caculate > delayChange)
            {
                percent = (time_caculate - delayChange) / changeTime;
                if (percent > 1)
                    percent = 1;
                if (percent == 1)
                {
                    disabled = true;
                }
                alpha = 1 - (float)percent;
            }

            x = elapsedTime * speed_right;
            y = elapsedTime * speed_up;
            y = Position.Y + y;
            x = Position.X + x;
            Position.X = x;
            Position.Y = y;

            speed_up -= G * elapsedTime;

        }

        public override void Render()
        {
            Render(Stage1State._renderer);
        }

        public override void Render(Renderer renderer)
        {
            sprite.SetColor(new Color(1, 1, 1, alpha));
            sprite.SetPosition(Position.X, Position.Y);
            renderer.DrawSprite(sprite);
        }
        //不添加碰撞判定(基类函数里会自动将working置为false，因此这里不能继承)
        public override bool Collision(Collider c)
        {
            return false;
        }


    }

    //CatchPoint   收点（敌人死亡后，它的点全部成为收点并飞向玩家）
    public class CatchPoint 
    {

    }

    //仅表示一个分数【含有坐标以及透明值等属性】
    public class IScore 
    {
        public Vector2D Position;           //道具所对应的坐标
        public int Score=0;                 //道具所携带的分数信息
        public float alpha = 1;             //道具所带有的透明值
        float upspeed = 10;                 //向上位移的速度
        double delayTime = 1;               //消失时间（由一般颜色逐渐转为透明）
        double Time_caculate = 0;           //计时器
        public bool working = true;                //是否工作
        public bool disabled = false;              //是否应该被注销
        public IScore(double x, double y, int score)
        {
            Position = new Vector2D(x, y);
            Score = score;
            alpha = 1;
        }
        public void Update(double elapsedTime)
        {
            Time_caculate += elapsedTime;
            double percent = Time_caculate / delayTime;
            if (percent > 1)
            {
                percent = 1;
                working = false;
                disabled = true;                       //自动消失
            }
            alpha = 1 - (float)(percent*percent);
            double _y = elapsedTime * upspeed * alpha;
            _y = Position.Y + _y;
            Position.Y = _y;
        }
    }

    //ScoreItem    用于显示分数而使用的道具，  需要有数个数字的组装
    public class ScoreItem
    {
        
        public Score_Number score_number;
        
        public ScoreItem(TextureManager textureManager)
        {
            score_number = new Score_Number(textureManager);
        }
        
        /// <summary>
        /// 提供一个分数参数，将其画下来
        /// </summary>
        /// <param name="isc">Score参数</param>
        /// <param name="distance">数字间的距离</param>
        public void DrawNumber(Renderer _renderer, IScore isc , float distance)
        {
            float alpha = isc.alpha;
            Vector2D vct = new Vector2D(isc.Position.X, isc.Position.Y);
            string num = isc.Score.ToString();
            char[] arr = num.ToArray<char>();
            int length = arr.Length;
            int len2 = length / 2; 
            for (int i = 0; i < length; i++)
            {
                Sprite sp = score_number.GetNumber(arr[i].ToString());
                sp.SetPosition(vct.X + (i-len2) * distance, vct.Y);              //数字居中显示
                sp.SetColor(new Color(1, 1, 1, alpha*0.5f));
                _renderer.DrawSprite(sp);
            }
        }
       
    }

    //ScoreNumber  用于给ScoreItem提供需要的对应数字的Sprite（和texturefont类似）
    public class Score_Number
    {
        public const float  per_256 = 0.03125f;
        protected Dictionary<string, Sprite> NumberDic;
        TextureManager texturemanager;
        public Score_Number(TextureManager _t)
        {
            texturemanager = _t;
            NumberDic = new Dictionary<string, Sprite>();
            Texture texture = _t.Get("fontex");
            for (int i = 0; i < 10; i++)
            {
                Sprite sp = new Sprite();
                sp.Texture = texture;
                sp.SetWidth(8);
                sp.SetHeight(8);
                sp.SetUVs(i * per_256, 0, (i + 1) * per_256, per_256);
                NumberDic.Add(i.ToString(), sp);
            }
        }
        public Sprite GetNumber(string key)
        {
            return NumberDic[key];
        }
    }

}
