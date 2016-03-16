using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;
using System.Runtime.InteropServices;

namespace FastLoopExample
{
    //渲染
    public class Renderer
    {
        public Renderer()
        {
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);  //透明化
        }
        public virtual void DrawImmediateModeVertex(Vector position, Color color, Point uvs)  //顶点处理
        {
            Gl.glColor4f(color.Red, color.Green, color.Blue, color.Alpha);
            Gl.glTexCoord2f(uvs.X, uvs.Y);
            Gl.glVertex3d(position.X, position.Y, position.Z);
        }
        public  void DrawSprite(Sprite sprite)
        {
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, sprite.Texture.ID); 
             //利用迭代画出每个顶点（纹理）
            Gl.glBegin(Gl.GL_TRIANGLES);
            {
                for(int i=0;i<Sprite.VertexAmount;i++)
                {
                DrawImmediateModeVertex(
                    sprite.VertexPositions[i],
                    sprite.VertexColor[i],
                    sprite.VertexUVs[i]);
                }
            }
            Gl.glEnd();
        }
        public virtual void DrawSprite3D(Sprite sprite)
        {
            //绑定3D纹理
            Gl.glBindTexture(Gl.GL_TEXTURE_3D, sprite.Texture.ID);
            //利用迭代画出每个顶点（纹理）
            Gl.glBegin(Gl.GL_TRIANGLES);
            {
                for (int i = 0; i < Sprite.VertexAmount; i++)
                {
                    DrawImmediateModeVertex(
                        sprite.VertexPositions[i],
                        sprite.VertexColor[i],
                        sprite.VertexUVs[i]);
                }
            }
            Gl.glEnd();
        }

        const float PI = 3.1415f;
        public virtual void DrawSprite(Sprite sprite ,float x, float y, float rad)
        {
            Gl.glTranslatef(x, y, 0);
            Gl.glRotatef(rad, 0, 0, 1);
            Gl.glTranslatef(-x, -y, 0);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, sprite.Texture.ID);
            //利用迭代画出每个顶点（纹理）
            Gl.glBegin(Gl.GL_TRIANGLES);
            {
                for (int i = 0; i < Sprite.VertexAmount; i++)
                {
                    DrawImmediateModeVertex(
                        sprite.VertexPositions[i],
                        sprite.VertexColor[i],
                        sprite.VertexUVs[i]);
                }
            }
            Gl.glEnd();
            Gl.glTranslatef(x, y, 0);
            Gl.glRotatef(-rad, 0, 0, 1);
            Gl.glTranslatef(-x, -y, 0);
        }
        public virtual void DrawSprite(Sprite sprite, double x, double y, float rad)
        {
            Gl.glTranslatef((float)x, (float)y, 0);
            Gl.glRotatef(rad, 0, 0, 1);
            Gl.glTranslatef(-(float)x, -(float)y, 0);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, sprite.Texture.ID);
            //利用迭代画出每个顶点（纹理）
            Gl.glBegin(Gl.GL_TRIANGLES);
            {
                for (int i = 0; i < Sprite.VertexAmount; i++)
                {
                    DrawImmediateModeVertex(
                        sprite.VertexPositions[i],
                        sprite.VertexColor[i],
                        sprite.VertexUVs[i]);
                }
            }
            Gl.glEnd();
            Gl.glTranslatef((float)x, (float)y, 0);
            Gl.glRotatef(-rad, 0, 0, 1);
            Gl.glTranslatef(-(float)x, -(float)y, 0);
        }
        public void DrawString(string str)
        {
            int lists;
            


        }


        /*  使用 batch （批）   */
        /*
        Batch _batch = new Batch();
        public void DrawSprite(Sprite sprite)
        {
            _batch.AddSprite(sprite);
        }
        public void Render()
        {
            _batch.Draw();
        }
        */
    }

    //对象坐标含有偏移的着色器
    public class OffsetRenderder : Renderer
    {

        double dx, dy ,dz;
        public double OffsetX
        {
            get { return dx; }
            set { dx = value; }
        }
        public double OffsetY
        {
            get { return dy; }
            set { dy = value; }
        }
        public double OffsetZ
        {
            get { return dz; }
            set { dz = value; }
        }

        public OffsetRenderder(double x=0, double y=0 , double z=0)
        {
            OffsetX = x;
            OffsetY = y;
            OffsetZ = z;
        }

        //重载点描述，使它渲染时产生偏移
        public override void DrawImmediateModeVertex(Vector position, Color color, Point uvs)
        {
            Gl.glColor4f(color.Red, color.Green, color.Blue, color.Alpha);
            Gl.glTexCoord2f(uvs.X, uvs.Y);
            Gl.glVertex3d(position.X+dx, position.Y+dy, position.Z+dz);
        }

        public override void DrawSprite(Sprite sprite, float x, float y, float rad)
        {
            x += (float)OffsetX;
            y += (float)OffsetY;
            Gl.glTranslatef(x, y, 0);
            Gl.glRotatef(rad, 0, 0, 1);
            Gl.glTranslatef(-x, -y, 0);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, sprite.Texture.ID);
            //利用迭代画出每个顶点（纹理）
            Gl.glBegin(Gl.GL_TRIANGLES);
            {
                for (int i = 0; i < Sprite.VertexAmount; i++)
                {
                    DrawImmediateModeVertex(
                        sprite.VertexPositions[i],
                        sprite.VertexColor[i],
                        sprite.VertexUVs[i]);
                }
            }
            Gl.glEnd();
            Gl.glTranslatef(x, y, 0);
            Gl.glRotatef(-rad, 0, 0, 1);
            Gl.glTranslatef(-x, -y, 0);
        }

        public override void DrawSprite(Sprite sprite, double _x, double _y, float rad)
        {
            float x = (float)_x;
            float y = (float)_y;

            x += (float)OffsetX;
            y += (float)OffsetY;
            Gl.glTranslatef(x, y, 0);
            Gl.glRotatef(rad, 0, 0, 1);
            Gl.glTranslatef(-x, -y, 0);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, sprite.Texture.ID);
            //利用迭代画出每个顶点（纹理）
            Gl.glBegin(Gl.GL_TRIANGLES);
            {
                for (int i = 0; i < Sprite.VertexAmount; i++)
                {
                    DrawImmediateModeVertex(
                        sprite.VertexPositions[i],
                        sprite.VertexColor[i],
                        sprite.VertexUVs[i]);
                }
            }
            Gl.glEnd();
            Gl.glTranslatef(x, y, 0);
            Gl.glRotatef(-rad, 0, 0, 1);
            Gl.glTranslatef(-x, -y, 0);
        }


    }


}
