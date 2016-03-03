using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{
    /// <summary>
    /// 子弹的相关数据（保管了它的transform相关内容，主要用于行为控制器的使用）
    /// </summary>
    public class BulletData
    {
          public float Speed;          //移动的速度
        //坐标
        public Vector2D Position = new Vector2D();
        //运动方向
        public Vector2D Direction = new Vector2D();
        //旋转方向
        public Vector2D Rotation = new Vector2D();
        //缩放
        public Vector2D Scale = new Vector2D();
        //宽与高
        public float Width;
        public float Height;

        public BulletData(Vector2D pos, Vector2D dir, Vector2D scl, float speed)
        {
            Position = pos;
            Direction = dir;
            Scale = scl;
            Speed = speed;
        }
    }
}
