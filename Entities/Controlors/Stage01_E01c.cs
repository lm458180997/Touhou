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


}
