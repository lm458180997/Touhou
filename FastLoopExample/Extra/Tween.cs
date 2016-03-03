using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{
     public class Tween
    {
         double _original;                   //_original是补间初始值
         double _distance;                   //distance 记录变化的长度
         double _current;                    //返回的结果
         double _totalTimePassed = 0;        //已经过时间
         double _totalDuration = 5;          //总共的维持时间
         bool _finished = false;             //工作状态
         TweenFunction _tweenF = null;       //插值方式
         public delegate double TweenFunction(double timePassed, double start,
             double distance, double duration);
        
         public double Value()
         {
             return _current; 
         }

         public bool IsFinished()
         {
             return _finished;
         }

         public Tween(double start, double end, double time)
         {
             Construct(start, end, time, Tween.Linear);
         }

         public Tween(double start, double end, double time, TweenFunction tweenF)
         {
             Construct(start, end, time, tweenF);
         }

         //Construct 记录了补间的初始值、最终值和执行补间操作的时间，可传入一个补间函数来确定只随时间如何变化
         public void Construct(double start, double end, double time, TweenFunction tweenF)
         {
             _distance = end - start;
             _original = start;                    //_original是初始值
             _current = start;
             _totalDuration = time;                //总共的维持时间
             _tweenF = tweenF;
         }

         //Linear线性
         public static double Linear(double timePassed, double start, double distance,
             double duration)
         {
             return distance * timePassed /duration + start;   // 长度*时间百分比 + 初始值
         }

         //对数
         public static double EaseOutExpo(double timePassed, double start, double distance
              , double duration)
         {
             if (timePassed >= duration)
                 return start + distance;
             return distance*(-Math.Pow(2,-10*timePassed/duration)+1)+start;     
         }

         //不同的插值。。
         public static double EaseInExpo(double timePassed, double start,
             double distance, double duration)
         {
             if (timePassed == 0) return start;
             else
                 return distance * Math.Pow(2, 10 * (timePassed / duration - 1)) + start;
         }

         public static double EaseOutCirc(double timePassed, double start,
             double distance, double duration)
         {
             return distance * Math.Sqrt(1 - (timePassed = timePassed / duration - 1) *
                 timePassed) + start;
         }

         public static double EaseInCirc(double timePassed, double start,
             double distance, double duration)
         {
             return -distance * (Math.Sqrt(1 - (timePassed /= duration) * timePassed) - 1) + start;
         }
        
         public void Update(double elapsedTime)
         {
             _totalTimePassed += elapsedTime;          //Timepassed记录的是经过的时间
             _current = _tweenF(_totalTimePassed, _original, _distance, _totalDuration);
             if (_totalTimePassed > _totalDuration)    //TotalDuration记录的是总共的维持时间
             {
                 _totalTimePassed = _totalDuration;
                 _current = _original + _distance;
                 _finished = true;                     //完成工作
             }
         }


    }
}
