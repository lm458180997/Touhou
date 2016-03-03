using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{

    public class multiVector
    {
        public Vector2D MainVector;       // 主矢量
        public int count = 1;             // 子矢量数量
        public Vector2D[] childvectors;   // 子矢量数组
        public multiVector() { }
    }
    /// <summary>
    /// 等角度间距复数矢量组合
    /// </summary>
    public class EqualAngleMultiVctor : multiVector
    {
        public float angle;
        public EqualAngleMultiVctor(Vector2D _mainvector, int _count, float _angle)
        {
            if (_count % 2 != 0)
                _count++;
            this.angle = _angle;
            this.count = _count;
            MainVector = _mainvector;        //设定主矢量
            childvectors = new Vector2D[count];
            Vector2D vct = new Vector2D(_mainvector.X, _mainvector.Y);
            vct.rotate(- count/2 *angle);
            for (int i = 0; i < count; i++)
            {
                childvectors[i] = new Vector2D(vct.X, vct.Y);
                vct.rotate(angle);
            }
        }
        public void refresh()
        {
            Vector2D vct = new Vector2D(MainVector.X, MainVector.Y);
            vct.rotate(-count / 2 * angle);
            for (int i = 0; i < count; i++)
            {
                childvectors[i] = new Vector2D(vct.X, vct.Y);
                vct.rotate(angle);
            }
        }
    }

    //Stage01_E01
    public class Stage01_E01 : Enemy ,RoundCollider
    {
        
        EnemyData enemydata;             //敌人的数据
        Controler controler;             //行为控制器
        float radius = 20;               //判定半径
        TextureManager texturemanager;   //纹理管理器
        List<TCset> commands= new List<TCset>();            //命令列表
        int time_caculate = 0;                              //计时器（帧数）
        bool turnleft;                                      //是否向左转
        EqualAngleMultiVctor fireDirection;                 //指向敌人方向的一个矢量组合
        Vector2D vct;                                       //用于存放指向敌人的主方向
        bool hited = false;           //是否被击中

        BreakOut_Blue Break_ani;      //被击破后的动画
        Particle.BreakPointsParticles breakParticle;      //击破后的溅射粒子

        public Stage01_E01(TextureManager _texturemanager, float x, float y,bool _turnleft = false)
        {
            name = "Stage01_E01";
            texturemanager = _texturemanager;
            Position.X = x; Position.Y = y;
            enemybody = new DarkBlueLittleSprite(texturemanager);   //皮肤选择为DarkBlueLittleSprite
            enemybody.multiSprite.State = 0;                        //一开始先重置为一般状态
            hp = 1500;
            enemydata = new EnemyData(Position, Direction, Scale, 100);    //建立一个敌人数据，用于传递给行为控制器使用
            turnleft = _turnleft;
            controler = new Stage01_E01c(enemydata,turnleft);                        //建立一个专属的行为控制器
            renderlevel = 0;                                 //在敌人列表中置于最底层

            Break_ani = new BreakOut_Blue(_texturemanager, Position,20,16,64);       //爆炸动画
            breakParticle = new Particle.BreakPointsParticles(_texturemanager, 0);   //溅射粒子
            breakParticle.EnableVisibleLenth(true, 5);

            TCset fire01 = new TCset(50, 250);              //200-500帧第一阶段释放子弹
            fire01.Name = "fire01";
            commands.Add(fire01);                            //添加进任务队列
            TCset fire02 = new TCset(250, 400);              //500-580帧时进行旋转，并且进行加速 
            fire02.Name = "fire02";
            commands.Add(fire02);
            TCset stop01 = new TCset(320, 340);              //中间过程停止发射(stop要置于fire等命令的后面)
            stop01.Name = "stop01";
            commands.Add(stop01);

        }
        void playcommands()
        {
            time_caculate++;
            foreach (TCset tcs in commands)
            {
                if (tcs.interval == time_caculate)
                {
                    tcs.useable = true;
                }
                if (tcs.useable)
                {
                    if (tcs.Name == "fire01")
                    {
                        tcs.TickRun();         //所有计时周期进一格
                        if (tcs.Tick01 == 30)
                        {
                            Fire(new Vector2D(0, -1), Position);
                            tcs.Tick01 = 0;    //(Tick01专门控制定周期发射子弹)
                        }
                    }
                    else if (tcs.Name == "fire02")
                    {
                        tcs.TickRun();         //所有计时周期进一格
                        if (tcs.Tick01 == 10)
                        {
                            Fire(fireDirection.MainVector, 
                                Position);
                            foreach (Vector2D v in fireDirection.childvectors)
                            {
                                Fire(v,
                                Position);
                            }
                            tcs.Tick01 = 0;
                        }
                        if (tcs.Tick02 == 1)      //每40帧锁定一次主角方向
                        {
                            vct = new Vector2D(Datas.CurrentPlayer.Position.X - Position.X,
                            Datas.CurrentPlayer.Position.Y - Position.Y).GetNormalize();
                            fireDirection = new EqualAngleMultiVctor(vct, 4, 20);
                        }
                        else if (tcs.Tick02 == 50)
                            tcs.Tick02 = 0;
                    }
                    else if (tcs.Name == "stop01")
                    {
                        bullet_toAdd.Clear();
                    }
                }
                if (tcs.caculateTime == time_caculate)
                {
                    tcs.useable = false;
                }
            }
        }

        public override bool Collision(Collider c)
        {
            return base.Collision(c);
        }

        public override void Fire()
        {
            if (!living)
                return;
            Bullet b = new Stage01_EB01(texturemanager,new Vector2D(0,-1) , Position);
            b.Position.X = Position.X; b.Position.Y = Position.Y;
            bullet_toAdd.Add(b);
        }
        public void Fire(Vector2D dir, Vector2D pos , int color = 0)
        {
            if (!living)
                return;
            Bullet b = new Stage01_EB01(texturemanager, dir, pos , color);
            b.Position.X = Position.X; b.Position.Y = Position.Y;
            bullet_toAdd.Add(b);
        }

        public override void Render()
        {
            if (living)
            {
                if (hited)
                {
                    enemybody.sprite.SetColor(new Color(1, 0.7f, 0.7f, 1));
                    hited = false;
                }
                else
                    enemybody.sprite.SetColor(new Color(1, 1, 1, 1));
                enemybody.sprite.SetPosition(Position.X, Position.Y);
                Stage1State._renderer.DrawSprite(enemybody.sprite);
            }
            else
            {
                Break_ani.Render(Stage1State._renderer);
                breakParticle.Render(Stage1State._renderer);
            }
        }

        public override void Hitted(AttackValue atkv)
        {
            this.hp -= atkv.attack;
            hited = true;               //被击中
        } 

        public override void Update(double elapsedTime)
        {
            base.Update(elapsedTime);
            enemybody.Update(elapsedTime);                         //皮肤逻辑更新
            if (living)                                            //是否存活
            {
                controler.Update(elapsedTime);                     //行为控制器逻辑更新
                playcommands();                                    //执行命令队列的命令
            }
            else
            {
                breakParticle.Update(elapsedTime);         //粒子需要一直保持更新
                if ((!Break_ani.work_over))                //工作是否完成
                {
                    Break_ani.Update(elapsedTime);
                }
                else
                {
                    Break_ani.working = false;           //停止工作
                    if(breakParticle.particles.Count==0)
                       disabled = true;            //粒子消失完后销毁
                }
            }
            //血掉光后死亡
            if (hp <= 0 && living)
            {
                living = false;
                Break_ani.working = true;                      //打开爆炸动画
                //生成溅射粒子，形成粒子效果
                for (int i = 0; i < 20; i++)
                {
                    breakParticle.AddParticle(Position, 1);    //生成概率为1
                }
                int count = (int)(Datas.GameRandom.NextDouble() * 2) + 1;
                int score = (int)(Datas.GameRandom.NextDouble() * 20000 + 100000);
                //死亡时掉落道具
                for (int i = 0; i < count; i++)
                {
                    Datas.CurrentItemAdd.Add(new PowerPoint(texturemanager, Position.X, Position.Y));
                }
                Datas.CurrentItemAdd.Add(new GrazePoint(texturemanager, Position.X, Position.Y, score));
                //Datas.CurrentItemAdd.Add(new GratePowerPoint(texturemanager, Position.X, Position.Y));
                //Datas.CurrentItemAdd.Add(new BoomPoint(texturemanager, Position.X, Position.Y));
                //Datas.CurrentItemAdd.Add(new HelpPoint(texturemanager, Position.X, Position.Y));
            }

            int currentstate = enemybody.multiSprite.State;
            if (Direction.X < -0.2)
            {
                if (currentstate != 3 && currentstate != 4)                //切换为向左移动动画
                {
                    enemybody.multiSprite.State = 3;
                }
            }
            else if (Direction.X < 0.2)                            //切换为正面状态
            {
                if (currentstate != 0)
                {
                    enemybody.multiSprite.State = 0;
                }
            }
            else                                                   //切换为向右移动动画
            {
                if (currentstate != 1 && currentstate != 2)
                {
                    enemybody.multiSprite.State = 1;
                }
            }

            //越界则销毁此对象
            if (Position.X < -210 || Position.X > 210 || Position.Y < -30)
            {
                living = false;
                disabled = true;
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
    }

    namespace Stage1
    {

        public class Enemy01 : Enemy, RoundCollider
        {
            EnemyData enemydata;             //敌人的数据
            Controler controler;             //行为控制器
            float radius = 20;               //判定半径
            TextureManager texturemanager;   //纹理管理器
            List<TCset> commands = new List<TCset>();           //命令列表
            int time_caculate = 0;                              //计时器（帧数）
            bool turnleft;                                      //是否向左转
            Vector2D vct = new Vector2D(0,1);                                       //用于存放指向敌人的主方向
            bool hited = false;           //是否被击中

            BreakOut_Blue Break_ani;      //被击破后的动画
            Particle.BreakPointsParticles breakParticle;      //击破后的溅射粒子

            public Enemy01(TextureManager _texturemanager, float x, float y, bool _turnleft = false)
            {
                name = "Stage1Enemy01";
                texturemanager = _texturemanager;
                Position.X = x; Position.Y = y;
                enemybody = new DarkBlueLittleSprite(texturemanager);   //皮肤选择为DarkBlueLittleSprite
                enemybody.multiSprite.State = 0;                        //一开始先重置为一般状态
                hp = 500;
                enemydata = new EnemyData(Position, Direction, Scale, 100);    //建立一个敌人数据，用于传递给行为控制器使用
                turnleft = _turnleft;

                /////添加控制器处//////
                controler = new Stage1.Controler01(enemydata, turnleft);       //建立一个专属的行为控制器
                renderlevel = 0;                                 //在敌人列表中置于最底层
                /////添加动画爆炸处////
                Break_ani = new BreakOut_Blue(_texturemanager, Position, 20, 16, 64);       //爆炸动画
                breakParticle = new Particle.BreakPointsParticles(_texturemanager, 0);   //溅射粒子
                breakParticle.EnableVisibleLenth(true, 5);

                TCset fire01 = new TCset(60, 1200);              //200-500帧第一阶段释放子弹
                fire01.Name = "fire01";
                commands.Add(fire01);                            //添加进任务队列
                TCset fire02 = new TCset(60, 1200);              //200-500帧第一阶段释放子弹
                fire02.Name = "fire02";
                commands.Add(fire02);                            //添加进任务队列

            }
            void playcommands()
            {
                time_caculate++;
                                                      #region Normal Level
                if (Datas._GameLevel == GameLevel.normal)
                foreach (TCset tcs in commands)
                {
                    if (tcs.interval == time_caculate)
                    {
                        tcs.useable = true;
                    }
                    if (tcs.useable)
                    {
                        if (tcs.Name == "fire01")
                        {
                            tcs.TickRun();         //所有计时周期进一格
                            #region fire01
                            if (tcs.Tick01 == 130)
                            {
                                //1
                                Vector2D v = new Vector2D(vct.X, vct.Y);
                                double angle = Datas.GameRandom.NextDouble() *  90 - 30;
                                v.rotate((float)angle);
                                Bullet b = new Stage1.Bullet1(texturemanager, v,
                                    new Vector2D(Position.X, Position.Y),2);
                                bullet_toAdd.Add(b);
                                //2
                                v = new Vector2D(vct.X, vct.Y);
                                angle = Datas.GameRandom.NextDouble() * 90 - 30;
                                v.rotate((float)angle);
                                b = new Stage1.Bullet1(texturemanager, v,
                                    new Vector2D(Position.X, Position.Y), 2);
                                bullet_toAdd.Add(b);
                                //3
                                v = new Vector2D(vct.X, vct.Y);
                                angle = Datas.GameRandom.NextDouble() * 60 - 30;
                                v.rotate((float)angle);
                                b = new Stage1.Bullet1(texturemanager, v,
                                    new Vector2D(Position.X, Position.Y),2);
                                bullet_toAdd.Add(b);
                                tcs.Tick01 = 0;    //(Tick01专门控制定周期发射子弹)
                            }
                            #endregion
                        }
                        else if (tcs.Name == "fire02")
                        {
                            tcs.TickRun();         //所有计时周期进一格
                            #region fire02
                            if (tcs.Tick01 == 60)
                            {
                                //获取瞄准玩家的方向
                                vct = new Vector2D(Datas.CurrentPlayer.Position.X - Position.X,
                                Datas.CurrentPlayer.Position.Y - Position.Y).GetNormalize();
                                Bullet b = new Stage1.Bullet1(texturemanager, new Vector2D(vct.X,vct.Y),
                                    new Vector2D(Position.X, Position.Y));
                                bullet_toAdd.Add(b);
                                tcs.Tick01 = 0;    //(Tick01专门控制定周期发射子弹)
                            }
                            #endregion 
                        }
                    }
                    if (tcs.caculateTime == time_caculate)
                    {
                        tcs.useable = false;
                    }
                }
                #endregion
            }

            public override bool Collision(Collider c)
            {
                return base.Collision(c);
            }

            public override void Fire()
            {
                if (!living)
                    return;
                Bullet b = new Stage01_EB01(texturemanager, new Vector2D(0, -1), Position);
                b.Position.X = Position.X; b.Position.Y = Position.Y;
                bullet_toAdd.Add(b);
            }
            public override void Render()
            {
                if (living)
                {
                    if (hited)
                    {
                        enemybody.sprite.SetColor(new Color(1, 0.7f, 0.7f, 1));
                        hited = false;
                    }
                    else
                        enemybody.sprite.SetColor(new Color(1, 1, 1, 1));
                    enemybody.sprite.SetPosition(Position.X, Position.Y);
                    Stage1State._renderer.DrawSprite(enemybody.sprite);
                }
                else
                {
                    Break_ani.Render(Stage1State._renderer);
                    breakParticle.Render(Stage1State._renderer);
                }
            }

            public override void Hitted(AttackValue atkv)
            {
                this.hp -= atkv.attack;
                hited = true;               //被击中
            }

            public override void Update(double elapsedTime)
            {
                base.Update(elapsedTime);
                enemybody.Update(elapsedTime);                         //皮肤逻辑更新
                if (living)                                            //是否存活
                {
                    controler.Update(elapsedTime);                     //行为控制器逻辑更新
                    playcommands();                                    //执行命令队列的命令
                }
                else
                {
                    breakParticle.Update(elapsedTime);         //粒子需要一直保持更新
                    if ((!Break_ani.work_over))                //工作是否完成
                    {
                        Break_ani.Update(elapsedTime);
                    }
                    else
                    {
                        Break_ani.working = false;           //停止工作
                        if (breakParticle.particles.Count == 0)
                            disabled = true;            //粒子消失完后销毁
                    }
                }
                //血掉光后死亡
                if (hp <= 0 && living)
                {
                    living = false;
                    Break_ani.working = true;                      //打开爆炸动画
                    //生成溅射粒子，形成粒子效果
                    for (int i = 0; i < 20; i++)
                    {
                        breakParticle.AddParticle(Position, 1);    //生成概率为1
                    }
                    int count = (int)(Datas.GameRandom.NextDouble() * 2) + 1;
                    int score = (int)(Datas.GameRandom.NextDouble() * 20000 + 100000);
                    //死亡时掉落道具
                    for (int i = 0; i < count; i++)
                    {
                        Datas.CurrentItemAdd.Add(new PowerPoint(texturemanager, Position.X, Position.Y));
                    }
                    Datas.CurrentItemAdd.Add(new GrazePoint(texturemanager, Position.X, Position.Y, score));
                    //Datas.CurrentItemAdd.Add(new GratePowerPoint(texturemanager, Position.X, Position.Y));
                    //Datas.CurrentItemAdd.Add(new BoomPoint(texturemanager, Position.X, Position.Y));
                    //Datas.CurrentItemAdd.Add(new HelpPoint(texturemanager, Position.X, Position.Y));
                }

                int currentstate = enemybody.multiSprite.State;
                if (Direction.X < -0.2)
                {
                    if (currentstate != 3 && currentstate != 4)                //切换为向左移动动画
                    {
                        enemybody.multiSprite.State = 3;
                    }
                }
                else if (Direction.X < 0.2)                            //切换为正面状态
                {
                    if (currentstate != 0)
                    {
                        enemybody.multiSprite.State = 0;
                    }
                }
                else                                                   //切换为向右移动动画
                {
                    if (currentstate != 1 && currentstate != 2)
                    {
                        enemybody.multiSprite.State = 1;
                    }
                }

                //越界则销毁此对象
                if (Position.X < -210 || Position.X > 210 || Position.Y < -30)
                {
                    living = false;
                    disabled = true;
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
        }

        public class Enemy02 : Enemy, RoundCollider
        {
            EnemyData enemydata;             //敌人的数据
            Controler controler;             //行为控制器
            float radius = 20;               //判定半径
            TextureManager texturemanager;   //纹理管理器
            List<TCset> commands = new List<TCset>();           //命令列表
            int time_caculate = 0;                              //计时器（帧数）
            bool turnleft;                                      //是否向左转
            Vector2D vct = new Vector2D(0, 1);                                       //用于存放指向敌人的主方向
            bool hited = false;           //是否被击中

            BreakOut_Blue Break_ani;      //被击破后的动画
            Particle.BreakPointsParticles breakParticle;      //击破后的溅射粒子

            public Enemy02(TextureManager _texturemanager, float x, float y, bool _turnleft = false)
            {
                name = "Stage1Enemy02";
                texturemanager = _texturemanager;
                Position.X = x; Position.Y = y;
                enemybody = new DarkBlueLittleSprite(texturemanager);   //皮肤选择为DarkBlueLittleSprite
                enemybody.multiSprite.State = 0;                        //一开始先重置为一般状态
                hp = 4000;
                enemydata = new EnemyData(Position, Direction, Scale, 100);    //建立一个敌人数据，用于传递给行为控制器使用
                turnleft = _turnleft;

                /////添加控制器处//////
                controler = new Stage1.Controler02(enemydata);       //建立一个专属的行为控制器
                renderlevel = 0;                                 //在敌人列表中置于最底层
                /////添加动画爆炸处////
                Break_ani = new BreakOut_Blue(_texturemanager, Position, 20, 16, 64);       //爆炸动画
                breakParticle = new Particle.BreakPointsParticles(_texturemanager, 0);   //溅射粒子
                breakParticle.EnableVisibleLenth(true, 5);

                TCset fire01 = new TCset(160, 210);              //200-500帧第一阶段释放子弹
                fire01.Name = "fire01";
                commands.Add(fire01);                            //添加进任务队列
                TCset fire02 = new TCset(250, 300);              //200-500帧第一阶段释放子弹
                fire02.Name = "fire02";
                commands.Add(fire02);                            //添加进任务队列
                TCset fire03 = new TCset(340, 390, "fire03");    //第三发射任务
                commands.Add(fire03);

                TCset fire04 = new TCset(60, 1200, "fire04");    //自机狙任务
                commands.Add(fire04);
            }
            void playcommands()
            {
                time_caculate++;
                #region Normal Level
                if (Datas._GameLevel == GameLevel.normal)
                    foreach (TCset tcs in commands)
                    {
                        if (tcs.interval == time_caculate)
                        {
                            tcs.useable = true;
                        }
                        if (tcs.useable)
                        {
                            if (tcs.Name == "fire01")
                            {
                                tcs.TickRun();         //所有计时周期进一格
                                #region fire01
                                if (tcs.Tick01 == 10)
                                {
                                    //1
                                    Vector2D v = new Vector2D(0, -1);
                                    v.rotate(15);
                                    Bullet b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), 2);
                                    bullet_toAdd.Add(b);
                                    //2
                                    v = new Vector2D(0, -1);
                                    v.rotate(30);
                                    b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), 2);
                                    bullet_toAdd.Add(b);
                                    //3
                                    v = new Vector2D(0, -1);
                                    v.rotate(-15);
                                    b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), 2);
                                    bullet_toAdd.Add(b);
                                    //4
                                    v = new Vector2D(0, -1);
                                    v.rotate(-30);
                                    b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), 2);
                                    bullet_toAdd.Add(b);
                                    //5
                                    v = new Vector2D(0, -1);
                                    b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), 2);
                                    bullet_toAdd.Add(b);
                                    tcs.Tick01 = 0;    //(Tick01专门控制定周期发射子弹)
                                }
                                #endregion
                            }
                            else if (tcs.Name == "fire02")
                            {
                                tcs.TickRun();         //所有计时周期进一格
                                #region fire02
                                if (tcs.Tick01 == 10)
                                {
                                    //1
                                    Vector2D v = new Vector2D(0, -1);
                                    v.rotate(15);
                                    Bullet b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), 2);
                                    bullet_toAdd.Add(b);
                                    //2
                                    v = new Vector2D(0, -1);
                                    v.rotate(30);
                                    b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), 2);
                                    bullet_toAdd.Add(b);
                                    //3
                                    v = new Vector2D(0, -1);
                                    v.rotate(-15);
                                    b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), 2);
                                    bullet_toAdd.Add(b);
                                    //4
                                    v = new Vector2D(0, -1);
                                    v.rotate(-30);
                                    b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), 2);
                                    bullet_toAdd.Add(b);
                                    //5
                                    v = new Vector2D(0, -1);
                                    b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), 2);
                                    bullet_toAdd.Add(b);
                                    tcs.Tick01 = 0;    //(Tick01专门控制定周期发射子弹)
                                }
                                #endregion
                            }
                            else if (tcs.Name == "fire03")
                            {
                                tcs.TickRun();         //所有计时周期进一格
                                #region fire03
                                if (tcs.Tick01 == 10)
                                {
                                    //1
                                    Vector2D v = new Vector2D(0, -1);
                                    v.rotate(15);
                                    Bullet b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), 2);
                                    bullet_toAdd.Add(b);
                                    //2
                                    v = new Vector2D(0, -1);
                                    v.rotate(30);
                                    b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), 2);
                                    bullet_toAdd.Add(b);
                                    //3
                                    v = new Vector2D(0, -1);
                                    v.rotate(-15);
                                    b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), 2);
                                    bullet_toAdd.Add(b);
                                    //4
                                    v = new Vector2D(0, -1);
                                    v.rotate(-30);
                                    b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), 2);
                                    bullet_toAdd.Add(b);
                                    //5
                                    v = new Vector2D(0, -1);
                                    b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), 2);
                                    bullet_toAdd.Add(b);
                                    tcs.Tick01 = 0;    //(Tick01专门控制定周期发射子弹)
                                }
                                #endregion
                            }
                            else if (tcs.Name == "fire04")
                            {
                                tcs.TickRun();         //所有计时周期进一格
                                #region fire04
                                if (tcs.Tick01 == 60)
                                {
                                    //获取瞄准玩家的方向
                                    vct = new Vector2D(Datas.CurrentPlayer.Position.X - Position.X,
                                    Datas.CurrentPlayer.Position.Y - Position.Y).GetNormalize();
                                    Bullet b = new Stage1.Bullet1(texturemanager, new Vector2D(vct.X, vct.Y),
                                        new Vector2D(Position.X, Position.Y));
                                    bullet_toAdd.Add(b);
                                    tcs.Tick01 = 0;    //(Tick01专门控制定周期发射子弹)
                                }
                                #endregion
                            }
                        }
                        if (tcs.caculateTime == time_caculate)
                        {
                            tcs.useable = false;
                        }
                    }
                #endregion
            }

            public override bool Collision(Collider c)
            {
                return base.Collision(c);
            }

            public override void Fire()
            {
                if (!living)
                    return;
                Bullet b = new Stage01_EB01(texturemanager, new Vector2D(0, -1), Position);
                b.Position.X = Position.X; b.Position.Y = Position.Y;
                bullet_toAdd.Add(b);
            }
            public override void Render()
            {
                if (living)
                {
                    if (hited)
                    {
                        enemybody.sprite.SetColor(new Color(1, 0.7f, 0.7f, 1));
                        hited = false;
                    }
                    else
                        enemybody.sprite.SetColor(new Color(1, 1, 1, 1));
                    enemybody.sprite.SetPosition(Position.X, Position.Y);
                    Stage1State._renderer.DrawSprite(enemybody.sprite);
                }
                else
                {
                    Break_ani.Render(Stage1State._renderer);
                    breakParticle.Render(Stage1State._renderer);
                }
            }

            public override void Hitted(AttackValue atkv)
            {
                this.hp -= atkv.attack;
                hited = true;               //被击中
            }

            public override void Update(double elapsedTime)
            {
                base.Update(elapsedTime);
                enemybody.Update(elapsedTime);                         //皮肤逻辑更新
                if (living)                                            //是否存活
                {
                    controler.Update(elapsedTime);                     //行为控制器逻辑更新
                    playcommands();                                    //执行命令队列的命令
                }
                else
                {
                    breakParticle.Update(elapsedTime);         //粒子需要一直保持更新
                    if ((!Break_ani.work_over))                //工作是否完成
                    {
                        Break_ani.Update(elapsedTime);
                    }
                    else
                    {
                        Break_ani.working = false;           //停止工作
                        if (breakParticle.particles.Count == 0)
                            disabled = true;            //粒子消失完后销毁
                    }
                }
                //血掉光后死亡
                if (hp <= 0 && living)
                {
                    living = false;
                    Break_ani.working = true;                      //打开爆炸动画
                    //生成溅射粒子，形成粒子效果
                    for (int i = 0; i < 20; i++)
                    {
                        breakParticle.AddParticle(Position, 1);    //生成概率为1
                    }
                    int count = (int)(Datas.GameRandom.NextDouble() * 2) + 1;
                    int score = (int)(Datas.GameRandom.NextDouble() * 20000 + 100000);
                    //死亡时掉落道具
                    for (int i = 0; i < count; i++)
                    {
                        Datas.CurrentItemAdd.Add(new PowerPoint(texturemanager, Position.X, Position.Y));
                    }
                    Datas.CurrentItemAdd.Add(new GrazePoint(texturemanager, Position.X, Position.Y, score));
                    //Datas.CurrentItemAdd.Add(new GratePowerPoint(texturemanager, Position.X, Position.Y));
                    //Datas.CurrentItemAdd.Add(new BoomPoint(texturemanager, Position.X, Position.Y));
                    //Datas.CurrentItemAdd.Add(new HelpPoint(texturemanager, Position.X, Position.Y));
                }

                int currentstate = enemybody.multiSprite.State;
                if (Direction.X < -0.2)
                {
                    if (currentstate != 3 && currentstate != 4)                //切换为向左移动动画
                    {
                        enemybody.multiSprite.State = 3;
                    }
                }
                else if (Direction.X < 0.2)                            //切换为正面状态
                {
                    if (currentstate != 0)
                    {
                        enemybody.multiSprite.State = 0;
                    }
                }
                else                                                   //切换为向右移动动画
                {
                    if (currentstate != 1 && currentstate != 2)
                    {
                        enemybody.multiSprite.State = 1;
                    }
                }

                //越界则销毁此对象
                if (Position.X < -210 || Position.X > 210 || Position.Y < -30)
                {
                    living = false;
                    disabled = true;
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
        }

        public class Enemy02_2 : Enemy, RoundCollider
        {
            EnemyData enemydata;             //敌人的数据
            Controler controler;             //行为控制器
            float radius = 20;               //判定半径
            TextureManager texturemanager;   //纹理管理器
            List<TCset> commands = new List<TCset>();           //命令列表
            int time_caculate = 0;                              //计时器（帧数）
            bool turnleft;                                      //是否向左转
            Vector2D vct = new Vector2D(0, 1);                                       //用于存放指向敌人的主方向
            bool hited = false;           //是否被击中

            BreakOut_Blue Break_ani;      //被击破后的动画
            Particle.BreakPointsParticles breakParticle;      //击破后的溅射粒子

            public Enemy02_2(TextureManager _texturemanager, float x, float y, bool _turnleft = false)
            {
                name = "Stage1Enemy02_2";
                texturemanager = _texturemanager;
                Position.X = x; Position.Y = y;
                enemybody = new DarkBlueLittleSprite(texturemanager);   //皮肤选择为DarkBlueLittleSprite
                enemybody.multiSprite.State = 0;                        //一开始先重置为一般状态
                hp = 1500;
                enemydata = new EnemyData(Position, Direction, Scale, 100);    //建立一个敌人数据，用于传递给行为控制器使用
                turnleft = _turnleft;

                /////添加控制器处//////
                controler = new Stage1.Controler02_02(enemydata,_turnleft);       //建立一个专属的行为控制器
                renderlevel = 0;                                 //在敌人列表中置于最底层
                /////添加动画爆炸处////
                Break_ani = new BreakOut_Blue(_texturemanager, Position, 20, 16, 64);       //爆炸动画
                breakParticle = new Particle.BreakPointsParticles(_texturemanager, 0);   //溅射粒子
                breakParticle.EnableVisibleLenth(true, 5);

                TCset fire01 = new TCset(1, 51);              //200-500帧第一阶段释放子弹
                fire01.Name = "fire01";
                commands.Add(fire01);             
            }
            void playcommands()
            {
                time_caculate++;
                #region Normal Level
                if (Datas._GameLevel == GameLevel.normal)
                    foreach (TCset tcs in commands)
                    {
                        if (tcs.interval == time_caculate)
                        {
                            tcs.useable = true;
                        }
                        if (tcs.useable)
                        {
                            if (tcs.Name == "fire01")
                            {
                                tcs.TickRun();         //所有计时周期进一格
                                #region fire01
                                if (tcs.Tick01 == 10)
                                {
                                    int color = 2;
                                    if (turnleft)
                                        color = 4;
                                    //1
                                    Vector2D v = new Vector2D(0, -1);
                                    v.rotate(15);
                                    Bullet b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), color, 150);
                                    bullet_toAdd.Add(b);
                                    //2
                                    v = new Vector2D(0, -1);
                                    v.rotate(30);
                                    b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), color, 150);
                                    bullet_toAdd.Add(b);
                                    //3
                                    v = new Vector2D(0, -1);
                                    v.rotate(-15);
                                    b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), color, 150);
                                    bullet_toAdd.Add(b);
                                    //4
                                    v = new Vector2D(0, -1);
                                    v.rotate(-30);
                                    b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), color, 150);
                                    bullet_toAdd.Add(b);
                                    //5
                                    v = new Vector2D(0, -1);
                                    b = new Stage1.Bullet1(texturemanager, v,
                                        new Vector2D(Position.X, Position.Y), color, 150);
                                    bullet_toAdd.Add(b);
                                    tcs.Tick01 = 0;    //(Tick01专门控制定周期发射子弹)
                                }
                                #endregion
                            }
                            
                        }
                        if (tcs.caculateTime == time_caculate)
                        {
                            tcs.useable = false;
                        }
                    }
                #endregion
            }

            public override bool Collision(Collider c)
            {
                return base.Collision(c);
            }

            public override void Fire()
            {
                if (!living)
                    return;
                Bullet b = new Stage01_EB01(texturemanager, new Vector2D(0, -1), Position);
                b.Position.X = Position.X; b.Position.Y = Position.Y;
                bullet_toAdd.Add(b);
            }
            public override void Render()
            {
                if (living)
                {
                    if (hited)
                    {
                        enemybody.sprite.SetColor(new Color(1, 0.7f, 0.7f, 1));
                        hited = false;
                    }
                    else
                        enemybody.sprite.SetColor(new Color(1, 1, 1, 1));
                    enemybody.sprite.SetPosition(Position.X, Position.Y);
                    Stage1State._renderer.DrawSprite(enemybody.sprite);
                }
                else
                {
                    Break_ani.Render(Stage1State._renderer);
                    breakParticle.Render(Stage1State._renderer);
                }
            }

            public override void Hitted(AttackValue atkv)
            {
                this.hp -= atkv.attack;
                hited = true;               //被击中
            }

            public override void Update(double elapsedTime)
            {
                base.Update(elapsedTime);
                enemybody.Update(elapsedTime);                         //皮肤逻辑更新
                if (living)                                            //是否存活
                {
                    controler.Update(elapsedTime);                     //行为控制器逻辑更新
                    playcommands();                                    //执行命令队列的命令
                }
                else
                {
                    breakParticle.Update(elapsedTime);         //粒子需要一直保持更新
                    if ((!Break_ani.work_over))                //工作是否完成
                    {
                        Break_ani.Update(elapsedTime);
                    }
                    else
                    {
                        Break_ani.working = false;           //停止工作
                        if (breakParticle.particles.Count == 0)
                            disabled = true;            //粒子消失完后销毁
                    }
                }
                //血掉光后死亡
                if (hp <= 0 && living)
                {
                    living = false;
                    Break_ani.working = true;                      //打开爆炸动画
                    //生成溅射粒子，形成粒子效果
                    for (int i = 0; i < 20; i++)
                    {
                        breakParticle.AddParticle(Position, 1);    //生成概率为1
                    }
                    int count = (int)(Datas.GameRandom.NextDouble() * 2) + 1;
                    int score = (int)(Datas.GameRandom.NextDouble() * 20000 + 100000);
                    //死亡时掉落道具
                    for (int i = 0; i < count; i++)
                    {
                        Datas.CurrentItemAdd.Add(new PowerPoint(texturemanager, Position.X, Position.Y));
                    }
                    Datas.CurrentItemAdd.Add(new GrazePoint(texturemanager, Position.X, Position.Y, score));
                    //Datas.CurrentItemAdd.Add(new GratePowerPoint(texturemanager, Position.X, Position.Y));
                    //Datas.CurrentItemAdd.Add(new BoomPoint(texturemanager, Position.X, Position.Y));
                    //Datas.CurrentItemAdd.Add(new HelpPoint(texturemanager, Position.X, Position.Y));
                }

                int currentstate = enemybody.multiSprite.State;
                if (Direction.X < -0.2)
                {
                    if (currentstate != 3 && currentstate != 4)                //切换为向左移动动画
                    {
                        enemybody.multiSprite.State = 3;
                    }
                }
                else if (Direction.X < 0.2)                            //切换为正面状态
                {
                    if (currentstate != 0)
                    {
                        enemybody.multiSprite.State = 0;
                    }
                }
                else                                                   //切换为向右移动动画
                {
                    if (currentstate != 1 && currentstate != 2)
                    {
                        enemybody.multiSprite.State = 1;
                    }
                }

                //越界则销毁此对象
                if (Position.X < -210 || Position.X > 210 || Position.Y < -30)
                {
                    living = false;
                    disabled = true;
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
        }

        public class Enemy03 : Enemy, RoundCollider
        {
            EnemyData enemydata;             //敌人的数据
            Controler controler;             //行为控制器
            float radius = 20;               //判定半径
            TextureManager texturemanager;   //纹理管理器
            List<TCset> commands = new List<TCset>();           //命令列表
            int time_caculate = 0;                              //计时器（帧数）
            bool turnleft;                                      //是否向左转
            Vector2D vct = new Vector2D(0, 1);                                       //用于存放指向敌人的主方向
            bool hited = false;           //是否被击中

            BreakOut_Blue Break_ani;      //被击破后的动画
            Particle.BreakPointsParticles breakParticle;      //击破后的溅射粒子

            public Enemy03(TextureManager _texturemanager, float x, float y, bool _turnleft = false)
            {
                name = "Stage1Enemy03";
                texturemanager = _texturemanager;
                Position.X = x; Position.Y = y;
                enemybody = new DarkBlueLittleSprite(texturemanager);   //皮肤选择为DarkBlueLittleSprite
                enemybody.multiSprite.State = 0;                        //一开始先重置为一般状态
                hp = 1500;
                enemydata = new EnemyData(Position, Direction, Scale, 100);    //建立一个敌人数据，用于传递给行为控制器使用
                turnleft = _turnleft;

                /////添加控制器处//////
                controler = new Stage1.Controler03(enemydata, _turnleft);       //建立一个专属的行为控制器
                renderlevel = 0;                                 //在敌人列表中置于最底层
                /////添加动画爆炸处////
                Break_ani = new BreakOut_Blue(_texturemanager, Position, 20, 16, 64);       //爆炸动画
                breakParticle = new Particle.BreakPointsParticles(_texturemanager, 0);   //溅射粒子
                breakParticle.EnableVisibleLenth(true, 5);

                TCset fire01 = new TCset(60, 120);              //200-500帧第一阶段释放子弹
                fire01.Name = "fire01";
                commands.Add(fire01);
            }
            void playcommands()
            {
                #region Normal Level
                if (Datas._GameLevel == GameLevel.normal)
                    foreach (TCset tcs in commands)
                    {
                        if (tcs.interval == time_caculate)
                        {
                            tcs.useable = true;
                        }
                        if (tcs.useable)
                        {
                            if (tcs.Name == "fire01")
                            {
                                tcs.TickRun();         //所有计时周期进一格
                                #region fire01
                                if (tcs.Tick01 == 10)
                                {
                                   
                                }
                                #endregion
                            }

                        }
                        if (tcs.caculateTime == time_caculate)
                        {
                            tcs.useable = false;
                        }
                    }
                #endregion
                time_caculate++;
            }

            public override bool Collision(Collider c)
            {
                return base.Collision(c);
            }

            public override void Fire()
            {
                if (!living)
                    return;
                Bullet b = new Stage01_EB01(texturemanager, new Vector2D(0, -1), Position);
                b.Position.X = Position.X; b.Position.Y = Position.Y;
                bullet_toAdd.Add(b);
            }
            public override void Render()
            {
                if (living)
                {
                    if (hited)
                    {
                        enemybody.sprite.SetColor(new Color(1, 0.7f, 0.7f, 1));
                        hited = false;
                    }
                    else
                        enemybody.sprite.SetColor(new Color(1, 1, 1, 1));
                    enemybody.sprite.SetPosition(Position.X, Position.Y);
                    Stage1State._renderer.DrawSprite(enemybody.sprite);
                }
                else
                {
                    Break_ani.Render(Stage1State._renderer);
                    breakParticle.Render(Stage1State._renderer);
                }
            }

            public override void Hitted(AttackValue atkv)
            {
                this.hp -= atkv.attack;
                hited = true;               //被击中
            }

            public override void Update(double elapsedTime)
            {
                base.Update(elapsedTime);
                enemybody.Update(elapsedTime);                         //皮肤逻辑更新
                if (living)                                            //是否存活
                {
                    controler.Update(elapsedTime);                     //行为控制器逻辑更新
                    playcommands();                                    //执行命令队列的命令
                }
                else
                {
                    breakParticle.Update(elapsedTime);         //粒子需要一直保持更新
                    if ((!Break_ani.work_over))                //工作是否完成
                    {
                        Break_ani.Update(elapsedTime);
                    }
                    else
                    {
                        Break_ani.working = false;           //停止工作
                        if (breakParticle.particles.Count == 0)
                            disabled = true;            //粒子消失完后销毁
                    }
                }
                //血掉光后死亡
                if (hp <= 0 && living)
                {
                    living = false;
                    Break_ani.working = true;                      //打开爆炸动画
                    //生成溅射粒子，形成粒子效果
                    for (int i = 0; i < 20; i++)
                    {
                        breakParticle.AddParticle(Position, 1);    //生成概率为1
                    }
                    int count = (int)(Datas.GameRandom.NextDouble() * 2) + 1;
                    int score = (int)(Datas.GameRandom.NextDouble() * 20000 + 100000);
                    //死亡时掉落道具
                    for (int i = 0; i < count; i++)
                    {
                        Datas.CurrentItemAdd.Add(new PowerPoint(texturemanager, Position.X, Position.Y));
                    }
                    Datas.CurrentItemAdd.Add(new GrazePoint(texturemanager, Position.X, Position.Y, score));
                    //Datas.CurrentItemAdd.Add(new GratePowerPoint(texturemanager, Position.X, Position.Y));
                    //Datas.CurrentItemAdd.Add(new BoomPoint(texturemanager, Position.X, Position.Y));
                    //Datas.CurrentItemAdd.Add(new HelpPoint(texturemanager, Position.X, Position.Y));
                }

                int currentstate = enemybody.multiSprite.State;
                if (Direction.X < -0.2)
                {
                    if (currentstate != 3 && currentstate != 4)                //切换为向左移动动画
                    {
                        enemybody.multiSprite.State = 3;
                    }
                }
                else if (Direction.X < 0.2)                            //切换为正面状态
                {
                    if (currentstate != 0)
                    {
                        enemybody.multiSprite.State = 0;
                    }
                }
                else                                                   //切换为向右移动动画
                {
                    if (currentstate != 1 && currentstate != 2)
                    {
                        enemybody.multiSprite.State = 1;
                    }
                }

                //越界则销毁此对象
                if (Position.X < -210 || Position.X > 210 || Position.Y < -30)
                {
                    living = false;
                    disabled = true;
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
        }

        public class Enemy03_2 : Enemy, RoundCollider
        {
            EnemyData enemydata;             //敌人的数据
            Controler controler;             //行为控制器
            float radius = 20;               //判定半径
            TextureManager texturemanager;   //纹理管理器
            List<TCset> commands = new List<TCset>();           //命令列表
            int time_caculate = 0;                              //计时器（帧数）
            bool turnleft;                                      //是否向左转
            Vector2D vct = new Vector2D(0, 1);                                       //用于存放指向敌人的主方向
            bool hited = false;           //是否被击中

            BreakOut_Blue Break_ani;      //被击破后的动画
            Particle.BreakPointsParticles breakParticle;      //击破后的溅射粒子

            public Enemy03_2(TextureManager _texturemanager, float x, float y, bool _turnleft = false)
            {
                name = "Stage1Enemy03_2";
                texturemanager = _texturemanager;
                Position.X = x; Position.Y = y;
                enemybody = new DarkBlueLittleSprite(texturemanager);   //皮肤选择为DarkBlueLittleSprite
                enemybody.multiSprite.State = 0;                        //一开始先重置为一般状态
                hp = 1500;
                enemydata = new EnemyData(Position, Direction, Scale, 100);    //建立一个敌人数据，用于传递给行为控制器使用
                turnleft = _turnleft;

                /////添加控制器处//////
                controler = new Stage1.Controler03_02(enemydata);       //建立一个专属的行为控制器
                renderlevel = 0;                                 //在敌人列表中置于最底层
                /////添加动画爆炸处////
                Break_ani = new BreakOut_Blue(_texturemanager, Position, 20, 16, 64);       //爆炸动画
                breakParticle = new Particle.BreakPointsParticles(_texturemanager, 0);   //溅射粒子
                breakParticle.EnableVisibleLenth(true, 5);

                TCset fire01 = new TCset(60, 120);              
                fire01.Name = "fire01";
                commands.Add(fire01);
            }
            void playcommands()
            {
                #region Normal Level
                if (Datas._GameLevel == GameLevel.normal)
                    foreach (TCset tcs in commands)
                    {
                        if (tcs.interval == time_caculate)
                        {
                            tcs.useable = true;
                        }
                        if (tcs.useable)
                        {
                            if (tcs.Name == "fire01")
                            {
                                tcs.TickRun();         //所有计时周期进一格
                                #region fire01
                                if (tcs.Tick01 == 10)
                                {

                                }
                                #endregion
                            }

                        }
                        if (tcs.caculateTime == time_caculate)
                        {
                            tcs.useable = false;
                        }
                    }
                #endregion
                time_caculate++;
            }

            public override bool Collision(Collider c)
            {
                return base.Collision(c);
            }

            public override void Fire()
            {
                if (!living)
                    return;
                Bullet b = new Stage01_EB01(texturemanager, new Vector2D(0, -1), Position);
                b.Position.X = Position.X; b.Position.Y = Position.Y;
                bullet_toAdd.Add(b);
            }
            public override void Render()
            {
                if (living)
                {
                    if (hited)
                    {
                        enemybody.sprite.SetColor(new Color(1, 0.7f, 0.7f, 1));
                        hited = false;
                    }
                    else
                        enemybody.sprite.SetColor(new Color(1, 1, 1, 1));
                    enemybody.sprite.SetPosition(Position.X, Position.Y);
                    Stage1State._renderer.DrawSprite(enemybody.sprite);
                }
                else
                {
                    Break_ani.Render(Stage1State._renderer);
                    breakParticle.Render(Stage1State._renderer);
                }
            }

            public override void Hitted(AttackValue atkv)
            {
                this.hp -= atkv.attack;
                hited = true;               //被击中
            }

            public override void Update(double elapsedTime)
            {
                base.Update(elapsedTime);
                enemybody.Update(elapsedTime);                         //皮肤逻辑更新
                if (living)                                            //是否存活
                {
                    controler.Update(elapsedTime);                     //行为控制器逻辑更新
                    playcommands();                                    //执行命令队列的命令
                }
                else
                {
                    breakParticle.Update(elapsedTime);         //粒子需要一直保持更新
                    if ((!Break_ani.work_over))                //工作是否完成
                    {
                        Break_ani.Update(elapsedTime);
                    }
                    else
                    {
                        Break_ani.working = false;           //停止工作
                        if (breakParticle.particles.Count == 0)
                            disabled = true;            //粒子消失完后销毁
                    }
                }
                //血掉光后死亡
                if (hp <= 0 && living)
                {
                    living = false;
                    Break_ani.working = true;                      //打开爆炸动画
                    //生成溅射粒子，形成粒子效果
                    for (int i = 0; i < 20; i++)
                    {
                        breakParticle.AddParticle(Position, 1);    //生成概率为1
                    }
                    int count = (int)(Datas.GameRandom.NextDouble() * 2) + 1;
                    int score = (int)(Datas.GameRandom.NextDouble() * 20000 + 100000);
                    //死亡时掉落道具
                    for (int i = 0; i < count; i++)
                    {
                        Datas.CurrentItemAdd.Add(new PowerPoint(texturemanager, Position.X, Position.Y));
                    }
                    Datas.CurrentItemAdd.Add(new GrazePoint(texturemanager, Position.X, Position.Y, score));
                    //Datas.CurrentItemAdd.Add(new GratePowerPoint(texturemanager, Position.X, Position.Y));
                    //Datas.CurrentItemAdd.Add(new BoomPoint(texturemanager, Position.X, Position.Y));
                    //Datas.CurrentItemAdd.Add(new HelpPoint(texturemanager, Position.X, Position.Y));
                }

                int currentstate = enemybody.multiSprite.State;
                if (Direction.X < -0.2)
                {
                    if (currentstate != 3 && currentstate != 4)                //切换为向左移动动画
                    {
                        enemybody.multiSprite.State = 3;
                    }
                }
                else if (Direction.X < 0.2)                            //切换为正面状态
                {
                    if (currentstate != 0)
                    {
                        enemybody.multiSprite.State = 0;
                    }
                }
                else                                                   //切换为向右移动动画
                {
                    if (currentstate != 1 && currentstate != 2)
                    {
                        enemybody.multiSprite.State = 1;
                    }
                }

                //越界则销毁此对象
                if (Position.X < -210 || Position.X > 210 || Position.Y < -30)
                {
                    living = false;
                    disabled = true;
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
        }


    }

}
