using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{
    public interface IGameObject
    {
        void Start();                       //切换状态时执行
        void Update(double elapsedTime);    //逻辑更新
        void Render();                      //画面渲染
    }
}
