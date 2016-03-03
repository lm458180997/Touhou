using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Entities.Controlors
{
    
    /// <summary>
    /// Stage01_Enemy Bullet01 Controler        关卡1 敌机子弹01 行为控制器
    /// 行走直线的行为控制器（简单控制）
    /// </summary>
    public class DirectControler : Controler
    {
        public BulletData bulletdata;         //子弹的记录数据
        public DirectControler(BulletData b)
        {
            this.bulletdata = b;
            Direction = bulletdata.Direction;
            Position = bulletdata.Position;
        }
        public override void Update(double elapsedTime)
        {
            double dx = elapsedTime * Direction.X * bulletdata.Speed;
            double dy = elapsedTime * Direction.Y * bulletdata.Speed;
            double tx = Position.X + dx;
            double ty = Position.Y + dy;
            Position.X = tx;
            Position.Y = ty;
        }
    }

}
