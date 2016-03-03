using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;

namespace FastLoopExample
{

    public interface ImportSprite
    {
        Sprite GetSprite();
    }


    //精灵              （含有纹理，颜色，顶点等重要信息）
    public class Sprite
    {
        internal const int VertexAmount = 6;
        Vector[] _vertexPositions = new Vector[VertexAmount];        //对应两个三角形(一个矩形)
        Color[] _vertexColors = new Color[VertexAmount];
        Point[] _vertexUVs = new Point[VertexAmount];
        Texture _texture = new Texture();
        public float Uvs1=0, Uvs2=0, Uvs3=1, Uvs4=1;
        public Sprite()
        {
            InitVertexPosition(new Vector(0, 0, 0), 1, 1);
            SetColor(new Color(1, 1, 1, 1));
            SetUVs(new Point(0, 0), new Point(1, 1));         //纹理对应坐标
        }
        public Sprite(Sprite spr)                   //复制构造函数
        {
            _texture = spr.Texture;
            for (int i = 0; i < VertexAmount; i++)
            {
                _vertexPositions[i] = spr._vertexPositions[i];
                _vertexColors[i] = spr._vertexColors[i];
                _vertexUVs[i] = spr._vertexUVs[i];
            }
        }

        public Texture Texture
        {
            get { return _texture; }
            set 
            {
                _texture = value;
                InitVertexPosition(GetCenter(), _texture.Width, _texture.Height);
            }
        }

        public Vector[] VertexPositions
        {
            get { return _vertexPositions; }
        }

        public Color[] VertexColor
        {
            get { return _vertexColors; }
        }

        public Point[] VertexUVs
        {
            get { return _vertexUVs;}
        }

        public Vector GetCenter()
        {
            double halfWidth = GetWidth() / 2;
            double halHeight = GetHeight() / 2;
            return new Vector(_vertexPositions[0].X + halfWidth, _vertexPositions[0].Y - halHeight,
                _vertexPositions[0].Z);
        }

        public void InitVertexPosition(Vector position, double width, double height) //初始化顶点（根据一个中心点）
        {
            double halfWidth = width / 2;
            double halfHeight = height / 2;
            //Clockwise creation of two triangles to make a quad.

            //TopLeft , TopRight, BottomLeft (顺时针方向)
            _vertexPositions[0] = new Vector(position.X - halfWidth, position.Y + halfHeight,
                position.Z);
            _vertexPositions[1] = new Vector(position.X + halfWidth, position.Y + halfHeight,
                position.Z);
            _vertexPositions[2] = new Vector(position.X - halfWidth, position.Y - halfHeight,
                position.Z);

            //TopRight , BottomRight , BottomLeft
            _vertexPositions[3] = new Vector(position.X + halfWidth, position.Y + halfHeight,
                position.Z);
            _vertexPositions[4] = new Vector(position.X + halfWidth, position.Y - halfHeight,
                position.Z);
            _vertexPositions[5] = new Vector(position.X - halfWidth, position.Y - halfHeight,
                position.Z);
        }

        public double GetHeight()
        {
            //topleft - bottomleft
            return _vertexPositions[0].Y - _vertexPositions[2].Y;
        }

        public double GetWidth()
        {
            //topright - topleft
            return _vertexPositions[1].X - _vertexPositions[0].X;
        }

        public void SetWidth(float width)
        {
            InitVertexPosition(GetCenter(), width, GetHeight());
        }

        public void SetHeight(float height)
        {
            InitVertexPosition(GetCenter(), GetWidth(), height);
        }

        public void SetPosition(double x, double y)
        {
            SetPosition(new Vector(x, y, 0));
        }

        public void SetPosition(double x, double y, double z)
        {
            SetPosition(new Vector(x, y, z));
        }

        public void SetPosition(Vector position)  //更改坐标后，整个顶点序列都要改变(z轴是否也需要变化)
        {
            InitVertexPosition(position, GetWidth(), GetHeight());
        }

        public void SetColor(Color color)
        {
            for (int i = 0; i < Sprite.VertexAmount; i++)
            {
                _vertexColors[i] = color;
            }
        }

        public void SetColorUp(Color color)         //设置上半部分的颜色
        {
            _vertexColors[0] = color;
            _vertexColors[1] = color;
            _vertexColors[3] = color;
        }

        public void SetColorDown(Color color)
        {
            _vertexColors[2] = color;
            _vertexColors[4] = color;
            _vertexColors[5] = color;
        }
        public void SetColorLeft(Color color)
        {
            _vertexColors[2] = color;
            _vertexColors[5] = color;
            _vertexColors[0] = color;
        }
        public void SetColorRight(Color color)
        {
            _vertexColors[1] = color;
            _vertexColors[3] = color;
            _vertexColors[4] = color;
        }


        public void SetUVs(Point topLeft, Point bottomRight) 
        {
            Uvs1 = topLeft.X; Uvs2 = topLeft.Y;
            Uvs3 = bottomRight.X; Uvs4 = bottomRight.Y;
            //TopLeft , TopRight , BottomLeft
            _vertexUVs[0] = topLeft;
            _vertexUVs[1] = new Point(bottomRight.X,topLeft.Y);
            _vertexUVs[2] = new Point(topLeft.X, bottomRight.Y);

            //TopRight ,BottomRight , BottomLeft
            _vertexUVs[3] = new Point(bottomRight.X, topLeft.Y);
            _vertexUVs[4] = bottomRight;
            _vertexUVs[5] = new Point(topLeft.X, bottomRight.Y);
        }

        public void SetUVs(float type, int topleft_x, int topleft_y, int bottomright_x, int bottomright_y)
        {
            SetUVs(type * topleft_x, type * topleft_y, type * bottomright_x, type * bottomright_y);
        }
        public void SetUVs(float topleft_x, float topleft_y, float bottomright_x, float bottomright_y)
        {
            Uvs1 = topleft_x; Uvs2 = topleft_y;
            Uvs3 = bottomright_x; Uvs4 = bottomright_y;
            //TopLeft , TopRight , BottomLeft
            _vertexUVs[0].X = topleft_x;
            _vertexUVs[0].Y = topleft_y;
            _vertexUVs[1].X = bottomright_x;
            _vertexUVs[1].Y = topleft_y;
            _vertexUVs[2].X = topleft_x;
            _vertexUVs[2].Y = bottomright_y;

            //TopRight ,BottomRight , BottomLeft
            _vertexUVs[3].X = bottomright_x;
            _vertexUVs[3].Y = topleft_y;
            _vertexUVs[4].X = bottomright_x;
            _vertexUVs[4].Y = bottomright_y;
            _vertexUVs[5].X = topleft_x;
            _vertexUVs[5].Y = bottomright_y;
        }

        /// <summary>
        /// 复制一个sprite。[坐标不会被复制]
        /// </summary>
        /// <param name="spr"></param>
        /// <returns></returns>
        public static Sprite Clone(Sprite spr)
        {
            Sprite s = new Sprite();
            s.Texture = spr.Texture;
            s.SetWidth((float)spr.GetWidth());
            s.SetHeight((float)spr.GetHeight());
            Point[] vct = spr.VertexUVs;
            for (int i = 0; i < vct.Length; i++)
            {
                s.VertexUVs[i] = vct[i];
            }
            return s;
        }


        //public void SetUVs(Point topleft, Point topright, Point bottomleft, Point bottomright)
        //{
        //    _vertexUVs[0] = topleft;
        //    _vertexUVs[1] = topright; _vertexUVs[3] = topright;
        //    _vertexUVs[2] = bottomleft; _vertexUVs[5] = bottomleft;
        //    _vertexUVs[4] = bottomright;
        //}




    }
}
