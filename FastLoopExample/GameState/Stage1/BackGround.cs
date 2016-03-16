using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;

namespace FastLoopExample.GameState.Stage1
{
    public class BackGround : IGameObject,RenderLevel
    {

        int level;

        public Renderer renderer;
        TextureManager texturemanager;
        Sprite background;

        BackGroundImage backgroundimage;

        List<Tree> tree;

        Particle.Particles particles;            //粒子
        bool allowrunParticles = false;          //是否能启用擦弹粒子效果
        List<Particle.Particles> ParticlesList = new List<Particle.Particles>();     //粒子序列

        public BackGround(TextureManager _texturemanager, int level, Particle.Particles _p = null)
        {

            if (_p != null)
            {
                particles = _p;
                allowrunParticles = true;
            }

            this.level = level;
            renderer = Stage1State._renderer;
            this.texturemanager = _texturemanager;

            Texture texture = _texturemanager.Get("shade1");
            background = new Sprite();
            background.Texture = texture;
            background.SetPosition(0, 225);
            background.SetHeight(450);
            background.SetWidth(385+10);

            tree = new List<Tree>();

            //添加一开始就有的树
            for (int i = 0; i < 3; i++)
            {
                backgroundimage = new BackGroundImage(_texturemanager, renderer, "stg1bg", new Rectangle(0.01f, 0.01f, 0.48f, 0.48f), 385+10, 450);
                SpeedY = 0.05f;
                backgroundimage.SetPosition(0, 225);

                Tree tree1 = new Tree(texturemanager, renderer, "stg1bg", false, 200, 200);
                tree1.setTop(400);
                tree1.setPosition(16 + 30 * (float)random.NextDouble() - 15, i*200 + 60 * (float)random.NextDouble() - 30);
                tree.Add(tree1);

                tree1 = new Tree(texturemanager, renderer, "stg1bg", true, 200, 200);
                tree1.setTop(400);
                tree1.setPosition(-100 + 30 * (float)random.NextDouble() - 15, i * 200 + 100 * (float)random.NextDouble() - 50);
                tree.Add(tree1);

                tree1 = new Tree(texturemanager, renderer, "stg1bg", true, 200, 200);
                tree1.setTop(400);
                tree1.setPosition(160 + 30 * (float)random.NextDouble() - 15, i * 200 + 60 * (float)random.NextDouble() - 30);
                tree.Add(tree1);
            }

           
        }

        public void BindParticle(Particle.Particles p)
        {
            ParticlesList.Add(p);
        }

        public void Start()
        {
            backgroundimage.Start();
        }

        Random random = new Random();
        List<Tree> ToRemove = new List<Tree>();
        double move_y =0 ;
        double each_m = 120;
        int current_index = 0;          //根据编号排位生成树的位置
        public void Update(double elapsedTime)
        {
            if (allowrunParticles)
            {
                particles.Update(elapsedTime);
            }
            foreach (Particle.Particles p in ParticlesList)
            {
                p.Update(elapsedTime);
            }

            move_y += elapsedTime * SpeedY*680;

            
            if (move_y > each_m)
            {
                move_y = 0;

                
                switch (current_index)
                {
                    case 0:
                        Tree Tr = new Tree(texturemanager, renderer, "stg1bg", false, 200, 200);
                         Tr = new Tree(texturemanager, renderer, "stg1bg", false, 200, 200);
                         Tr.setTop(400);
                         Tr.setPosition(31, 430);
                         tree.Add(Tr);
                         Tr = new Tree(texturemanager, renderer, "stg1bg", true, 200, 200);
                         Tr.setTop(400);
                         Tr.setPosition(-115, 450);
                         tree.Add(Tr);
                         Tr = new Tree(texturemanager, renderer, "stg1bg", true, 200, 200);
                         Tr.setTop(400);
                         Tr.setPosition(175, 405);
                         tree.Add(Tr);
                        current_index++;
                        break;
                    case 1:
                        Tree Tr1 = new Tree(texturemanager, renderer, "stg1bg", false,200,200);
                        Tr1.setTop(400);
                        Tr1.setPosition(60, 430);
                        tree.Add(Tr1);
                        Tr1 = new Tree(texturemanager, renderer, "stg1bg", true, 200, 200);
                        Tr1.setTop(400);
                        Tr1.setPosition(-75, 430);
                        tree.Add(Tr1);
                        Tr1 = new Tree(texturemanager, renderer, "stg1bg", true, 200, 200);
                        Tr1.setTop(400);
                        Tr1.setPosition(155, 418);
                        tree.Add(Tr1);
                        current_index++;
                        break;
                    case 2:
                        Tree Tr2 = new Tree(texturemanager, renderer, "stg1bg", false,200,200);
                        Tr2.setTop(400);
                        Tr2.setPosition(10, 430);
                        tree.Add(Tr2);
                        Tr2 = new Tree(texturemanager, renderer, "stg1bg", true, 200, 200);
                        Tr2.setTop(400);
                        Tr2.setPosition(-100, 425);
                        tree.Add(Tr2);
                        Tr2 = new Tree(texturemanager, renderer, "stg1bg", true, 200, 200);
                        Tr2.setTop(400);
                        Tr2.setPosition(165, 408);
                        tree.Add(Tr2);
                        current_index++;
                        break;
                    case 3:
                        Tree Tr3 = new Tree(texturemanager, renderer, "stg1bg", false,200,200);
                        Tr3.setTop(400);
                        Tr3.setPosition(-10, 410);
                        tree.Add(Tr3);
                        Tr3 = new Tree(texturemanager, renderer, "stg1bg", true, 200, 200);
                        Tr3.setTop(400);
                        Tr3.setPosition(-120, 435);
                        tree.Add(Tr3);
                        Tr3 = new Tree(texturemanager, renderer, "stg1bg", true, 200, 200);
                        Tr3.setTop(400);
                        Tr3.setPosition(135, 428);
                        tree.Add(Tr3);
                        current_index = 0;
                        break;
                }

                //Tree tree1 = new Tree(texturemanager, renderer, "stg1bg", false,200,200);
                //tree1.setTop(400);
                //tree1.setPosition(16+ 30*(float)random.NextDouble() - 15, 400+60*(float)random.NextDouble() - 30);
                //tree.Add(tree1);

                //tree1 = new Tree(texturemanager, renderer, "stg1bg", true, 200, 200);
                //tree1.setTop(400);
                //tree1.setPosition(-100 + 30 * (float)random.NextDouble() - 15, 400 + 100 * (float)random.NextDouble() - 50);
                //tree.Add(tree1);

                //tree1 = new Tree(texturemanager, renderer, "stg1bg", true, 200, 200);
                //tree1.setTop(400);
                //tree1.setPosition(160 + 30 * (float)random.NextDouble() - 15, 400 + 10 * (float)random.NextDouble() - 5);
                //tree.Add(tree1);

            }
            SpeedY = SpeedY;


            backgroundimage.Update(elapsedTime);

            foreach (Tree t in tree)
                t.Update(elapsedTime);

            for (int i = 0; i < tree.Count; i++)
            {
                if (tree[i].Y < -150)
                    ToRemove.Add(tree[i]);
            }
            for (int i = 0; i < ToRemove.Count; i++)
            {
                tree.Remove(ToRemove[i]);
            }
            ToRemove.Clear();

        }

        public float SpeedY
        {
            get { return backgroundimage.SpeedY; }
            set {
                backgroundimage.SpeedY = value;
                foreach (Tree t in tree)
                {
                    t.SpeedY = value * 680;
                }
            }
        }

        public void Render()
        {
            
            backgroundimage.Render();
            foreach (Tree t in tree)
                t.Render();
            if(allowrunParticles)
                particles.Render(Stage1State._renderer);
            renderer.DrawSprite(background);

            foreach (Particle.Particles p in ParticlesList)
            {
                p.Render(Stage1State._renderer);
            }
        }

        public int getLevel()
        {
            return level;
        }

        public void setLevel(int i)
        {
            level = i;
        }

        public void RenderByLevel()
        {
            Render();
        }
    }

    class Rectangle
    {
        float  x, y, width, height;
        public Rectangle(float x, float y, float w, float h)
        {
            this.x = x; this.y = y; this.width = w; this.height = h;
        }
        public float X { get { return x; } set { x = value; } }
        public float Y { get { return y; } set { y = value; } }
        public float Width { get { return width; } set { width = value; } }
        public float Height { get { return height; } set { height = value; } }
    }

    class BackGroundImage : IGameObject
    {
        private float speed_x;                //x轴上的移动速度( percent / s)
        private float speed_y;                //y轴上的移动速度
        private float x, y , width , height;
        private Sprite map_up , map_down;
        private Renderer renderer;
        private bool needloop = false;
        private Rectangle current_rect;       //当前的作画区间(相对于图像的)
        private Rectangle UVs;                //图像映射对应原图像的区域（Uvs区域）

        public float X
        {
            get { return x; }
            set { x = value; }
        }
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        public void SetPosition(float x, float y)
        {
            X = x; Y = y;
            map_up.SetPosition(X, Y);
        }
        
        TextureManager texturemanager;        //纹理管理器
        public BackGroundImage(TextureManager _t ,Renderer _renderer, string TextureName ,
           Rectangle UVs, float width = 385, float height = 450)
        {
            texturemanager = _t;
            renderer = _renderer;
            current_rect = new Rectangle(0.1f, 0, 0.8f, 0.8f);
            this.UVs = UVs;
            this.width = width; this.height = height;

            map_up = new Sprite();
            map_up.Texture = texturemanager.Get(TextureName);
            map_up.SetWidth(width);
            map_up.SetHeight(height);
            map_up.SetPosition(X, Y);
            map_up.SetUVs(new Point(current_rect.X , current_rect.Y), 
                new Point(current_rect.X+current_rect.Width, current_rect.Y+ current_rect.Height));

            map_down = new Sprite();
            map_down.Texture = texturemanager.Get(TextureName);
            map_down.SetWidth(width);
            map_down.SetHeight(height);
            map_down.SetPosition(X, Y);
            map_down.SetUVs(new Point(current_rect.X, current_rect.Y),
                new Point(current_rect.X + current_rect.Width, current_rect.Y + current_rect.Height));

        }

        public float SpeedX
        {
            get { return speed_x; }
            set { speed_x = value; }
        }
        public float SpeedY
        {
            get { return speed_y; }
            set { speed_y = value; }
        }

        public void Start()
        {
          
        }

        public void Update(double elapsedTime)
        {

            double move_y = current_rect.Y  + speed_y * elapsedTime;

            if (move_y+current_rect.Height <= 1)
            {
                if (needloop == true)
                {
                    map_up.SetPosition(X, Y);
                    map_up.SetHeight(height);
                }

                current_rect.Y = (float)move_y;

                //实际纹理坐标映射
                float _x = UVs.X + current_rect.X * UVs.Width;
                float _y = UVs.Y + current_rect.Y * UVs.Height;
                float _w = UVs.Width * current_rect.Width;
                float _h = UVs.Height * current_rect.Height;

                map_up.SetUVs(new Point(_x + _w, _y + _h), new Point(_x, _y));
                map_up.SetColorUp(new Color(0, 0, 0, 0));
                map_up.SetColorDown(new Color(1, 1, 1, 1));

                needloop = false;
            }
            else if (move_y >= 1)         //完全到底部时候
            {
                if (needloop == true)
                {
                    map_up.SetPosition(X, Y);
                    map_up.SetHeight(height);
                }
                  
                move_y = 1 - move_y;

                current_rect.Y = (float)move_y;
                //实际纹理坐标映射
                float _x = UVs.X + current_rect.X * UVs.Width;
                float _y = UVs.Y + current_rect.Y * UVs.Height;
                float _w = UVs.Width * current_rect.Width;
                float _h = UVs.Height * current_rect.Height;

                map_up.SetUVs(new Point(_x + _w, _y + _h), new Point(_x, _y));
                map_up.SetColorUp(new Color(0, 0, 0, 0));
                map_up.SetColorDown(new Color(1, 1, 1, 1));

                needloop = false;
            }
            else                           //位于两者之间时，需要特殊处理，包括对坐标的处理以及颜色渐变的处理
            {
                current_rect.Y = (float)move_y;
                float x1 = current_rect.X;
                float _x1_width = current_rect.Width;
                float y1 = current_rect.Y;
                float _y1_height = 1 - current_rect.Y;
                map_up.SetPosition(X, height * _y1_height /current_rect.Height * 0.5f);
                map_up.SetHeight(height * _y1_height / current_rect.Height);
                float x2 = current_rect.X;
                float _x2_width = current_rect.Width;
                float y2 = 0;
                float _y2_height = current_rect.Y + current_rect.Height - 1;
                map_down.SetPosition(X, height * (current_rect.Height - _y2_height) 
                    / current_rect.Height + height * _y2_height / current_rect.Height * 0.5f);
                map_down.SetHeight(height * _y2_height / current_rect.Height);
                float percent = _y2_height / current_rect.Height;
                map_down.SetColorUp(new Color(0, 0, 0, 0));
                map_down.SetColorDown(new Color(percent, percent, percent, percent));
                map_up.SetColorUp(new Color(percent, percent, percent, percent));
                map_up.SetColorDown(new Color(1, 1, 1, 1));
                float _x = UVs.X + x1 * UVs.Width;
                float _y = UVs.Y + y1 * UVs.Height;
                float _w = UVs.Width * _x1_width;
                float _h = UVs.Height * _y1_height;
                map_up.SetUVs(new Point(_x + _w, _y + _h), new Point(_x, _y));
                _x = UVs.X + x2 * UVs.Width;
                _y = UVs.Y + y2 * UVs.Height;
                _w = UVs.Width * _x2_width;
                _h = UVs.Height * _y2_height;
                map_down.SetUVs(new Point(_x + _w, _y + _h), new Point(_x, _y));
                needloop = true;
            }
           

        }

        public void Render()
        {
            if (!needloop)
                renderer.DrawSprite(map_up);
            else
            {
                renderer.DrawSprite(map_up);
                renderer.DrawSprite(map_down);
            }
        }
    }
    class Leaf
    {
        public Sprite leaf;
        public Vector2D position;
        public Leaf()
        {
            position = new Vector2D();
            leaf = new Sprite();
        }
        public void SetPosition(float x, float y)
        {
            position.X = x;
            position.Y = y;
            leaf.SetPosition(x, y);
        }
    }
    class Tree :IGameObject
    {
        public float X, Y;                //树所在的相对坐标
        public float Ori_X =0, Ori_Y=0;        //树为参照的原点
        private float speed_x;                //x轴上的移动速度( percent / s)
        private float speed_y;                //y轴上的移动速度
        private float width, height;          //宽和高(以最底层的为基础，为最大状态时的大小)
        private float top = 450;              //设置一个顶部限制
        private float left = 192;             //设置一个横侧限制
        private float min_percent = 0.6f;     //当坐标处于顶部时的减小百分比
        private const float topleaf = 0.4f, middleleaf = 0.6f, bottomleaf = 1;
        static Random random;

        TextureManager texturemanager;
        Renderer renderer;
        Leaf[] leaf ;                    // 0 最顶层 ， 1中间层 ， 2最底层

        public float SpeedX
        {
            get { return speed_x; }
            set { speed_x = value; }
        }
        public float SpeedY
        {
            get { return speed_y; }
            set { speed_y = value; }
        }

        public Tree(TextureManager _t , Renderer _r , string name , bool isleft = true,float w = 150 ,float h= 150, float orix = 0, float oriy = 0)
        {
            if (random == null)
                random = new Random();
            texturemanager = _t;
            renderer = _r;
            width = w; height = h; 
            leaf = new Leaf[3];
            Texture texture = texturemanager.Get(name);
            for (int i = 0; i < 3; i++)
            {
                leaf[i] = new Leaf();
                leaf[i].leaf = new Sprite();

                leaf[i].leaf.Texture = texture;

                if (isleft)
                    leaf[i].leaf.SetUVs(0.51f, 0, 0.74f, 0.24f);
                else
                    leaf[i].leaf.SetUVs(0.75f, 0, 0.99f, 0.24f);
                leaf[i].leaf.SetHeight(h);
                leaf[i].leaf.SetWidth(h);
            }

        }

        public void setPosition(float x, float y)
        {
            X = x;  Y = y;
        }
        public void setOriPosition(float x, float y)
        {
            Ori_X = x; Ori_Y = y;
        }
        public void setTop(float t)
        {
            top = t;
        }
        public void setMinPercent(float p)
        {
            if (p >= 0 && p <= 1)
                min_percent = p;
        }


        public void Start()
        {
          
        }

        public void Update(double elapsedTime)
        {
            float move_y = speed_y * (float)elapsedTime;
            Y = Y - move_y;

            if (Y > top)            //在顶部以上时，显示最小状态
            {
                setLeafs(min_percent);
            }
            else if (Y >= 0)        //靠近 y = 0 时，逐渐变大
            {
                float percent = ((top - Y) / top) * (1 - min_percent) + min_percent;  
                setLeafs(percent);
            }
            else                  //其它状态时恢复正常大小
            {
                setLeafs(1);      //1倍缩放 ， 保持原型
            }

            //处理每块树叶的坐标信息
            DealWithPosition(elapsedTime);



        }

        //设置每块树叶的大小
        void setLeafs(float percent)
        {
            leaf[0].leaf.SetWidth(percent * width * topleaf);
            leaf[0].leaf.SetHeight(percent * height * topleaf);
            leaf[1].leaf.SetWidth(percent * width * middleleaf);
            leaf[1].leaf.SetHeight(percent * height * middleleaf);
            leaf[2].leaf.SetWidth(percent * width * bottomleaf);
            leaf[2].leaf.SetHeight(percent * height * bottomleaf);

        }
        //设置每块树叶的坐标（产生立体效果）
        void DealWithPosition(double elapsedTime)
        {
            leaf[2].SetPosition(X, Y);

            float x0 = X, y0 = 0, x1 = X, y1 = 0;
            float per = ((top - Y) / top) * ((top - Y) / top);
            float percent = 0.3f - per * 0.1f;

            if (per > 1)
                per = 1;

            if (X < 0)
            {
                X -= width*0.02f * per * (float)elapsedTime;
            }
            else if (X > 0)
            {
                X += width * 0.02f * per * (float)elapsedTime;
            }
            per = per * ((top - Y) / top); 

            //修改Y值坐标
            if (Y > 0)
            {
                y1 = Y + height * bottomleaf * percent;
                y0 = y1 + height * middleleaf * percent;
            }
            else
            {
                y1 = Y + height * bottomleaf * percent;
                y0 = y1 + height * middleleaf * percent;
            }

            //修改X值坐标
            if (X < 0)
            {
                float distance = -X;
                float p = distance / left;
                x1 = X - width * middleleaf * p * per*0.3f; ;
                x0 = x1 - width * topleaf * p * per * 0.3f; ;
            }
            else if (X > 0)
            {
                float distance = X;
                float p = distance / left;
                x1 = X + width * middleleaf * p * per * 0.3f;
                x0 = x1 + width * topleaf * p * per * 0.3f;
            }

            per = ((top - Y) / (top/1.5f));
            if (per > 1) per = 1;
            leaf[0].leaf.SetColor(new Color(1, 1, 1, 0.7f*per));
            leaf[1].leaf.SetColor(new Color(1, 1, 1, 0.6f*per));
            leaf[2].leaf.SetColor(new Color(1, 1, 1, 0.5f*per));

            leaf[1].SetPosition(x1, y1);
            leaf[0].SetPosition(x0, y0);
        }

        public void Render()
        {
            for (int i = 2; i >= 0; i--)
            {
                renderer.DrawSprite(leaf[i].leaf);
            }
        }
    }


    class BackGroundImage3D : IGameObject
    {
        private float speed_x;                //x轴上的移动速度( percent / s)
        private float speed_y;                //y轴上的移动速度
        private float x, y, width, height;
        private Renderer renderer;
        private bool needloop = false;
        List<Sprite> Sprites;                 //纹理队列

        public float X
        {
            get { return x; }
            set { x = value; }
        }
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        TextureManager texturemanager;        //纹理管理器
        public BackGroundImage3D(TextureManager _t, Renderer _renderer, string TextureName, float width = 385, float height = 450)
        {
            texturemanager = _t;
            renderer = _renderer;
            this.width = width; this.height = height;

            Sprite sp;

            sp = new Sprite();
            sp.Texture = texturemanager.Get(TextureName);
            sp.SetWidth(width);
            sp.SetHeight(height);
            sp.SetPosition(X, Y);

            Sprites.Add(sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get(TextureName);
            sp.SetWidth(width);
            sp.SetHeight(height);
            sp.SetPosition(X, Y);

            Sprites.Add(sp);

        }

        public float SpeedX
        {
            get { return speed_x; }
            set { speed_x = value; }
        }
        public float SpeedY
        {
            get { return speed_y; }
            set { speed_y = value; }
        }

        public void Start()
        {

        }

        public void Update(double elapsedTime)
        {

        }

        public void Render()
        {
            
        }
    }

    /// <summary>
    /// 循环片段
    /// </summary>
    class Image3D
    {

    }




}
