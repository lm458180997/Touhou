using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;   //需要引入

namespace FastLoopExample
{
    //按照C语言的处理方式在内存中布局结构，更容易与OpenGL库交互
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public Vector(double x, double y, double z):this()
        {
            X = x; Y = y; Z = z;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public float X { get; set; }
        public float Y { get; set; }
        public Point(float x, float y)
            : this()
        {
            X = x; Y = y;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Color
    {
        public float Red { get; set; }
        public float Green { get; set; }
        public float Blue { get; set; }
        public float Alpha { get; set; }
        public Color(float r, float g, float b, float a)
            : this()
        {
            Red = r; Green = g; Blue = b; Alpha = a;
        }
    }


}
