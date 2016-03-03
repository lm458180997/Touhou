using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{
    public class Vector2D      //表示方向的一个类
    {
        private double x, y;
        public double X       //方向属性用X,Y来表示
        {
            get { return x; }
            set { x = value; }
        }
        public double Y
        {
            get { return y; }
            set { y = value; }
        }
        public Vector2D() : this(0, 0) { }
        public Vector2D(float x, float y)  //float精度
        {
            this.x = x;
            this.y = y;
        }
        public override string ToString()
        {
            return "X:"+ x.ToString() +"Y:"+ y.ToString();
        }
        public Vector2D(float x)           //用角度来获得方向,正右方向，顺时针旋转 起
        {
            this.x = (float)Math.Acos(x);
            this.y = (float)Math.Asin(x);
        }
        public Vector2D(double x, double y)//double精度
        {
            this.x = (float)x;
            this.y = (float)y;
        }
        public Vector2D(Point p)            //点的构造方法
        {
            this.x = p.X;
            this.y = p.Y;
        }
        public float ToRadian()            
        {
            if (x != 0)
                return (float)Math.Atan2(this.y, this.x) * 180 / (float)Math.PI;
            else if (y == -1)
                return 1.5f * (float)Math.PI;
            else if (y == 1)
                return 0.5f * (float)Math.PI;
            else
                return 0;
        }
        public void rotate(float a)               //根据角度，方向进行旋转，顺时针起
        {
            double x = this.x;
            double y = this.y;
            this.x = (x * Math.Cos(a / 180 * Math.PI) - y * Math.Sin(a / 180 * Math.PI));         //根据新角度重新定义其向量
            this.y = (x * Math.Sin(a / 180 * Math.PI) + y * Math.Cos(a / 180 * Math.PI));
            Normalize();
        }

        public static Vector2D operator +(Vector2D v1, Vector2D v2)  //向量叠加
        {
            return new Vector2D(v1.x + v2.x, v1.y + v2.y);
        }
        public static Vector2D operator -(Vector2D v1, Vector2D v2)  //向量叠减
        {
            return new Vector2D(v1.x - v2.x, v1.y - v2.y);
        }
        public static Vector2D operator *(Vector2D v1, Vector2D v2)  //向量相乘
        {
            return new Vector2D(v1.x * v2.x, v1.y * v2.y);
        }
        public static Vector2D operator *(Vector2D v1, float s)    //向量数乘
        {
            return new Vector2D(v1.x * s, v1.y * s);
        }
        //public static bool operator ==(Vector2D v1, Vector2D v2)   //相等判断
        //{
        //    return v1.x == v2.y && v1.y == v2.y;
        //}
        //public static bool operator !=(Vector2D v1, Vector2D v2)   //不等判断
        //{
        //    return !(v1 == v2);
        //}
        public float Length()                                  //返回长度
        {
            return (float)Math.Sqrt(x * x + y * y);
        }

        public static float distance(Vector2D v1, Vector2D v2)
        {
            return (float)Math.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y));
        }
        public Vector2D Cross(Vector2D v2)
        {
            return new Vector2D();                   //清空方向
        }
        public void Normalize()     //将方向数值单位化
        {
            if (Length() == 0)      //排出意外情况
                return;
            float l = Length();
            x /= l;
            y /= l;
        }
        public float getcurve()     //获得方向的角度度(仅对于敌人)
        {
            return (float)(Math.Atan2(y, x) * 180 / Math.PI) - 90;
        }

        public double GetAngle()    //获取方位角（0——359）
        {
            if (x > 0)
            {
                if (y == 0)
                {
                    return 90;
                }
                else if (y > 0)
                {
                   return Math.Atan(x / y) * 180 / Math.PI ;
                }
                else if (y < 0)
                {
                    return Math.Atan(-y / x) * 180 / Math.PI + 90;
                }
            }
            else if (x == 0)
            {
                if (y < 0)
                    return 180;
                return 0;
            }
            else if (x < 0)
            {
                if (y < 0)
                {
                    return Math.Atan(x / y) * 180 / Math.PI +180;
                }
                else if (y == 0)
                {
                    return 270;
                }
                else if (y > 0)
                {
                    return Math.Atan(y / -x) * 180 / Math.PI + 270;
                }
            }
            return 0;
        }

        public Vector2D opposite()
        {
            return new Vector2D(-x, -y);
        }
        public Vector2D GetNormalize()       // 返回一个标准化的矢量
        {
            Normalize();
            return this;
        }
    }
}
