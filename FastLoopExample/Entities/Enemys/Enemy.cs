using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{

    //为了提高效率，渲染等级约定在0——3以内，否则迭代次数过大
    public class Enemy : Entity ,RenderLevel , Collider
    {
        protected int renderlevel = 0;                 //敌人具有渲染优先等级
        public string name = "default";          //敌人所具有的名字
        public const float per_512 = 0.015625f, per_256 = 0.03125f;
        public MultiSprite multiSprite;          //具有的动态纹理
        protected bool allowfire = true;         //允许开火
        protected float hp;                      //生命值
        public bool living = true;               //是否还活着
        public bool disabled = false;            //是否能够被完全消除
        public List<Bullet> Bullets = new List<Bullet>();             //属于敌人的bullet集合
        public List<Bullet> bullet_toAdd = new List<Bullet>();        //子弹的添加列表
        public List<Bullet> bullet_toRemove = new List<Bullet>();     //子弹的删除列表 
        public EnemyBody enemybody;              //敌人所填装的身体（纹理）

        public int getLevel()
        {
            return renderlevel;
        }

        public virtual void setLevel(int i)
        {
            renderlevel = i;
        }

        public virtual void RenderByLevel()
        {
            Render();
        }

        public override void Update(double elapsedTime)
        {

            foreach (Bullet b in Bullets)
            {
                if (b.disabled)
                    bullet_toRemove.Add(b);
            }
            //只管理删除自己的子弹，添加子弹以及子弹逻辑更新由 控制端 管理
            foreach (Bullet b in bullet_toRemove)
            {
                Bullets.Remove(b);
            }
            bullet_toRemove.Clear();

        }
        public virtual void Fire()
        {
        }
        public virtual void Hitted(AttackValue atkv)
        {
            float atk = atkv.attack;
            this.hp -= atk;
        }

        public virtual bool Collision(Collider c)
        {
            return false;
        }
    }

    //蝴蝶小精灵（enemy）
    public class RedButterflyEnemy : Enemy , RoundCollider
    {
        float radius = 20;
        float speed = 130;
        bool isleft = false;
        TextureManager texturemanager;

        public RedButterflyEnemy(TextureManager texturemanager ,float x, float y, bool il)
        {
            this.texturemanager = texturemanager;
            name = "RedButterflyEnemy";
            Position.X = x; Position.Y = y;
            enemybody = new RedButterfly(texturemanager,(float)Position.X,(float)Position.Y,speed);       //填装DrakBlueSprite的身体（纹理）数据
            enemybody.multiSprite.State = 0;                     //拨起0号动画
            enemybody.multiSprites[1].State = 0;                 //拨起身体的0号动画
            enemybody.multiSprites[1].changeinterval = 0.1f;
            enemybody.multiSprite.changeinterval = 0.1f;
            Direction = new Vector2D(0, -1);

            isleft = il;
            hp = 20000;       //20w血量
            renderlevel = 2;           //画在最顶层

            rotateTcset = new TCset(0.1f);
            fireTcset = new TCset(0.3f);
            TCsets = new List<TCset>();
            TCsets.Add(rotateTcset);
            TCsets.Add(fireTcset);
        }

        public override void Render()
        {
            base.Render();

            //画enemy的特效
            enemybody.effect.Render(Stage1State._renderer, (float)Position.X, (float)Position.Y);
            //画enemy的身体
            enemybody.Render(Stage1State._renderer);
            //画enemy的头部
            float ag = Direction.getcurve();
            enemybody.sprite.SetPosition(Position.X, Position.Y);
            Stage1State._renderer.DrawSprite(enemybody.sprite, (float)Position.X, (float)Position.Y, ag+90 );
            
        }

        bool needReupdate = false;
        double ReupdateTime = 0;

        TCset rotateTcset;         //旋转的计时处理器
        TCset fireTcset;           //开火的计时处理器
        List<TCset> TCsets;        //处理器的集合

        public override void Update(double elapsedTime)
        {
            base.Update(elapsedTime);
            enemybody.Update(elapsedTime,Position.X,Position.Y);

            double littleoffset = 1;
            bool have = false; 
            for (int i = 0; i < TCsets.Count; i++)
            {
                TCset st = TCsets[i];
                if (st.useable)
                {
                    st.AddTime(elapsedTime);
                    if (st.caculateTime  >= st.interval)
                    {
                        double offset = st.caculateTime - st.interval;
                        if (littleoffset > offset)
                        {
                            littleoffset = offset;         //找到最小的offset值
                        }
                        have = true;
                    }
                }
            }
            if (have)
            {
                for (int i = 0; i < TCsets.Count; i++)
                {
                    TCset cmd = TCsets[i];
                    if (cmd.useable)
                    {
                        if (cmd.caculateTime - cmd.interval == littleoffset)
                        {
                            if (i == 1)     //fire
                            {
                                if(living)
                                  Fire();
                                //System.Diagnostics.Debug.Print("cmd.caculate: "+ cmd.caculateTime.ToString()+
                                //    "  fire time: " + Stage1State.GameTime.ToString()
                                //    + " \nelapsedtime : "+ elapsedTime.ToString() 
                                //    + " offset :"+ littleoffset.ToString());
                               // System.Diagnostics.Debug.Print(Position.ToString());
                            }
                            if (i == 0)     //rotate
                            {
                                if (isleft)
                                    Direction.rotate(10);
                                else
                                    Direction.rotate(-10);
                            }
                            cmd.ClearTime();                                    //需要执行的任务就清空计时器
                        }
                        else
                            cmd.caculateTime -= (elapsedTime - littleoffset);   //不需执行的任务则需要返回应用的逻辑时间（减去后置误差值）    
                    }
                }
                needReupdate = true;
                ReupdateTime = littleoffset;
            }

            if (needReupdate)
            {
                needReupdate = false;
                Update(ReupdateTime);
            }
            else
                EntitiesUpdate(elapsedTime);

        }

        void EntitiesUpdate(double elapsedTime)
        {

            //血掉光后死亡
            if (hp <= 0 && living)
            {
                living = false;
                disabled = true;
                //死亡时掉落道具
                Datas.CurrentItemAdd.Add(new PowerPoint(texturemanager, Position.X, Position.Y));
                Datas.CurrentItemAdd.Add(new PowerPoint(texturemanager, Position.X, Position.Y));
                Datas.CurrentItemAdd.Add(new PowerPoint(texturemanager, Position.X, Position.Y));
                Datas.CurrentItemAdd.Add(new PowerPoint(texturemanager, Position.X, Position.Y));
                Datas.CurrentItemAdd.Add(new PowerPoint(texturemanager, Position.X, Position.Y));
                Datas.CurrentItemAdd.Add(new PowerPoint(texturemanager, Position.X, Position.Y));
                Datas.CurrentItemAdd.Add(new PowerPoint(texturemanager, Position.X, Position.Y));
                Bullets.Clear();              //清空子弹列表
            }
            double dx = elapsedTime * Direction.X * speed;
            double dy = elapsedTime * Direction.Y * speed;
            double tx = Position.X + dx;
            double ty = Position.Y + dy;
            Position.X = tx;
            Position.Y = ty;
        }

        public override void Fire()
        {
            if (!living)
                return;
            //Bullet b = new Blue_Ray(texturemanager,
            //    new Vector2D(Position.X, Position.Y - 10), new Vector2D(Direction.X, Direction.Y), isleft);
            Vector2D vct = new Vector2D(Datas.CurrentPlayer.Position.X - Position.X,
                       Datas.CurrentPlayer.Position.Y - Position.Y);

            Bullet b = new Direct_BlueKnife(texturemanager, vct);// new Vector2D(Direction.X, Direction.Y));
            b.Position.X = Position.X; b.Position.Y = Position.Y;
            bullet_toAdd.Add(b);

            if (needReupdate)
            {
                b.Update(ReupdateTime);           //有再更新的话需要单独对子弹也进行更新
            }

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

        public override bool Collision(Collider c)
        {
            return false;
        }

    }


    //敌人的身体（纹理）数据
    public  class EnemyBody
    {
        public Sprite sprite;                    //敌人图片所对应的核心sprite
        public Sprite[] sprites;                 //当身体需要多个sprite的时候使用的纹理序列
        public const float per_512 = 0.015625f, per_256 = 0.03125f;
        protected string name = "default";
        public MultiSprite multiSprite;
        public MultiSprite[] multiSprites;       //当身体需要多个动画精灵的时候需要使用
        public Effect effect;
        public string Name
        {
            get { return name; }
        }
        public virtual void Update(double elapsedTime)
        {
            multiSprite.Update(elapsedTime);
            if(effect!= null)
              effect.Update(elapsedTime);
        }
        public virtual void Update(double elapsedTime, double x, double y) { }
        public virtual void Render() { }
        public virtual void Render(Renderer renderer)
        {
            if (effect != null)
            effect.Render(renderer);
        }
        public virtual void Render(Renderer renderer, float x, float y)
        {

        }
    }

    //暗蓝色的精灵
    public class DrakBlueSprite : EnemyBody
    {
        public DrakBlueSprite(TextureManager texturemanager)
        {
            Texture texture = texturemanager.Get("enemys1");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(32);
            sprite.SetHeight(32);
            multiSprite = new MultiSprite(sprite);
            multiSprite.changeinterval = 0.15f;
            RecTangleF[] rects = new RecTangleF[2];
            for (int i = 0; i < 2; i++)
            {
                rects[i] = new RecTangleF(i * per_512 * 4, 0, (i + 1) * 4 * per_512, 4 * per_512);
            }
            multiSprite.RegeditState(0, rects);
        }

        public override void Update(double elapsedTime)
        {
            multiSprite.Update(elapsedTime);
        }

    }

    //红色蝴蝶
    public class RedButterfly : EnemyBody
    {
        Vector2D[] positions;                      //所有节点的坐标数据
        public float speed;                        //记录头部的速度（速度属性）
        float[] speeds;                            //分配在各个节点的速率列表
        int bodycount = 10;                        //切分的身体块数
        float max_distance = 0;                    //部位最大距离差距
        float max_percent = 0.1f;
        float min_distance = 0;                    //部位最小距离差距
        float min_percent = 0.07f;
        float divide_percent = 0.3f;               //速率差异的百分比

        public float Speed
        {
            get { return speed; }
            set 
            {
                speed = value;
                max_distance = speed * max_percent;
                min_distance = speed * min_percent;
                float percent = 0;
                for (int i = 0; i < bodycount; i++)
                {
                    percent = (float)i / (bodycount - 1);
                    speeds[i] = speed - percent * speed * divide_percent;
                }
            }
        }

        public RedButterfly(TextureManager texturemanager , float orix , float oriy, float speed =180)
        {
            this.speed = speed;
            max_distance = speed * max_percent;
            min_distance = speed * min_percent;
            sprites = new Sprite[bodycount];
            positions = new Vector2D[bodycount];
            multiSprites = new MultiSprite[2];
            speeds = new float[bodycount];
            Texture texture = texturemanager.Get("enemys1");
            float percent = 1;
            for (int i = 0; i < bodycount; i++)
            {
                percent = (float)i / (bodycount-1);
                speeds[i] = speed - percent * speed * divide_percent;
                 percent = 1 - percent; 
                percent *= percent;
                sprites[i] = new Sprite();
                sprites[i].Texture = texture;
                sprites[i].SetWidth(percent *32);
                sprites[i].SetHeight(percent * 32);
                sprites[i].SetUVs(0, 0, 0, 0);
                sprites[i].SetPosition(orix, oriy);
                positions[i] = new Vector2D(orix, oriy);
            }
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(32);
            sprite.SetHeight(32);
            sprite.SetPosition(orix, oriy);
            multiSprites[1] = new MultiSprite(sprites);        //身体（球）,导入整个数组进入
            multiSprites[1].changeinterval = 0.15f;
            multiSprites[0] = new MultiSprite(sprite);         //头部（蝴蝶）
            multiSprites[0].changeinterval = 0.05f;
            multiSprite = multiSprites[0];

            RecTangleF[] rects = new RecTangleF[4];
            for (int i = 0; i < 4; i++)
            {
                rects[i] = new RecTangleF(16*per_512+ i * per_512 * 4, 20*per_512,
                    16 * per_512 + (i + 1) * 4 * per_512, 24 * per_512);
            }
            multiSprites[0].RegeditState(0, rects);
            rects = new RecTangleF[4];
            for (int i = 0; i < 4; i++)
            {
                rects[i] = new RecTangleF(32 * per_512 + i * per_512 * 4, 20 * per_512,
                    32 * per_512 + (i + 1) * 4 * per_512, 24 * per_512);
            }
            multiSprites[1].RegeditState(0, rects);
            effect = new StartEffect_Red(Stage1State._textureManager);

        }

        double time_caculate = 0;

        public override void Update(double elapsedTime , double x,double y)
        {
            _x = x; _y = y;
            multiSprite.Update(elapsedTime);
            multiSprites[1].Update(elapsedTime);
            effect.Update(elapsedTime);

            time_caculate += elapsedTime;
            if (l_x != _x || l_y != _y)
            {
                //向前移动一步
                for (int i = positions.Length - 1; i > 0; i--)
                {
                    Vector2D vct = new Vector2D(positions[i].X - positions[i - 1].X,
                   positions[i].Y - positions[i - 1].Y);
                    if (vct.X != 0 && vct.Y != 0)
                    {
                        vct.Normalize();
                        float dis = Vector2D.distance(positions[i], positions[i - 1]);
                        //大于最大距离后加速
                        if (dis > max_distance)
                        {
                            speeds[i] = speeds[i - 1];
                        }
                        //小于最小距离后减速
                        else if (dis < min_distance)
                        {
                            float percent = 0;
                            percent = (float)i / (bodycount - 1);
                            speeds[i] = speed - percent * speed * divide_percent;
                        }
                        positions[i].X -= vct.X * speeds[i] * (float)elapsedTime;
                        positions[i].Y -= vct.Y * speeds[i] * (float)elapsedTime;
                    }

                }
                positions[0].X = _x;
                positions[0].Y = _y;
                l_x = _x; l_y = _y;
            }
        }

        double l_x = 0, l_y = 0;
        double _x, _y;
        public override void Render(Renderer renderer)
        {
            //x,y是头的坐标位置，（以下是画出身体）
            float ag = 0;
            for (int i = bodycount-1; i > 0; i--)
            {
                sprites[i].SetPosition(positions[i].X, positions[i].Y);
                //方向定义为指向前一个块
                Vector2D vct = new Vector2D(positions[i].X - positions[i - 1].X,
                    positions[i].Y - positions[i - 1].Y);
                //如果点没有发生变化，则跳过本次循环
                if (vct.X == 0 && vct.Y == 0)
                    continue;
                vct.Normalize();
                ag = vct.getcurve();
                renderer.DrawSprite(sprites[i], positions[i].X, positions[i].Y, ag + 90);
            }
            

        }

    }
    public class BlueButterfly : EnemyBody
    {
        public BlueButterfly(TextureManager texturemanager)
        {
            Texture texture = texturemanager.Get("enemys1");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(32);
            sprite.SetHeight(32);
            multiSprite = new MultiSprite(sprite);
            multiSprite.changeinterval = 0.15f;
            RecTangleF[] rects = new RecTangleF[4];
            for (int i = 0; i < 4; i++)
            {
                rects[i] = new RecTangleF(16 * per_512 + i * per_512 * 4, 16 * per_512,
                    16 * per_512 + (i + 1) * 4 * per_512, 20 * per_512);
            }
            multiSprite.RegeditState(0, rects);
            effect = new StartEffect_Red(Stage1State._textureManager);
        }

        public override void Update(double elapsedTime)
        {
            multiSprite.Update(elapsedTime);
            effect.Update(elapsedTime);
        }

    }

    //暗黑色的精灵皮肤
    public class DarkBlueLittleSprite : EnemyBody
    {
        public DarkBlueLittleSprite(TextureManager texturemanager)
        {
            name = "DarkBlueLittleSprite";
            Texture texture = texturemanager.Get("enemy");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(32);
            sprite.SetHeight(32);
            multiSprite = new MultiSprite(sprite);

            RecTangleF[] rects = new RecTangleF[4];          
            for (int i = 0; i < 4; i++)
                rects[i] = new RecTangleF(i * 4 * per_512 , 0,
                    (i + 1) * 4 * per_512, 4 * per_512);
            multiSprite.RegeditState(0, rects);      //将此rectangleF注册到0号state ,表示正面状态

            rects = new RecTangleF[4];
            for (int i = 0; i < 4; i++)
                rects[i] = new RecTangleF(i * 4 * per_512, 4 * per_512,
                    (i + 1) * 4 * per_512, 8 * per_512);
            multiSprite.RegeditState(1, rects);      //将此rectangleF注册到1号state ,表示右侧渐动的状态

            rects = new RecTangleF[4];
            for (int i = 0; i < 4; i++)
                rects[i] = new RecTangleF(i * 4 * per_512, 8 * per_512,
                    (i + 1) * 4 * per_512, 12 * per_512);
            multiSprite.RegeditState(2, rects);      //将此rectangleF注册到2号state ,表示右侧移动状态

            rects = new RecTangleF[4];
            for (int i = 0; i < 4; i++)
                rects[i] = new RecTangleF((i + 1) * 4 * per_512, 4 * per_512,
                    i * 4 * per_512, 8 * per_512);
            multiSprite.RegeditState(3, rects);      //将此rectangleF注册到3号state ,表示左侧渐动的状态

            rects = new RecTangleF[4];
            for (int i = 0; i < 4; i++)
                rects[i] = new RecTangleF((i + 1) * 4 * per_512, 8 * per_512,
                    i * 4 * per_512, 12 * per_512);
            multiSprite.RegeditState(4, rects);      //将此rectangleF注册到4号state ,表示左侧移动状态
            multiSprite.RegeditCollection(1, 2);     //将1号动画连接到2号动画上
            multiSprite.RegeditCollection(3, 4);     //将3号动画连接到4号动画上
            multiSprite.State = 0;                   //将状态切换为0号state
            multiSprite.changeinterval = 0.1f;
        }

        public override void Update(double elapsedTime)
        {
            multiSprite.Update(elapsedTime);
        }

    }






}
