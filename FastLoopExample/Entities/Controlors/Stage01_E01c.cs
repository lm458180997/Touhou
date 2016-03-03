using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{


    public class Stage01_E01c : Controler
    {

        EnemyData enemydata;
        double time_caculate;               //计时器(帧数)
        Vector2D direction;                 //记录其参考方向
        Vector2D position;                  //记录其坐标
        List<TCset> commands = new List<TCset>();          //命令列表
        bool turnleft;
        public Stage01_E01c(EnemyData edt,bool _turnleft = false)
        {
            turnleft = _turnleft;
            enemydata = edt;
            enemydata.Speed = 150;
            direction = enemydata.Direction;
            position = enemydata.Position;
            direction.X = 0;
            direction.Y = -1;

            TCset godir = new TCset(0,200);                  //0-200帧行走直线并且进行减速
            godir.Name = "godir";
            commands.Add(godir);

            TCset rotate = new TCset(500, 580);              //500-580帧时进行旋转，并且进行加速 
            rotate .Name =  "rotate"; 
            commands .Add(rotate);
        }

        public override void Update(double elapsedTime)
        {
            playCommand();
            double dx = elapsedTime * direction.X * enemydata.Speed;
            double dy = elapsedTime * direction.Y * enemydata.Speed;
            double tx = position.X + dx;
            double ty = position.Y + dy;
            position.X = tx;
            position.Y = ty;

            time_caculate++;
        }

        void playCommand()
        {
            foreach (TCset tcs in commands)
            {
                if (tcs.interval == time_caculate)
                {
                    tcs.useable = true;
                }
                if (tcs.useable)
                {
                    if (tcs.Name == "godir")
                    {
                        float dt = (float)tcs.caculateTime - tcs.interval;
                        float percent = (dt - ((float)time_caculate - tcs.interval)) / dt;

                        enemydata.Speed = percent * percent * 200;
                    }
                    if(tcs.Name == "rotate")
                    {
                        float dt = (float)tcs.caculateTime - tcs.interval;
                        float percent = ((float)time_caculate - tcs.interval) / dt * 0.5f + 0.5f;
                        enemydata.Speed = percent * percent * 200;
                        if (turnleft)
                            direction.rotate(-1);
                        else
                            direction.rotate(1);

                    }
                }
                if (tcs.caculateTime == time_caculate)
                {
                    tcs.useable = false;
                }
            }
        }


    }

    namespace Stage1
    {
        public class Controler01 : Controler
        {

            EnemyData enemydata;
            double time_caculate;               //计时器(帧数)
            Vector2D direction;                 //记录其参考方向
            Vector2D position;                  //记录其坐标
            List<TCset> commands = new List<TCset>();          //命令列表
            bool showleft;
            public Controler01(EnemyData edt, bool _left = false)
            {
                showleft = _left;
                enemydata = edt;
                enemydata.Speed = 100;
                direction = enemydata.Direction;
                position = enemydata.Position;
                direction.X = 0;
                direction.Y = -1;

                TCset speeddown = new TCset(0, 120);              //0-120帧行走直线切弧线行走(速度逐渐衰减)
                speeddown.Name = "speeddown";
                commands.Add(speeddown);
                TCset rotate01 = new TCset(60, 120);              //弧线所需的角度变换
                rotate01.Name = "rotate01";
                commands.Add(rotate01);
                TCset speedup01 = new TCset(150, 210, "speedup01");            //停顿30帧后进入加速阶段
                commands.Add(speedup01);
                TCset rotate02 = new TCset(180, 280, "rotate02");             //反向弧度旋转，最终呈正向运动
                rotate02.Name = "rotate02";
                commands.Add(rotate02);

            }

            public override void Update(double elapsedTime)
            {
                playCommand();
                double dx = elapsedTime * direction.X * enemydata.Speed;
                double dy = elapsedTime * direction.Y * enemydata.Speed;
                double tx = position.X + dx;
                double ty = position.Y + dy;
                position.X = tx;
                position.Y = ty;

                time_caculate++;
            }

            void playCommand()
            {
                foreach (TCset tcs in commands)
                {
                    if (tcs.interval == time_caculate)
                    {
                        tcs.useable = true;
                    }
                    if (tcs.useable)
                    {
                        if (tcs.Name == "speeddown")                //速度从150降为30
                        {
                            tcs.TickRun();
                            double percent = tcs.Tick01 / (tcs.caculateTime - tcs.interval);
                            percent = 1-percent;
                            enemydata.Speed = 70 * (float)percent + 30;
                        }
                        if (tcs.Name == "rotate01")                ////弧线所需的角度变换
                        {
                            if(showleft)
                               direction.rotate(-0.5f);
                            else
                                direction.rotate(0.5f);
                        }
                        if (tcs.Name == "speedup01")                ////停顿30帧后进入加速阶段
                        {
                            tcs.TickRun();
                            double percent = tcs.Tick01 / (tcs.caculateTime - tcs.interval);
                            enemydata.Speed = 70 * (float)percent + 30;
                        }
                        if (tcs.Name == "rotate02")                //反向弧度旋转，最终呈正向运动
                        {
                            if (showleft)
                                direction.rotate(0.5f);
                            else
                                direction.rotate(-0.5f);
                        }
                    }
                    if (tcs.caculateTime == time_caculate)
                    {
                        tcs.useable = false;
                    }
                }
            }


        }

        public class Controler02 : Controler
        {

            EnemyData enemydata;
            double time_caculate;               //计时器(帧数)
            Vector2D direction;                 //记录其参考方向
            Vector2D position;                  //记录其坐标
            List<TCset> commands = new List<TCset>();          //命令列表
            public Controler02(EnemyData edt)
            {
                enemydata = edt;
                enemydata.Speed = 100;
                direction = enemydata.Direction;
                position = enemydata.Position;
                direction.X = 0;
                direction.Y = -1;

                TCset speeddown = new TCset(60,160 , "speeddown");              //速度逐渐衰减
                speeddown.Name = "speeddown";
                commands.Add(speeddown);
                TCset speedup01 = new TCset(480, 600, "speedup");            //进入加速阶段
                commands.Add(speedup01);

            }

            public override void Update(double elapsedTime)
            {
                playCommand();
                double dx = elapsedTime * direction.X * enemydata.Speed;
                double dy = elapsedTime * direction.Y * enemydata.Speed;
                double tx = position.X + dx;
                double ty = position.Y + dy;
                position.X = tx;
                position.Y = ty;

                time_caculate++;
            }

            void playCommand()
            {
                foreach (TCset tcs in commands)
                {
                    if (tcs.interval == time_caculate)
                    {
                        tcs.useable = true;
                    }
                    if (tcs.useable)
                    {
                        if (tcs.Name == "speeddown")                //速度从150降为30
                        {
                            tcs.TickRun();
                            double percent = tcs.Tick01 / (tcs.caculateTime-tcs.interval);
                            percent = percent * percent;
                            percent = 1 - percent;
                            enemydata.Speed = 100 * (float)percent;
                        }
                        if (tcs.Name == "speedup")                ////停顿30帧后进入加速阶段
                        {
                            tcs.TickRun();
                            double percent = tcs.Tick01 / (tcs.caculateTime - tcs.interval);
                            percent = percent * percent;
                            enemydata.Speed = 100 * (float)percent ;
                        }
                    }
                    if (tcs.caculateTime == time_caculate)
                    {
                        tcs.useable = false;
                    }
                }
            }


        }

        public class Controler02_02 : Controler
        {

            EnemyData enemydata;
            double time_caculate;               //计时器(帧数)
            Vector2D direction;                 //记录其参考方向
            Vector2D position;                  //记录其坐标
            List<TCset> commands = new List<TCset>();          //命令列表
            bool showleft;
            public Controler02_02(EnemyData edt, bool _left = false)
            {
                showleft = _left;
                enemydata = edt;
                enemydata.Speed = 150;
                direction = enemydata.Direction;
                position = enemydata.Position;
                //此控制器中控制运动轨迹的是额外的Direction，而非原组件（原组件的direction用来控制自身旋转等操作）
                if (_left)
                {
                    position.X = -200;
                    position.Y = 350;
                    direction.X = 1;
                    direction.Y = 0;
                }
                else
                {
                    position.X = 200;
                    position.Y = 350;
                    direction.X = -1;
                    direction.Y = 0;
                }

                TCset rotate1 = new TCset(0, 130,"rotate1");
                commands.Add(rotate1);

            }

            public override void Update(double elapsedTime)
            {
                playCommand();
                double dx = elapsedTime * direction.X * enemydata.Speed;
                double dy = elapsedTime * direction.Y * enemydata.Speed;
                double tx = position.X + dx;
                double ty = position.Y + dy;
                position.X = tx;
                position.Y = ty;

                time_caculate++;
            }

            void playCommand()
            {
                foreach (TCset tcs in commands)
                {
                    if (tcs.interval == time_caculate)
                    {
                        tcs.useable = true;
                    }
                    if (tcs.useable)
                    {
                        if (tcs.Name == "rotate1")               
                        {
                            if (showleft)
                                direction.rotate(-0.35f);
                            else
                                direction.rotate(0.35f);
                        }
                    }
                    if (tcs.caculateTime == time_caculate)
                    {
                        tcs.useable = false;
                    }
                }
            }


        }

        public class Controler03 : Controler
        {

            EnemyData enemydata;
            double time_caculate;               //计时器(帧数)
            Vector2D direction;                 //记录其参考方向
            Vector2D position;                  //记录其坐标
            List<TCset> commands = new List<TCset>();          //命令列表
            bool showleft;
            public Controler03(EnemyData edt, bool _left = false)
            {
                showleft = _left;
                enemydata = edt;
                enemydata.Speed = 150;
                direction = enemydata.Direction;
                position = enemydata.Position;
                if (_left)
                {
                    position.X = -200;
                    position.Y = 350;
                    direction.X = 1;
                    direction.Y = 0;
                }
                else
                {
                    position.X = 200;
                    position.Y = 350;
                    direction.X = -1;
                    direction.Y = 0;
                }

                commands.Add(new TCset(20, 120, "speeddown"));    //减速
                commands.Add(new TCset(0, 120, "rotate1"));      //1阶段旋转[旋转至转轴点]
                commands.Add(new TCset(121, 121, "rotate2"));    //2阶段旋转[一次性固定旋转]，两次旋转呈勾状运动
                commands.Add(new TCset(122, 150, "speedup"));    //旋转完成后进入加速（弹射效果）

            }

            public override void Update(double elapsedTime)
            {
                playCommand();
                double dx = elapsedTime * direction.X * enemydata.Speed;
                double dy = elapsedTime * direction.Y * enemydata.Speed;
                double tx = position.X + dx;
                double ty = position.Y + dy;
                position.X = tx;
                position.Y = ty;

                time_caculate++;
            }

            void playCommand()
            {
                foreach (TCset tcs in commands)
                {
                    if (tcs.interval == time_caculate)
                    {
                        tcs.useable = true;
                    }
                    if (tcs.useable)
                    {
                        if (tcs.Name == "speeddown")              //减速阶段
                        {
                            tcs.TickRun();
                            double percent = tcs.Tick01 / (tcs.caculateTime - tcs.interval);
                            percent = 1 - percent;
                            enemydata.Speed = 150 * (float)percent + 30;
                        }
                        if (tcs.Name == "rotate1")                ////弧线所需的角度变换
                        {
                            if (showleft)
                                direction.rotate(-0.8f);
                            else
                                direction.rotate(0.8f);
                        }
                        if (tcs.Name == "rotate2")                //反向弧度旋转，最终呈正向运动
                        {
                            if (showleft)
                            {
                                direction.X = 3;
                                direction.Y = 1;
                                direction.Normalize();
                            }
                            else
                            {
                                direction.X = -3;
                                direction.Y = 1;
                                direction.Normalize();
                            }
                        }
                        if (tcs.Name == "speedup")                 //旋转完成后进入加速
                        {
                            tcs.TickRun();
                            double percent = tcs.Tick01 / (tcs.caculateTime - tcs.interval);
                            percent = percent *percent;
                            enemydata.Speed = 100 * (float)percent + 30;
                        }
                    }
                    if (tcs.caculateTime == time_caculate)
                    {
                        tcs.useable = false;
                    }
                }
            }


        }

        public class Controler03_02 : Controler
        {
            EnemyData enemydata;
            double time_caculate;               //计时器(帧数)
            Vector2D direction;                 //记录其参考方向
            Vector2D position;                  //记录其坐标
            List<TCset> commands = new List<TCset>();          //命令列表
            public Controler03_02(EnemyData edt)
            {
                enemydata = edt;
                enemydata.Speed = 100;
                direction = enemydata.Direction;
                position = enemydata.Position;
                direction.X = 0;
                direction.Y = -1;

                TCset speeddown = new TCset(60, 160, "speeddown");              //速度逐渐衰减
                speeddown.Name = "speeddown";
                commands.Add(speeddown);
                TCset speedup01 = new TCset(480, 600, "speedup");            //进入加速阶段
                commands.Add(speedup01);

            }

            public override void Update(double elapsedTime)
            {
                playCommand();
                double dx = elapsedTime * direction.X * enemydata.Speed;
                double dy = elapsedTime * direction.Y * enemydata.Speed;
                double tx = position.X + dx;
                double ty = position.Y + dy;
                position.X = tx;
                position.Y = ty;
                time_caculate++;
            }

            void playCommand()
            {
                foreach (TCset tcs in commands)
                {
                    if (tcs.interval == time_caculate)
                    {
                        tcs.useable = true;
                    }
                    if (tcs.useable)
                    {
                        if (tcs.Name == "speeddown")                //速度从150降为30
                        {
                            tcs.TickRun();
                            double percent = tcs.Tick01 / (tcs.caculateTime - tcs.interval);
                            percent = percent * percent;
                            percent = 1 - percent;
                            enemydata.Speed = 100 * (float)percent;
                        }
                        if (tcs.Name == "speedup")                ////停顿30帧后进入加速阶段
                        {
                            tcs.TickRun();
                            double percent = tcs.Tick01 / (tcs.caculateTime - tcs.interval);
                            percent = percent * percent;
                            enemydata.Speed = 100 * (float)percent;
                        }
                    }
                    if (tcs.caculateTime == time_caculate)
                    {
                        tcs.useable = false;
                    }
                }
            }


        }

    }


}
