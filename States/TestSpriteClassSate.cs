using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;

namespace FastLoopExample
{
    public class TestSpriteClassSate : IGameObject       //举例怎么使用Sprite和Renderer
    {

        Renderer _renderer = new Renderer();
        TextureManager _textureManager;
        Sprite _testSprite = new Sprite();
        Sprite _testSprite2 = new Sprite();
        Sprite _testSprite3 = new Sprite();
        Sprite _testSprite4 = new Sprite();
        Sprite _testSprite5 = new Sprite();

        public TestSpriteClassSate(TextureManager textureManager)
        {
            _textureManager = textureManager;                    //接手纹理管理器
            _testSprite.Texture = _textureManager.Get("Font");   //设定精灵的纹理
            _testSprite.SetHeight(256 * 0.5f);           //不知什么原因，不能从纹理处获取图片的高度
                                                         //而宽度在帮顶纹理时就能够获得
            _testSprite2.Texture = _textureManager.Get("charactor1");
          //  _testSprite2.SetPosition(0, 100);
            _testSprite2.SetColor(new Color(1, 1, 1, 0.9f));// Alpha值会影响图片整个的透明度，非常实用，可以实现图像渐变的效果
            _testSprite2.SetHeight(128);

            _testSprite3.Texture = _textureManager.Get("Font");
            int g = _testSprite3.Texture.ID;
            _testSprite3.SetColor(new Color(1, 1, 1, 0.7f));// Alpha值会影响图片整个的透明度，非常实用，可以实现图像渐变的效果
            _testSprite3.SetHeight(128);

            _testSprite4.Texture = _textureManager.Get("mbit1");
            _testSprite4.SetColor(new Color(1, 1, 1, 0.5f));// Alpha值会影响图片整个的透明度，非常实用，可以实现图像渐变的效果
            _testSprite4.SetHeight(128);

            _testSprite5.Texture = _textureManager.Get("mbit1");
            _testSprite5.SetColor(new Color(1, 1, 1, 0.3f));// Alpha值会影响图片整个的透明度，非常实用，可以实现图像渐变的效果
            _testSprite5.SetHeight(128);
        }
        double rd = 100;
        double time_caculate = 0;
        double angle_speed = 30;
        static double PI = 3.1415926;
        public void Update(double elapsedTime)
        {
            time_caculate += elapsedTime;
            double rdiu = (time_caculate / 1) * angle_speed;   //每秒转动角度： angle_speed
            _testSprite2.SetPosition(0 + rd * Math.Cos((rdiu / 180) * PI), 
                0 + rd * Math.Sin((rdiu / 180) * PI));

            _testSprite3.SetPosition(0 + rd*1.5 * Math.Cos((rdiu / 180) * PI),
               0 + rd * 1.5 * Math.Sin((rdiu / 180) * PI));

            _testSprite4.SetPosition(0 + rd * 2* Math.Cos((rdiu / 180) * PI),
               0 + rd * 2 * Math.Sin((rdiu / 180) * PI));

            _testSprite5.SetPosition(0 + rd * 3 * Math.Cos((rdiu / 180) * PI),
               0 + rd * 3 * Math.Sin((rdiu / 180) * PI));
        }
        public void Render()
        {
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            
            _renderer.DrawSprite(_testSprite);      //绘制精灵

            Gl.glPushMatrix();                      //矩阵压栈

            double rdiu = (time_caculate / 1) * angle_speed;   //每秒转动角度： angle_speed
            Vector vector = _testSprite2.GetCenter();
            Gl.glTranslated(vector.X, vector.Y, 0);    //坐标移动到目标位置
            Gl.glRotated(rdiu-90, 0, 0, 1);               //旋转
            Gl.glTranslated(-vector.X, -vector.Y, 0);  //坐标原点重置回去

            _renderer.DrawSprite(_testSprite2);
            int g =_testSprite2.Texture.ID;

            Gl.glPopMatrix();                      //矩阵出栈

            _renderer.DrawSprite(_testSprite3);
            _renderer.DrawSprite(_testSprite4);
            _renderer.DrawSprite(_testSprite5);
           // Gl.glFlush();
            Gl.glFinish();
        }

        public void Start()
        {
           
        }
    }
}
