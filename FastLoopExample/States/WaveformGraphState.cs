using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;

namespace FastLoopExample
{
    class WaveformGraphState : IGameObject
    {
        TextureManager _texturemanager;
        double _xPosition = -100;                //原点坐标
        double _yPosition = -100;
        double _xlength = 200;
        double _ylength = 200;
        double _sampleSize =100;                 //平滑度
        double _frequency = 5;                   //震荡频率
        public delegate double WaveFunction(double value);

        public WaveformGraphState(TextureManager texturemanager)
        {
            _texturemanager = texturemanager;
            Gl.glLineWidth(1);               //线条宽度为1
            Gl.glDisable(Gl.GL_TEXTURE_2D);  //关闭纹理映射
        }

        public void DrawAxis()
        {
            Gl.glColor3f(1, 1, 1);
            Gl.glBegin(Gl.GL_LINES);
            {
                //X axis
                Gl.glVertex2d(_xPosition,_yPosition);
                Gl.glVertex2d(_xPosition + _xlength , _yPosition);
                //Y axis
                Gl.glVertex2d(_xPosition , _yPosition);
                Gl.glVertex2d(_xPosition,_yPosition+_ylength);
            }
            Gl.glEnd();
        }
        public void DrawGraph(WaveFunction waveFunction, Color color)
        {
            double xIncrement = _xlength / _sampleSize;
            double previousX = _xPosition;
            double previousY = _yPosition + (0.5 * _ylength);
            Gl.glColor3f(color.Red, color.Green, color.Blue);
            Gl.glBegin(Gl.GL_LINES);              //画出线条
            {
                for (int i = 0; i < _sampleSize; i++)
                {
                         //Work out new X and Y positions
                    double newX = previousX + xIncrement;     //Increment one unit on the x
                         //From 0-1 how far through plotting the graph are we?
                    double percentDone = (i / _sampleSize);
                    double percentRadians = percentDone * (Math.PI * _frequency);
                         //Scale the wave value by the half the length
                    double newY = _yPosition + waveFunction(percentRadians) * (_ylength / 2);
                         //Ignore the first value because the previous X and Y
                         //haven't been worked out yet
                    if (i > 1)
                    {
                        Gl.glVertex2d(previousX, previousY);
                        Gl.glVertex2d(newX, newY);
                    }
                         //Store the previous position
                    previousX = newX;
                    previousY = newY;
                }
            }
            Gl.glEnd();
        }        //Empty Update and Render methods omitted

        public void Update(double elapsedTime)
        {
        }

        public void Render()
        {
            DrawAxis();
            DrawGraph(Math.Sin, new Color(1, 0, 0, 1));    
            //只要返回类型和参数都与委托(waveFunction)相同，那么可以直接写函数在这里
            DrawGraph(Math.Cos, new Color(0, 0.5f, 0.5f, 1));
        }

        public void Start()
        {
           
        }
    }
}
