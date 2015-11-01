using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;

namespace FastLoopExample
{
    class DrawSpriteState : IGameObject
    {

        TextureManager _textureManager;
        double height = 200;
        double width = 200;
        double halfheight, halfwidth;
        double x = 0, y = 0, z = 0;

        public DrawSpriteState(TextureManager textureManager)
        {
            _textureManager = textureManager;

            Texture texture = _textureManager.Get("huiye");
            height = 128;
            width = 128;

            halfheight = height / 2;
            halfwidth = width / 2;
        }

        public void Update(double elapsedTime)
        {

        }

        public void Render()
        {
            Gl.glShadeModel(Gl.GL_SMOOTH);
            Texture texture = _textureManager.Get("lura");
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture.ID);
            height = 128;
            width = 128;

            halfheight = height / 2;
            halfwidth = width / 2;
            Gl.glBegin(Gl.GL_TRIANGLES);
            {
                Gl.glTexCoord2d(0, 0);
                Gl.glVertex3d(x - halfwidth, y + halfheight, z); //top left
                Gl.glTexCoord2d(1, 0);
                Gl.glVertex3d(x + halfwidth, y + halfheight, z);
                Gl.glTexCoord2d(0, 1);
                Gl.glVertex3d(x - halfwidth, y - halfheight, z);

                Gl.glTexCoord2d(1, 0);
                Gl.glVertex3d(x + halfwidth, y + halfheight, z);
                Gl.glTexCoord2d(1, 1);
                Gl.glVertex3d(x + halfwidth, y - halfheight, z);
                Gl.glTexCoord2d(0, 1);
                Gl.glVertex3d(x - halfwidth, y - halfheight, z);
            }
            Gl.glEnd();

            //texture = _textureManager.Get("charactor1");
            //Gl.glEnable(Gl.GL_TEXTURE_2D);
            //Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture.ID);
            //Gl.glEnable(Gl.GL_BLEND);
            //Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA); //这样便能使用透明化(带有alpha值的)
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


        }


        public void Start()
        {
            
        }
    }
}
