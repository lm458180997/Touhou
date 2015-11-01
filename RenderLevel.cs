using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{

    //继承此接口的类将会使用着色等级来执行绘画，规定为0——10级（继承后请一定要求按照此规定）
    public interface RenderLevel
    {
         int getLevel();
         void setLevel(int i);
         void RenderByLevel();
    }

}
