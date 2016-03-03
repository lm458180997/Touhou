using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Extra
{
    //状态机
    public  class StateMachine
    {
        string name;                          //状态机名字
        bool isused = false;                  //是否启用
        Dictionary<string, bool> States;      //所有的状态
        Dictionary<string, int> StateValues;  //值类状态记录
        /// <summary>
        /// 状态机[空]
        /// </summary>
        public StateMachine()
        {
            States = new Dictionary<string, bool>();
            StateValues = new Dictionary<string, int>();
        }
        /// <summary>
        /// 状态机初始化
        /// </summary>
        public virtual void Invalidate()
        {
        }
        /// <summary>
        /// 获取状态机名字
        /// </summary>
        /// <returns>返回状态机名字：字符串</returns>
        public virtual string GetName()
        {
            return name;
        }
        /// <summary>
        /// 设定是否启用状态机
        /// </summary>
        /// <param name="isu">设定是否使用</param>
        public virtual void SetUsed(bool isu)
        {
        }
    }


}
