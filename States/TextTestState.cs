using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;

namespace FastLoopExample
{
    class TextTestState : IGameObject
    {

        Sprite _text = new Sprite();           //精灵只会负责画出一个矩形
        Renderer _renderer = new Renderer();   //渲染器
        TextFont _textFont;
        Sprite spr;
        public TextTestState(TextureManager textureManager)
        {
            _text.Texture = textureManager.Get("Font");
            _textFont = new TextFont(textureManager);
            spr = _textFont.GetAci("终");
            spr.SetPosition(-50, 50);
            spr.SetHeight(14);spr.SetWidth(14);

            _text.SetUVs(new Point(0.0547f*2, 0.0547f), new Point(0.0547f * 3, 0.0547f * 2));
            _text.SetWidth(14);
            _text.SetHeight(14);
        }

        double time_caculate = 0;
        double time2 = 0;
        float alpha = 1;
        int num = 0;
        bool add = true;
        public void Update(double elapsedTime)
        {
            time_caculate += elapsedTime;
            if (add)
                time2 += elapsedTime;
            else
                time2 -= elapsedTime;
            if (time2 > 1)
            {
                time2 = 1;
                add = false;
            }
            if (time2 < 0.3)
            {
                time2 = 0.3;
                add = true;
            }
            alpha = (float)time2 / 1;
            num += 1;
        }

        public void Render()
        {
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            _renderer.DrawSprite(_text);
            _renderer.DrawSprite(spr);
            DrawText2D(new Point(0, 5), "I'm thunder , A student");
            DrawText2D(new Point(0, 10), "IdsADSDS , A student",14,0.5f);
            DrawText2D2(new Point(0, 35), num.ToString(), 14, alpha);
            Gl.glFinish();
            
        }

        public void DrawText2D(Point position, string text,int width =14 , float Alpha = 1, 
            int dw = 9, int rows = 1, int length = 20)
            // 坐标，文字，宽度， 透明值，间距，行数，每行长度
        {
            int len = text.Length;
            Sprite spr;
            for (int i = 0; i < len ; i++)
            {
                spr = _textFont.GetAci(text.Substring(i, 1));
                spr.SetWidth(width); spr.SetHeight(width);
                spr.SetPosition(new Vector(position.X+width/2+i*dw,position.Y-width/2,0));
                spr.SetColor(new Color(1,1,1,Alpha));
                _renderer.DrawSprite(spr);
            }
        }

        //从右到左（例如“    1”,“   11”,“  111”,“ 1111”,“11111” ）
        public void DrawText2D2(Point position, string text, int width = 14, float Alpha = 1,
            int dw = 9, int rows = 1, int length = 20)
        // 坐标，文字，宽度， 透明值，间距，行数，每行长度
        {
            int len = text.Length;
            Sprite spr;
            for (int i = 0; i < len; i++)
            {
                spr = _textFont.GetAci(text.Substring(i, 1));
                spr.SetWidth(width); spr.SetHeight(width);
                spr.SetPosition(new Vector(position.X - width / 2 - (len-1-i)* dw, position.Y - width / 2, 0));
                spr.SetColor(new Color(1, 1, 1, Alpha));
                _renderer.DrawSprite(spr);
            }
        }


        public void Start()
        {
            
        }
    }
}
