using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{

    //玩家类，圆形碰撞体
    public class Player :Entity,RoundCollider
    {
        public People CurrentCharactor;    //当前角色
        People fastCharactor;              //快速移动时候的角色
        People slowCharactor;              //慢速移动时候的角色
        int level = 0;                     //角色火力等级
        float radius = 2;                  //身体半径
        float renderSpeed;                 //动画的渲染速率
        float fast_speed;                  //快速的移动速度
        float slow_speed;                  //慢速的移动速度
        bool isfast = true;                //是否处于快速移动状态
        bool canmove = true;               //是否允许移动
        public List<Bullet> Bullets = new List<Bullet>();       //属于玩家的bullet集合
        public List<Bullet> bullet_toAdd = new List<Bullet>();  //子弹的添加列表
        public List<Bullet> bullet_toRemove = new List<Bullet>();     //子弹的删除列表 
        public bool hitted = false;        //是否被击中
        public bool unbeatable = false;    //是否处于无敌
        public bool ReLiving = false;      //是否处于重生状态中（重生状态中时道具自动飞向玩家的功能失效）
        double unbeatable_time = 0;        //处于无敌时间的计时器（大于0时处于无敌）
        Effect SlowMoveEffect;             //慢速位移的特效（需要先注册特效）
        public ParticleColiiders.ParticleCollider particle_collider;         //粒子的碰撞器（一般为弹射碰撞）

        public Player()
        {
            SlowMoveEffect = new NormalSlowEffect(Stage1State._textureManager, Position);
            particle_collider = new ParticleColiiders.HitOut_ParticleCollider(Position);
        }

        //是否处于快速移动状态
        public bool IsFast
        {
            get { return isfast; }
            set { 
                isfast = value;
                if (isfast)
                    CurrentCharactor = fastCharactor;
                else
                    CurrentCharactor = slowCharactor;
                this.fast_speed = CurrentCharactor.fastSpeed;
                this.slow_speed = CurrentCharactor.slowSpeed;
            }
        }
        
        public int Level
        {
            get { return level; }
            set 
            {
                level = value;
                fastCharactor.Level = value;         //将等级变动的信息传递给角色
                slowCharactor.Level = value;      
            }
        }
        public float RenderSpeed                        //渲染的人物动画更新速率
        {
            get { return renderSpeed; }
            set 
            {
                renderSpeed = value;
                CurrentCharactor.multiSprite.changeinterval = renderSpeed;
            }
        }
        //注册人物
        public void RegisteredRCharactors(People fc, People sc = null)
        {
            fastCharactor = fc;
            fast_speed = fc.fastSpeed;

            if (sc == null)
            {
                slowCharactor = fc;
                slow_speed = fc.slowSpeed;
            }
            else
            {
                slowCharactor = sc;
                slow_speed = sc.slowSpeed;
            }
            if (isfast)
                CurrentCharactor = fastCharactor;
            else
                CurrentCharactor = slowCharactor;
        }
        //注册慢速移动的光圈效果

        public override void Start()
        {
            base.Start();
            CurrentCharactor.Start();
        }

        public override void Render()
        {
            base.Render();
            //对人物进行绘画
            Sprite sp = CurrentCharactor.sprite;
            sp.SetPosition(Position.X, Position.Y);

            if (unbeatable)
            {
                sp.SetColorUp(new Color(1, 1, 1, 0.5f));
                sp.SetColorDown(new Color(1, 1, 1, 0.5f));
            }
            else
            {
                sp.SetColorUp(new Color(1, 1, 1, 1));
                sp.SetColorDown(new Color(1, 1, 1, 1));
            }
            Stage1State._renderer.DrawSprite(sp);

            if(!isfast)
               SlowMoveEffect.Render(Stage1State._renderer);            //执行慢速特效

        }

        double tt=0;
        public override void Update(double elapsedTime)
        {
            base.Update(elapsedTime);
            if(!isfast)
              SlowMoveEffect.Update(elapsedTime);

            #region Move
            //根据方向，改变其动画状态
            if (Direction.X < 0)        //向左移动
            {
                if (CurrentCharactor.multiSprite.State != 2 && CurrentCharactor.multiSprite.State != 1)
                {
                    CurrentCharactor.multiSprite.State = 1;
                }
            }
            else if (Direction.X > 0)  //向右移动
            {
                if (CurrentCharactor.multiSprite.State != 4 && CurrentCharactor.multiSprite.State != 3)
                {
                    CurrentCharactor.multiSprite.State = 3;
                }
            }
            else
            {
                if (CurrentCharactor.multiSprite.State != 0)
                {
                    CurrentCharactor.multiSprite.State = 0;
                }
            }
            if (!(Direction.X == 0 && Direction.Y == 0))
            {
                tt += elapsedTime;
                //System.Diagnostics.Debug.Print(tt.ToString());
                //System.Diagnostics.Debug.Print(" offset " + elapsedTime.ToString());
            }
            //    System.Diagnostics.Debug.Print("X: " + Position.X.ToString() + "Y: "+ Position.Y.ToString()
            //         + "  "+ DateTime.Now.ToString());
            float speed = 0;
            if (isfast)
                speed = fast_speed;
            else
                speed = slow_speed;
            double dx = (elapsedTime * Direction.X * speed);
            double dy = (elapsedTime * Direction.Y * speed);
            double tx = Position.X + dx;
            double ty = Position.Y + dy;
            if (tx > 180)
                tx = 180;
            if (tx < -180)
                tx = -180;
            if (ty < 25)
                ty = 25;
            if (ty > 425)
                ty = 425;
            if (canmove)
            {
                Position.X = tx;
                Position.Y = ty;
            }

            #endregion

            //       CurrentCharactor  Update
            CurrentCharactor.Update(elapsedTime);

            #region  AddBullets

            //  将人物序列中的载入子弹全部载入player子弹库中
            foreach (Bullet b in CurrentCharactor.bullets_toAdd)
            {
                bullet_toAdd.Add(b);
            }
            CurrentCharactor.bullets_toAdd.Clear();

            foreach (Bullet b in Bullets)
            {
                if (b.disabled)
                    bullet_toRemove.Add(b);
            }

            foreach (Bullet b in bullet_toRemove)
            {
                Bullets.Remove(b);
            }
            bullet_toRemove.Clear();


            #endregion

            #region UnBeatable
            //无敌
            if (unbeatable)
            {
                unbeatable_time -= elapsedTime;
                if (unbeatable_time < 0)
                {
                    unbeatable = false;
                    unbeatable_time = 0;
                    //解除被击中状态
                    hitted = false;
                }
            }
            #endregion

            //Fire
            if (fireing)
            {
                Fire(elapsedTime);
            }

        }

        bool fireing = false;
        public void StartFire()
        {
            fireing = true;
        }
        public void StopFire()
        {
            fireing = false;
            //重置发射刷新
            ReFreshFire();
        }

        public void Fire(double elapsedTime)
        {
            CurrentCharactor.Fire(Position,elapsedTime);
        }

        //发射时延重置
        public void ReFreshFire()
        {
            fastCharactor.RereshFire();
            slowCharactor.RereshFire();
        }

        //被击中所引发的事件
        public bool Hitted()
        {
            //无敌状态
            if (unbeatable)
            {
                hitted = false;
                return false;
            }
            hitted = true;
            unbeatable = true;
            unbeatable_time = 3;   
            System.Diagnostics.Debug.Print("Hited :" + Position.ToString());
            return true;
        }

        //返回当前的擦弹判定范围
        public float GetMissDistance()
        {
            return CurrentCharactor.MissDistance;
        }

        //发生了擦弹(随机向周围发出爆炸粒子)
        public void MissBullet(Bullet b)
        {

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

        public bool Collision(Collider c)
        {
            if (c is RoundCollider)
            {
                RoundCollider e = (RoundCollider)c;
                double x1 = e.GetX() , y1 = e.GetY() , r1 = e.GetRadius();
                if (((Position.X - x1) * (Position.X - x1) + (Position.Y - y1) * (Position.Y - y1))
                    < ((radius+r1) * (radius+r1)))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

    }

    //所有人物的基类
    public class People : IGameObject
    {
        public Sprite sprite;                    //人物所对应的核心sprite
        public const float per_512 = 0.015625f, per_256 = 0.03125f;       //用于将数据百分比化，从而便于制作渲染精灵
        protected  string name = "default";      //人物的名字（便于在众多对象中需找到指定对象）
        protected int level = 0;                 //人物的成长等级
        public MultiSprite multiSprite;          //控制人物动画的多纹理精灵控制装置
        public float attack = 10;                //人物本身的攻击力（暂且归纳为基础攻击力）
        public float fastSpeed = 350;            //快速飞行时的速度
        public float slowSpeed = 175;            //慢速飞行时候的速度
        public float fire_delay = 0.25f;         //发射的时延，（时延越低， 发射的速度越快）
        public double fire_delaytimer = 0;       //发射时延的计时器  （对发射延迟进行计时，为0时可以允许发射）
        protected bool allowfire = true;         //允许开火2
        public List<Bullet> bullets_toAdd = new List<Bullet>();       //释放子弹的列表(每帧由player类进行收集，然后统一处理)
        public float MissDistance = 10;          //擦弹的判定距离

        //姓名属性
        public string Name
        {
            get { return name; }
        }
        //角色等级属性
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        //每一个角色都会Fire,  （将其发送到bullet的加入队列，需要提供当前player的坐标位置）
        public virtual void Fire(Vector2D position , double elapsedTime)
        {
            
        }

        //刷新发射重置
        public virtual void RereshFire()
        {
        }

        //初始化组件
        public virtual void InitComponents()
        {

        }

        //作为GameObject的必须，在状态机切换时候调用，也可以主动调用
        public void Start()
        {
         
        }

        //作为GameObject的必须，在每帧切换的时候需要调用其执行逻辑更新
        public virtual void Update(double elapsedTime)
        {
            multiSprite.Update(elapsedTime);
            
        }

        //渲染方式， GameObject的实现必须，在每帧实现时必须调用其执行视图上的渲染
        public virtual void Render()
        {
            
        }

    }

    /// <summary>
    /// 计时（帧数）器
    /// </summary>
    public class TickTool
    {
      //当前记录的帧数
        public int currentTick; 
      //事件记录的总周期
        public int allTick;
        /// <summary>
        ///  提供一个总周期值的计时器 
        /// </summary>
        public TickTool(int _alltick,bool currentEqualToAll = true)
        {
            allTick = _alltick;
            if (currentEqualToAll)
                currentTick = allTick;
        }
    }

    //灵梦 角色
    public class ReiMu : People
    {
        //等级0时候的发射计时器0
        TickTool Level0_Fire0;
        TickTool Level0_Fire1;

        public ReiMu(TextureManager texturemanager)
        {
            name = "ReiMu";
            Texture texture = texturemanager.Get("Player2");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(32);
            sprite.SetHeight(48);
            sprite.SetColor(new Color(1, 1, 1, 0.2f));

            multiSprite = new MultiSprite(sprite);

            RecTangleF[] rects = new RecTangleF[4];
            for (int i = 0; i < 4; i++)
                rects[i] = new RecTangleF(0 + i * 4 * per_256, 0, 4 * per_256 + i * 4 * per_256, 6 * per_256);
            multiSprite.RegeditState(0, rects);      //将此rectangleF注册到0号state

            rects = new RecTangleF[3];
            for (int i = 0; i < 3; i++)
                rects[i] = new RecTangleF(i * 4 * per_256, 6 * per_256,
                    4 * per_256 + i * 4 * per_256, 12 * per_256);
            multiSprite.RegeditState(1, rects);      //将此rectangleF注册到1号state (过渡移动)

            rects = new RecTangleF[4];
            for (int i = 0; i < 4; i++)
                rects[i] = new RecTangleF(12 * per_256 + i * 4 * per_256, 6 * per_256,
                   12 * per_256 + 4 * per_256 + i * 4 * per_256, 12 * per_256);
            multiSprite.RegeditState(2, rects);      //将此rectangleF注册到2号state (移动)

            rects = new RecTangleF[3];
            for (int i = 0; i < 3; i++)
                rects[i] = new RecTangleF(4 * per_256 + i * 4 * per_256, 6 * per_256,
                   i * 4 * per_256, 12 * per_256);
            multiSprite.RegeditState(3, rects);      //将此rectangleF注册到3号state (过渡移动，右)

            rects = new RecTangleF[4];
            for (int i = 0; i < 4; i++)
                rects[i] = new RecTangleF(12 * per_256 + 4 * per_256 + i * 4 * per_256, 6 * per_256,
                12 * per_256 + i * 4 * per_256, 12 * per_256);
            multiSprite.RegeditState(4, rects);      //将此rectangleF注册到4号state (移动)

            multiSprite.RegeditCollection(1, 2);     //将1号动画连接到2号动画上
            multiSprite.RegeditCollection(3, 4);     //将3号动画连接到4号动画上
            multiSprite.State = 0;                   //将状态切换为3号state
            
            InitComponents();
        }

        public override void InitComponents()
        {
            Level0_Fire0 = new TickTool(6);    //Level0 ,每6帧发射一次直线弹
            Level0_Fire1 = new TickTool(12);    //Level0 ,每12帧发射一次跟踪弹*2
        }

        public override void RereshFire()
        {
            Level0_Fire0.currentTick = Level0_Fire0.allTick;
        }

        public override void Render()
        {
            base.Render();
        }

        public override void Update(double elapsedTime)
        {
            base.Update(elapsedTime);
            multiSprite.Update(elapsedTime);
        }

        public override void Fire(Vector2D positon , double elapsedTime)
        {
            Level0_Fire0.currentTick += 1;
            Level0_Fire1.currentTick += 1;
            switch (level)
            {
                case 0:                                      //等级0 时的发射表
                    if (Level0_Fire0.currentTick >= Level0_Fire0.allTick) 
                    {
                        Bullet b;
                        b = new ReimuBullet_Dir(Stage1State._textureManager, new Vector2D(0, 1));
                        b.Position.X = positon.X - 10;
                        b.Position.Y = positon.Y + 10;
                        bullets_toAdd.Add(b);
                        b = new ReimuBullet_Dir(Stage1State._textureManager, new Vector2D(0, 1));
                        b.Position.X = positon.X + 10;
                        b.Position.Y = positon.Y + 10;
                        bullets_toAdd.Add(b);
                        Level0_Fire0.currentTick = 0;         //刷新发射帧数计时器
                    }
                    if (Level0_Fire1.currentTick >= Level0_Fire1.allTick)
                    {
                        Bullet b = new ReimuBullet_Follow(Stage1State._textureManager, new Vector2D(-1, 3).GetNormalize());
                        b.Position.X = positon.X - 15;
                        b.Position.Y = positon.Y + 10;
                        bullets_toAdd.Add(b);
                        b = new ReimuBullet_Follow(Stage1State._textureManager, new Vector2D(1, 3).GetNormalize());
                        b.Position.X = positon.X + 15;
                        b.Position.Y = positon.Y + 10;
                        bullets_toAdd.Add(b);
                        Level0_Fire1.currentTick = 0;          //refresh
                    }


                    break;
            }
        }

    }

    //八云紫人物
    public class Yukari : People
    {

        public Yukari(TextureManager texturemanager)
        {
            name = "YuKaRi";
            Texture texture = texturemanager.Get("Player2");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(32);
            sprite.SetHeight(48);

            multiSprite = new MultiSprite(sprite);

            RecTangleF[] rects = new RecTangleF[4];
            for (int i = 0; i < 4; i++)
                rects[i] = new RecTangleF(4 * 4 * per_256 + i * 4 * per_256, 0,
                    4 * 4 * per_256 + 4 * per_256 + i * 4 * per_256, 6 * per_256);
            multiSprite.RegeditState(0, rects);      //将此rectangleF注册到0号state

            rects = new RecTangleF[3];
            for (int i = 0; i < 3; i++)
                rects[i] = new RecTangleF(i * 4 * per_256, 12 * per_256,
                    4 * per_256 + i * 4 * per_256, 18 * per_256);
            multiSprite.RegeditState(1, rects);      //将此rectangleF注册到1号state (过渡移动)

            rects = new RecTangleF[4];
            for (int i = 0; i < 4; i++)
                rects[i] = new RecTangleF(12 * per_256 + i * 4 * per_256, 12 * per_256,
                   12 * per_256 + 4 * per_256 + i * 4 * per_256, 18 * per_256);
            multiSprite.RegeditState(2, rects);      //将此rectangleF注册到2号state (移动)

            rects = new RecTangleF[3];
            for (int i = 0; i < 3; i++)
                rects[i] = new RecTangleF(4 * per_256 + i * 4 * per_256, 12 * per_256,
                   i * 4 * per_256, 18 * per_256);
            multiSprite.RegeditState(3, rects);      //将此rectangleF注册到3号state (过渡移动，右)

            rects = new RecTangleF[4];
            for (int i = 0; i < 4; i++)
                rects[i] = new RecTangleF(12 * per_256 + 4 * per_256 + i * 4 * per_256, 12 * per_256,
                12 * per_256 + i * 4 * per_256, 18 * per_256);
            multiSprite.RegeditState(4, rects);      //将此rectangleF注册到4号state (移动)

            multiSprite.RegeditCollection(1, 2);     //将1号动画连接到2号动画上
            multiSprite.RegeditCollection(3, 4);     //将3号动画连接到4号动画上
            multiSprite.State = 0;                   //将状态切换为0号state

        }

        public override void Render()
        {
            base.Render();
        }
        public override void Update(double elapsedTime)
        {
            base.Update(elapsedTime);
            multiSprite.Update(elapsedTime);
        }

        public override void Fire(Vector2D position , double elapsedTime)
        {
            
        }

    }



}
