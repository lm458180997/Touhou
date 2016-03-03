using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{

    //只沿直线发射的灵梦子弹
    public class ReimuBullet_Dir : Bullet
    {
        float radius = 5;
        bool disposing = false;                 //是否进入注销状态（过渡动画）
        int disposingTime = 20;      //消失的时间（帧数来计数）
        float angle;                 //消失的那段时间的旋转数值
        float _alpha;                //那段时间的透明值

        Sprite RedEsc;              //子弹消失时候的显示旋转红符
         

        public ReimuBullet_Dir(TextureManager texturemanager, Vector2D direction)
        {
            bulletbody = new RedCard(texturemanager);
            direction.Normalize();
            this.Direction.X = direction.X;
            this.Direction.Y = direction.Y;

            attack = 100;
            speed = 800;

            RedEsc = (new RedBox(texturemanager)).GetSprite();
          
        }

        public override void Render()
        {
            if (!disposing)
            {
                bulletbody.sprite.SetPosition(Position.X, Position.Y);
                Stage1State._renderer.DrawSprite(bulletbody.sprite, (float)Position.X, (float)Position.Y, 90);
            }
            else
            {
                RedEsc.SetPosition(Position.X, Position.Y);
                RedEsc.SetColor(new Color(1, 1, 1, _alpha*0.5f));
                Stage1State._renderer.DrawSprite(RedEsc, (float)Position.X, (float)Position.Y, angle+90);
            }
        
        
        }

        void move(double ElapsedTime)
        {
            double dx = ElapsedTime * Direction.X * speed;
            double dy = ElapsedTime * Direction.Y * speed;
            double tx = Position.X + dx;
            double ty = Position.Y + dy;
            Position.X = tx;
            Position.Y = ty;
        }

        public override void Update(double elapsedTime)
        {
            if (!working)
            {
                if (disposing)
                {
                    if (disposingTime > 0)
                    {
                        angle += (float)elapsedTime * 720;
                        _alpha = disposingTime / 12.0f;
                        speed = 25 * _alpha;
                        disposingTime--;          //注销计时-1
                    }
                    else
                    {
                        disposing = false;
                        disabled = true;          //在最后的旋转特效以后，同意注销此子弹
                    }
                    move(elapsedTime);
                }
                return;
            }

            move(elapsedTime);

            if (Position.X < -500 || Position.X > 500 || Position.Y < -200 || Position.Y > 500)
            {
                speed = 0;
                working = false;
                disabled = true;
            }
        }

        public override void Hit( Entity e)
        {
            Enemy _e = (Enemy)e;
            _e.Hitted(new AttackValue(attack));
        }

        public override bool Collision(Collider c)
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
                        disposing = true;           //进入销毁过渡期
                        speed = 50;
                        return true;
                    }
                }
            return false;
        }

    }

    //跟踪型的灵梦子弹
    public class ReimuBullet_Follow : Bullet
    {
        float radius = 5;                //碰撞判定的圆形半径判断值
        Enemy FollowEnemy = null;               //跟踪的Enemy对象
        float rotateSpeed = 580;                //旋转的角速度   radius/s 

        bool disposing = false;     //正处于将要消失的状态
        int disposingTime = 12;  //消失的时间（帧数来计数）
        float angle;                 //消失的那段时间的旋转数值
        float _alpha;                //那段时间的透明值

        Sprite BlueEsc;              //子弹消失时候的显示旋转蓝框
         
        public ReimuBullet_Follow(TextureManager texturemanager, Vector2D direction)
        {
            bulletbody = new BlueCard(texturemanager);
            direction.Normalize();
            this.Direction.X = direction.X;
            this.Direction.Y = direction.Y;
            attack = 100;
            speed = 400;

            BlueEsc = (new BlueBox(texturemanager)).GetSprite() ;
           

        }

        public override void Render()
        {
            if (working)
            {
                float ag = Direction.getcurve();
                bulletbody.sprite.SetPosition(Position.X, Position.Y);
                bulletbody.sprite.SetColor(new Color(1, 1, 1, 0.3f));
                Stage1State._renderer.DrawSprite(bulletbody.sprite, (float)Position.X, (float)Position.Y, ag + 90);
            }
            else if (disposing)
            {
                BlueEsc.SetWidth(32 * (1 * (1-_alpha) + 1));
                BlueEsc.SetHeight(32 * (1 * (1-_alpha) + 1));
                BlueEsc.SetColor(new Color(1, 1, 1, _alpha*0.6f));
                BlueEsc.SetPosition(Position.X, Position.Y);
                Stage1State._renderer.DrawSprite(BlueEsc, (float)Position.X, (float)Position.Y, angle + 90);
            }
        }

        public override void Update(double elapsedTime)
        {
            if (!working)
            {
                if (disposing)
                {
                    if (disposingTime > 0)
                    {
                        angle += (float)elapsedTime * 360 * 3;
                        _alpha = disposingTime / 12.0f;
                        disposingTime--;          //注销计时-1
                    }
                    else
                    {
                        disposing = false;
                        disabled = true;          //在最后的旋转特效以后，同意注销此子弹
                    }
                }
                return;
            }
                
            speed += 2;         //每帧速度+1
            if (Datas.CurrentEnemys.Count > 0)
            {
                double distance = 100000000;                            //取一个极端数值，使每一个求得值都会小于这个值
                double lastdistance = distance;
                foreach (Enemy e in Datas.CurrentEnemys)              //将离Player距离最短的作为跟踪对象
                {
                    if (e.Position.X < -200 || e.Position.X > 200 || e.Position.Y < -40 || e.Position.Y > 500)
                        continue;
                    distance = (Datas.CurrentPlayer.Position.X - e.Position.X) * (Datas.CurrentPlayer.Position.X - e.Position.X) +
                        (Datas.CurrentPlayer.Position.Y - e.Position.Y) * (Datas.CurrentPlayer.Position.Y - e.Position.Y);
                    if (distance < lastdistance)
                    {
                        FollowEnemy = e;
                    }
                    lastdistance = distance;
                }
            }
            if (Position.X < -190 || Position.X > 190 || Position.Y < -20 || Position.Y > 450)
            {
                speed = 0;
                working = false;
                disabled = true;
            }

            move(elapsedTime);
            //跟踪旋转
            if (FollowEnemy != null && !FollowEnemy.disabled && FollowEnemy.living)
            {
                Vector2D vct = new Vector2D(FollowEnemy.Position.X - Position.X,
                    FollowEnemy.Position.Y - Position.Y);
                if (!(vct.X == 0 && vct.Y == 0))
                {
                    double distance = vct.X * vct.X + vct.Y * vct.Y;
                    double percent = distance / 40000;
                    if (percent > 1)
                        percent = 1;
                    percent = 1 - percent;
                    float addangle = (float)percent * 1080;

                    vct.Normalize();
                    double vct_angle = vct.GetAngle();                 //目标的角度值
                    double dir_angle = Direction.GetAngle();           //当前方向的角度值
                    if (vct_angle < dir_angle)
                    {
                        if (dir_angle - vct_angle < 180)
                        {
                            Direction.rotate( (addangle + rotateSpeed) * (float)elapsedTime);
                            double d_2 = Direction.GetAngle();
                            if (d_2 > dir_angle)        //如果左旋转后反而比原本角度更大，则证明旋转过度
                            {
                                Direction.X = vct.X;
                                Direction.Y = vct.Y;
                            }
                            else if (d_2 < vct_angle)   //如果旋转后跑到方向角左边，则证明旋转过度
                            {
                                Direction.X = vct.X;
                                Direction.Y = vct.Y;
                            }
                        }
                        else
                        {
                            Direction.rotate((- addangle -rotateSpeed) * (float)elapsedTime);
                            double d_2 = Direction.GetAngle();
                            if (d_2 < dir_angle)         //如果旋转后反而比原本角度更小，则证明旋转过了一周
                            {
                                if (d_2 > vct_angle)
                                {
                                    Direction.X = vct.X;
                                    Direction.Y = vct.Y;
                                }
                            }
                        }
                    }
                    else if (vct_angle > dir_angle)
                    {
                        if (vct_angle - dir_angle < 180)
                        {
                            Direction.rotate((-addangle -rotateSpeed) * (float)elapsedTime);
                            double d_2 = Direction.GetAngle();
                            if (d_2 < dir_angle)
                            {
                                Direction.X = vct.X;
                                Direction.Y = vct.Y;
                            }
                            else if (d_2 > vct_angle)
                            {
                                Direction.X = vct.X;
                                Direction.Y = vct.Y;
                            }
                        }
                        else
                        {
                            Direction.rotate((addangle + rotateSpeed) * (float)elapsedTime);
                            double d_2 = Direction.GetAngle();
                            if (d_2 > dir_angle)
                            {
                                if (d_2 < vct_angle)
                                {
                                    Direction.X = vct.X;
                                    Direction.Y = vct.Y;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void move(double elapsedTime)
        {
            //移动
            double dx = elapsedTime * Direction.X * speed;
            double dy = elapsedTime * Direction.Y * speed;
            double tx = Position.X + dx;
            double ty = Position.Y + dy;
            Position.X = tx;
            Position.Y = ty;
        }

        public override void Hit( Entity e)
        {
            Enemy _e = (Enemy)e;
            _e.Hitted(new AttackValue(attack));
        }

        public override bool Collision(Collider c)
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
                        disposing = true;           //进入正在注销状态
                        return true;
                    }
                }
            return false;
        }
    }


    //  红色符卡， 形
    public class RedCard : BulletBody
    {
        public RedCard(TextureManager _t)
        {
            Texture texture = _t.Get("Player2");
            name = "RedCard";
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(72);
            sprite.SetHeight(16);
            sprite.SetUVs(2*per_256, 18 * per_256, 11 * per_256, 20 * per_256);
            sprite.SetColorUp(new Color(1,1,1,0.5f));
            sprite.SetColorDown(new Color(1, 1, 1, 0.5f));
        }
        public override void Update(double elapsedTime) { }
    }

    //  蓝色符卡，  形
    public class BlueCard : BulletBody
    {
        public BlueCard(TextureManager _t)
        {
            Texture texture;
            texture = _t.Get("Player2");
            name = "BlueCard";
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(16);
            sprite.SetHeight(16);
            sprite.SetUVs(4 * per_256, 22 * per_256, 6 * per_256, 24 * per_256);
            sprite.SetColorUp(new Color(1, 1, 1, 0.5f));
            sprite.SetColorDown(new Color(1, 1, 1, 0.5f));
        }
        public override void Update(double elapsedTime) { }
    }

    public class BlueBox : BulletBody
    {
        public BlueBox(TextureManager t)
        {
            name = "BlueBox";
            sprite= new Sprite();
             sprite.Texture = t.Get("Player2");
             sprite.SetHeight(32);
             sprite.SetWidth(32);
             sprite.SetUVs(0, 22 * per_256, 4 * per_256, 26 * per_256);
        }
        public Sprite GetSprite()
        {
            return sprite;
        }

    }

    public class RedBox : BulletBody
    {
        public RedBox(TextureManager t)
        {
            name = "BlueBox";
            sprite= new Sprite();
             sprite.Texture = t.Get("Player2");
             sprite.SetHeight(16);
             sprite.SetWidth(16);
             sprite.SetUVs(per_256,6,18,8,20);
        }
        public Sprite GetSprite()
        {
            return sprite;
        }

    }

}
