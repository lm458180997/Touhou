using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;

namespace FastLoopExample
{
    class TitleMenuState : IGameObject                 //标题状态
    {
        double _currentRotation = 0;

        public void Update(double elapsedTime)
        {
            _currentRotation = 10 * elapsedTime;       //每秒10度
        }

        public void Render()
        {
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            Gl.glPointSize(5.0f);
            Gl.glRotated(_currentRotation, 0, 1, 0);
            Gl.glBegin(Gl.GL_TRIANGLE_STRIP);
            {
                Gl.glColor4d(1.0, 0.0, 0.0, 0.5);
                Gl.glVertex3d(-50, 0, 0);
                Gl.glColor3d(0.0, 1.0, 0.0);
                Gl.glVertex3d(50, 0, 0);
                Gl.glColor3d(0.0, 0.0, 1.0);
                Gl.glVertex3d(0, 50, 0);
            }
            Gl.glEnd();
            Gl.glFinish();
        }

        public void Start()
        {
            
        }
    }
}
