using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{
    public class Controler                  //行为(路径)控制器
    {
        public Vector2D Position;           //控制顶点坐标
        public Vector2D Direction;          //方向
        public Controler() { }
        public Controler(Vector2D v)
        {
            Position = v;
        }
        public virtual void Update(double elapsedTime)
        {
        }
    }

}
