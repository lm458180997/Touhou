using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;

namespace FastLoopExample.GameState.Menu
{
    public class BackGround : IGameObject , RenderLevel
    {
        TextureManager _texturemanager;
        public const int _MAINMENU_ = 0 , _START_ = 1;
        int state = _MAINMENU_;
        int _render_level = 0;                                 //着色等级，10为最后一层着色(优先级最高)
        Dictionary<int, Sprite> BackGroundImage;              //背景图片的所有序列
        Renderer renderer;

        public int State
        {
            get { return state; }
            set
            {
                state = value;
            }
        }

        public BackGround(TextureManager texturemanager, int renderlevel)
        {
            _render_level = renderlevel;
            this._texturemanager = texturemanager;
            renderer = new Renderer();

            BackGroundImage = new Dictionary<int, Sprite>();
            BackGroundImage.Add(_MAINMENU_, new Sprite());
            BackGroundImage.Add(_START_, new Sprite());

          //  BackGroundImage[_MAINMENU_].Texture = this._texturemanager.Get("justtest");
            BackGroundImage[_MAINMENU_].Texture = this._texturemanager.Get("menu1");
            BackGroundImage[_START_].Texture = this._texturemanager.Get("menu2");

            //BackGroundImage[_MAINMENU_].SetPosition(0, -100);
            BackGroundImage[_MAINMENU_].SetPosition(0,0);
            BackGroundImage[_MAINMENU_].SetWidth(640);
            BackGroundImage[_MAINMENU_].SetHeight(640);
            BackGroundImage[_START_].SetWidth(1000);
            BackGroundImage[_START_].SetHeight(750);
        }

        public void Start()
        {

        }

        public void Update(double elapsedTime)
        {
            
        }

        public void Render()
        {
            Gl.glShadeModel(Gl.GL_LINE_SMOOTH);
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            renderer.DrawSprite(BackGroundImage[State]);
            Gl.glFinish();
        }

        public int getLevel()               
        {
            return _render_level;
        }

        public void setLevel(int level)
        {
            if (level <= 10)
                _render_level = level;
            else
                _render_level = 10;
        }

        public void RenderByLevel()
        {
            Render();
        }
    }
}
