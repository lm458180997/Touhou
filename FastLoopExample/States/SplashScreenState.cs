using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;

namespace FastLoopExample
{
    class SplashScreenState : IGameObject           //启动界面
    {
        StateSystem _system;
        double _delayInSeconds = 3;               //延迟时间

        public SplashScreenState(StateSystem system)
        {
            _system = system;
        }

        public void Update(double elapsedTime)
        {
            _delayInSeconds -= elapsedTime;
            if (_delayInSeconds <= 0)
            {
                _delayInSeconds = 3;
                _system.ChangeState("title_menu");
            }
        }

        public void Render()
        {
            Gl.glClearColor(1, 1, 1, 1);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            Gl.glFinish();
        }


        public void Start()
        {
            
        }
    }
}
