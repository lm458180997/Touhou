using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;

namespace FastLoopExample
{
    class AboutLura : IGameObject
    {
        TextureManager _textureManager;
        Sprite _testSprite = new Sprite();

        Renderer _renderer = new Renderer();

        SoundManagerEx soundmanager;

        public AboutLura(TextureManager textureManager , SoundManagerEx sd)
        {
            _textureManager = textureManager;
            soundmanager = sd;
            
            _testSprite.Texture = _textureManager.Get("lura1");
            _testSprite.SetWidth(512);
            _testSprite.SetHeight(512);
            
            _testSprite.SetPosition(new Vector(0, 0, 0));

           // soundmanager.Play("rj");
        }

        public void Update(double elapsedTime)
        {
            if (Input.getKeyDown("Escape"))
            {
                Form1._system.ChangeState("Menu");
            }
        }

        public void Render()
        {
            Gl.glShadeModel(Gl.GL_LINE_SMOOTH);
            //Texture texture = _textureManager.Get("lura");
            //Gl.glEnable(Gl.GL_TEXTURE_2D);
            //Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture.ID);
            //Gl.glBegin(Gl.GL_TRIANGLES);
            //{
            //    Gl.glTexCoord2d(0, 0);
            //    Gl.glVertex3d(x - halfwidth, y + halfheight, z); //top left
            //    Gl.glTexCoord2d(1, 0);
            //    Gl.glVertex3d(x + halfwidth, y + halfheight, z);
            //    Gl.glTexCoord2d(0, 1);
            //    Gl.glVertex3d(x - halfwidth, y - halfheight, z);

            //    Gl.glTexCoord2d(1, 0);
            //    Gl.glVertex3d(x + halfwidth, y + halfheight, z);
            //    Gl.glTexCoord2d(1, 1);
            //    Gl.glVertex3d(x + halfwidth, y - halfheight, z);
            //    Gl.glTexCoord2d(0, 1);
            //    Gl.glVertex3d(x - halfwidth, y - halfheight, z);
            //}
            //Gl.glEnd();


            Gl.glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            _renderer.DrawSprite(_testSprite);

            Gl.glFinish();
        }


        public void Start()
        {
        }
    }
}