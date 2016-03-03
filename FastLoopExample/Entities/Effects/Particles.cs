using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Particle
{
    /// <summary>
    /// 粒子（总）特效基类(控制生成以及总体逻辑)
    /// </summary>
    public class Particles
    {
        public Random random;      //生成粒子所需要的随机数生成器
        public bool working = true;       //是否正常工作
        public bool AllowCreateParticle = true;    //是否允许生成粒子
        public bool Enabled                 //Enabled属性（是否能正常启用粒子）
        { get { return AllowCreateParticle; } set { AllowCreateParticle = value;} }
        
        public List<particle> particles_toAdd = new List<particle>();     //粒子的添加
        public List<particle> particles= new List<particle>();           //粒子列表
        public List<particle> particles_toremove = new List<particle>();  //粒子的删除列表
        protected int CreateInterval = 5;         //每多少帧生成一次粒子
        public void StopCreate()
        {
            AllowCreateParticle = false;
        }

        public void Update(double elapsedTime)
        {
            foreach (particle p in particles_toAdd)
            {
                particles.Add(p);
            }
            particles_toAdd.Clear();
            update(elapsedTime);
            foreach (particle p in particles_toremove)
            {
                particles.Remove(p);
            }
            particles_toremove.Clear();
        }

        //可允许修改的子逻辑更新
        protected virtual void update(double elapsedTime)
        {
            foreach (particle p in particles)
            {
                p.Update(elapsedTime);
                if (p.disable)
                    particles_toremove.Add(p);
            }
        }
        //渲染
        public virtual void Render(Renderer renderder)
        {

        }
        public virtual void Render()
        {

        }
    }

    /// <summary>
    /// 飞舞的花瓣粒子效果
    /// </summary>
    public class FlyingFlowersParticles : Particles
    {
        int TimeTick;               //总时间计时器
        bool[][] particlesArr;      //粒子对应二维数组（防止过于密集或过余松散，未使用）
        TextureManager texturemanager;  //纹理管理器

        public FlyingFlowersParticles(TextureManager _t)
        {
            random = new Random();
            texturemanager = _t;
        }

        public void AddParticle()
        {
            double x = random.Next(30) * 10 + random.NextDouble() * 10 - 155;
            double y = random.Next(45) * 10 + random.NextDouble() * 10 ;
            particle p = new FlyingFlower(texturemanager, random, 3, new Vector2D(x, y),8);
            particles_toAdd.Add(p);
        }
        
        protected override void update(double elapsedTime)
        {
            if (!working) 
                return;
            TimeTick++;
            if (TimeTick % CreateInterval == 0)
            {
                if (AllowCreateParticle)
                {
                    AddParticle();
                }
            }

            foreach (particle p in particles)
            {
                p.Update(elapsedTime);
                if (p.disable)
                    particles_toremove.Add(p);
            }

        }

        public override void Render(Renderer renderer)
        {
            foreach (particle p in particles)
            {
                p.Render(renderer);
            }
        }


    }

    /// <summary>
    /// 单个粒子特效的基类
    /// </summary>
    public class particle : Entity
    {
        public ParticleBody Body;
        public bool working = true;       //是否正常工作
        public bool disable;       //是否可以注销
        public virtual void Render(Renderer renderer){}
    }

    /// <summary>
    /// 飞舞的花瓣，实现花瓣飞舞效果
    /// </summary>
    public class FlyingFlower :particle
    {
        int TimeTick = 0;           //计时器
        int lifeTick = 360;         //生存周期
        float percent;              //记录生存周期的百分比

        Random random;              //随机数生成器

        float MaxSpeed = 15;            //最大速度
        float UpSpeed = 0;              //垂直方向上的速度
        double  Upmax = 0;              //垂直方向上的最大速度
        float RightSpeed = 0;           //水平方向上的速度
        double Rightmax = 0;            //水平方向上的最大速度
        Vector2D ForceSpeedDirection = new Vector2D(0,0);   //随机产生的施加力的方向(模为0或1)
        float AddSpeed = 10;            // 施加力的加速度
        float AddSpeedMin = 0, AddSpeedMax = 10;  //力的加速度的最小值和最大值
        int ChangeforceInterval = 120;  //每120帧产生一种随机力
        /// <summary>
        /// 生成一个花瓣粒子
        /// </summary>
        /// <param name="_t">纹理器</param>
        /// <param name="random">随机器</param>
        /// <param name="color">颜色选择（现已无用，任意填写）</param>
        /// <param name="position">坐标</param>
        /// <param name="w_h">宽度以及高度（宽高相等）</param>
        public FlyingFlower(TextureManager _t, Random random , int color ,Vector2D position , float w_h = 16)
        {
            this.random = random;
            int select = random.Next(4);
            Body = new FLowerBody(_t,w_h);
            Position.X = position.X; Position.Y = position.Y;
            //生成随机数据
            CreteRandomData();
        }
        void CreteRandomData()
        {
            //先随机定义一个力,和一个介于最大值与最小值之间的随机加速度
            ForceSpeedDirection = new Vector2D(random.NextDouble() * 2 - 1, random.NextDouble() * 2 - 0.7).GetNormalize();
            Upmax = ForceSpeedDirection.Y * MaxSpeed;
            Rightmax = ForceSpeedDirection.X * MaxSpeed;
            AddSpeed = (float)random.NextDouble() * (AddSpeedMax - AddSpeedMin) + AddSpeedMin;
            if (Upmax < 0)
                Upmax = -Upmax;
            if (Rightmax < 0)
                Rightmax = -Rightmax;
        }
        public override void Render(Renderer renderer)
        {
            Body.body.SetWidth(4 * percent + 4);
            Body.body.SetHeight(4 * percent + 4);
            Body.body.SetColor(new Color(1, 1, 1, percent * 0.3f+0.1f));
            Body.body.SetPosition(Position.X,Position.Y);
            renderer.DrawSprite(Body.body);
        }

        public override void Update(double elapsedTime)
        {
            if (!working)
                return;
            TimeTick++;
            UpSpeed += (float)(ForceSpeedDirection.Y * AddSpeed * elapsedTime);
            RightSpeed += (float)(ForceSpeedDirection.X * AddSpeed*elapsedTime);
            
            if (UpSpeed > Upmax)
                UpSpeed = (float)Upmax;
            if (UpSpeed < -Upmax)
                UpSpeed = (float)(- Upmax);
            if (RightSpeed > Rightmax)
                RightSpeed = (float)Rightmax;
            if (RightSpeed < -Rightmax)
                RightSpeed = (float)(-Rightmax);
            double dx = elapsedTime * RightSpeed;
            double dy = elapsedTime * UpSpeed;
            double tx = Position.X + dx;
            double ty = Position.Y + dy;
            Position.X = tx;
            Position.Y = ty;
            if (TimeTick % ChangeforceInterval == 0)
            {
                CreteRandomData();               //生成随机数据
            }

            if (TimeTick > lifeTick)
            {
                working = false;
                disable = true;
            }
            else
            {
                int halftick = lifeTick / 2;
                if (TimeTick <= halftick)
                    percent = TimeTick / (float)halftick;
                else
                    percent = (lifeTick - TimeTick) / (float)halftick;
            }

        }

    }

    /// <summary>
    /// 爆炸【溅射】点的粒子效果 
    /// </summary>
    public class BreakPointsParticles : Particles
    {
         int TimeTick;               //总时间计时器
        TextureManager texturemanager;  //纹理管理器
        int color;                      //生成爆炸点的颜色
        float visible_lenth = 10;       //超过多久以后为可视范围
        public bool Usevisible_lenth= true;    //是否使用可视遮挡

        public BreakPointsParticles(TextureManager _t, int Color = 1)
        {
            random = new Random();
            texturemanager = _t;
            this.color = Color;
        }

        /// <summary>
        /// 生成粒子，提供坐标，以及产生概率
        /// </summary>
        /// <param name="position">产生的粒子的坐标</param>
        /// <param name="chance">产生的粒子的概率</param>
        public void AddParticle(Vector2D position,float chance = 0.3f)
        {
            double result = random.NextDouble();
            if (result < chance)
            {
                if (Usevisible_lenth)
                {
                    particle p = new BreakPoint(texturemanager, random, color, position, visible_lenth);
                    particles_toAdd.Add(p);
                }
                else
                {
                    particle p = new BreakPoint(texturemanager, random, color, position);
                    particles_toAdd.Add(p);
                }
            }
        }

        public void SelectColor(int c)
        {
            color = c;
            if (color > 3) color = 3;
            if (color < 0) color = 0;
        }
        public void EnableVisibleLenth(bool enable, float value)
        {
            Usevisible_lenth = enable;
            visible_lenth = value;
        }
        
        protected override void update(double elapsedTime)
        {
            if (!working) 
                return;
            TimeTick++;
            foreach (particle p in particles)
            {
                p.Update(elapsedTime);
                if (p.disable)
                    particles_toremove.Add(p);
            }

        }

        public override void Render(Renderer renderer)
        {
            foreach (particle p in particles)
            {
                p.Render(renderer);
            }
        }
    }

    /// <summary>
    /// 爆炸点（实现爆炸溅射效果）
    /// </summary>
    public class BreakPoint : particle
    {
        float visible_lenth = 10;    //运行多长距离后能够显形
        double lenth = 0;             //已经飞行的距离
        Random random;            //随机数生成器
        float percent;            //显示大小的百分比
        float speed;              //速度
        float speedMax = 250;     //随机能产生的最大速度
        float speedMin = 100;      //最低速度
        float width = 6;          //宽
        float height = 12;         //高

        int Tick = 0;             //计时器
        int TotolTick = 20;       //总的计时周期
        int TotolTickMin = 10;    //最小总计时周期
        int TotolTickMax = 30;    //最大总计时周期

        /// <summary>
        /// 生成一个爆炸粒子
        /// </summary>
        /// <param name="_t">纹理器</param>
        /// <param name="random">随机器</param>
        /// <param name="color">颜色选择（现已无用，任意填写）</param>
        /// <param name="position">坐标</param>
        public BreakPoint(TextureManager _t, Random random, int color, Vector2D position, float visibleLenth = 10)
        {
            this.random = random;
            int select = random.Next(4);
            Body = new BreakPointBody(_t, color, select);
            Position.X = position.X; Position.Y = position.Y;           //坐标

            float ag = (float)random.NextDouble() * 360;
            Vector2D vct = new Vector2D(0,1);
            vct.rotate(ag);
            Direction.X = vct.X; Direction.Y = vct.Y;                   //生成一个随机方向

            visible_lenth = visibleLenth;
            //生成随机数据
            CreteRandomData();

        }

        //生成随机大小以及随机速度
        void CreteRandomData()
        {
            float rd = (float)(random.NextDouble());
            speed = speedMin + rd * (speedMax - speedMin);
            rd = (float)(random.NextDouble());
            TotolTick = TotolTickMin + (int)(rd * (TotolTickMax - TotolTickMin));
            //宽度随机域为 +- 4 ， 高度随机域为 +- 2
            width = (float)random.NextDouble() * 8 - 4 + width;
            height = (float)random.NextDouble() * 4 - 2 + height;

        }

        public override void Render(Renderer renderer)
        {
            if (lenth < visible_lenth)         //如果小于可视距离则不渲染
                return;
            float ag = Direction.getcurve();   //获取旋转值
            float p = 1 - percent;
            float _w = width / 2;
            float _h = height / 2;
            Body.body.SetWidth(_w * p + _w);
            Body.body.SetHeight(_h * p + _h);
            Body.body.SetColor(new Color(1, 1, 1, p * 0.3f + 0.4f));
            Body.body.SetPosition(Position.X,Position.Y);
            renderer.DrawSprite(Body.body,Position.X,Position.Y,ag);
        }

        public override void Update(double elapsedTime)
        {
            Tick++;
            percent = ((float)Tick) / TotolTick;
            if (percent > 1)
            {
                percent = 1;
                working = false;
                disable = true;
            }

            double offset = speed * elapsedTime * ((1-percent)*0.5+0.5);
            lenth += offset;
            double tx = Position.X + offset * Direction.X;
            double ty = Position.Y + offset * Direction.Y;
            Position.X = tx;
            Position.Y = ty;
          
        }

    }

    /// <summary>
    /// 单个粒子的纹理贴图
    /// </summary>
    public class ParticleBody
    {
        public Sprite body;        //贴图所对应的Sprite
        public const float per_256 = 0.03125f;
    }

    //爆炸点【溅射效果】
    public class BreakPointBody : ParticleBody
    {
        public BreakPointBody(TextureManager _t, int color, int select)
        {
            float x = color * 8 * per_256 + select * 2 * per_256;
            Texture texture = _t.Get("Ef_etama2");
            body = new Sprite();
            body.Texture = texture;
            body.SetWidth(8);
            body.SetHeight(8);
            float leftop_x = color * 8 * per_256 + select * 2 * per_256;
            body.SetUVs(leftop_x, 30 * per_256,
                leftop_x + 2 * per_256, 32 * per_256);
            body.SetColor(new Color(1, 1, 1, 0.5f));
        }
    }

    //飞舞的花瓣
    public class FLowerBody : ParticleBody
    {
        public FLowerBody(TextureManager _t,float w_h = 16)
        {
            Texture texture = _t.Get("Ef_etama2");
            body = new Sprite();
            body.Texture = texture;
            body.SetWidth(w_h);
            body.SetHeight(w_h);
            body.SetUVs(2 * per_256, 12 * per_256, 4 * per_256, 14 * per_256);
            body.SetColor(new Color(1, 1, 1, 1));
        }



    }




}
