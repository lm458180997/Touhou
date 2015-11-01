using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{

    //Time Caculate set      , 计时周期装置
    public class TCset
    {
        public bool useable = true;  //是否处于启用状态
        public double caculateTime;  //计时时间
        public float interval;       //计时周期
        public string Name;          //唯一标识
        public bool UseTick = false; //是否会用上计时器
        public int Tick01 = 0 ;      //周期计时工具（设置定时发射弹幕时会用上）
        public int Tick02 = 0 ;
        public int Tick03 = 0 ;
        public TCset(float _interval = 10,string _name = "Left", bool _useable = true)
        {
            Name = _name;                //名字
            useable = _useable;          //初始化使用属性
            interval = _interval;        //赋值计时参数
            UseTick = false ;            //非计时器模式
        }
        public TCset(float _interval, float _caculate, string _name = "default",
            bool _useable = false, bool useTick = false)
        {
            Name = _name;
            interval = _interval;        //初始化计时上限
            caculateTime = _caculate;    //初始化计时下限
            useable = _useable;
            UseTick = useTick;
        }
        public void TickRun()
        {
            Tick01++; Tick02++; Tick03++;
        }
        public void AddTime(double elapsedTime)
        {
            caculateTime += elapsedTime;
        }
        
        //计时器计数清空
        public void ClearTime()
        {
            caculateTime = 0;
        }

    }

}
