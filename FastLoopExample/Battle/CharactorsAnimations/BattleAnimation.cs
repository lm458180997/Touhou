using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Battle
{
    /// <summary>
    /// 角色的每一种动画
    /// </summary>
    public class BattleAnimation 
    {
        public Sprite[] Sprites;    //保存动画纹理的数组

        public int startIndex=0;    //纹理的读取初始位
        public int lastIndex = 0;   //纹理的读取末尾位
        public int CurrentIndex = 0;//当前阅读到的纹理位置[用于标记进度百分比]

        public int count = 0;       //纹理总共的数量
        public string name = "defalt";  //动画的名字

        public bool needloop = true;     //是否是循环类型动画

        public bool IsOver = false;      //动画是否已经执行结束 

        public double livetime = 0;         //（从生成开始就记录的时间）

        public double caculatetime = 0;     //计时器

        public double changeinterval = 0.06;   //动画帧数切换时间间距

        /// <summary>
        /// 设定当前的执行纹理索引
        /// </summary>
        /// <param name="index">索引值</param>
        public virtual void SetCurrentIndex(int index)
        {
            CurrentIndex = index;
        }
        /// <summary>
        /// 获取当前的纹理索引值
        /// </summary>
        /// <returns>当前的纹理索引值</returns>
        public int GetCurrentIndex()
        {
            return CurrentIndex;
        }


        /// <summary>
        /// 总逻辑更新（不受子类修改）
        /// </summary>
        /// <param name="ElapsedTime">帧间距</param>
        public void Update(double ElapsedTime)
        {
            livetime += ElapsedTime;
            AnimUpdate(ElapsedTime);
        }

        /// <summary>
        /// 动画的逻辑更新
        /// </summary>
        /// <param name="ElapsedTime">帧间差</param>
        public virtual void AnimUpdate(double ElapsedTime)
        {
           

        }
        /// <summary>
        /// 获取当前动画的纹理
        /// </summary>
        /// <returns>当前动画的纹理</returns>
        public virtual Sprite GetSprite()
        {
            return Sprites[CurrentIndex];
        }

    }

}
