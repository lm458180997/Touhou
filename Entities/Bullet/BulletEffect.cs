using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Entities
{

    public class RushBlueEffect : Effect
    {
        //width
        float width;
        //height
        float height;
        //当前动画播放到的索引值
        int Index = 0;
        //当前动画所要面向的方向的偏移值
        float OffsetAngle = 0;
        //帧之间的间隔时间
        double timeInterval = 0.03;
        //生命周期计时器
        double timecaculate = 0;
        public RushBlueEffect(TextureManager _t, Vector2D Pos,float offsetangle,float w = 16,float h=16)
        {
            Position = Pos;
            OffsetAngle = offsetangle;
            Sprites = new Sprite[9];
            width = w;
            height = h;
            initTextures(_t);
        }
        void initTextures(TextureManager texturemanager)
        {
            string head = "RushBlue_Reimu_Effect";
            string middle = "";
            string num;
            for (int i = 1; i < 10; i++)
            {
                num = i.ToString();
                if (num.Length == 2)
                {
                    middle = "0" + num;
                }
                else
                    middle = "00" + num;
                int index = i - 1;
                Sprites[index] = new Sprite();
                Sprites[index].Texture = texturemanager.Get(head + middle);
                Sprites[index].SetWidth(width);
                Sprites[index].SetHeight(height);
                Sprites[index].SetUVs(0, 0, 1, 1);
            }
        }
        public override void Update(double ElapsedTime)
        {
            if (working)
            {
                timecaculate += ElapsedTime;
                if (timecaculate > timeInterval)
                {
                    timecaculate -= timeInterval;
                    Index++;
                }
                if (Index > 8)
                {
                    working = false;
                    disposed = true;
                    Index = 8;
                }
            }
        }
        public override void Render(Renderer renderer)
        {
            Sprite sp = Sprites[Index];
            sp.SetPosition(Position.X, Position.Y);
            sp.SetColor(new Color(1, 1, 1, 0.5f));
            renderer.DrawSprite(sp, Position.X, Position.Y, OffsetAngle + 90);
        } 
    }

    public class RushRedEffect : Effect
    {
        //width
        float width;
        //height
        float height;
        //当前动画播放到的索引值
        int Index = 0;
        //当前动画所要面向的方向的偏移值
        float OffsetAngle = 0;
        //帧之间的间隔时间
        double timeInterval = 0.03;
        //生命周期计时器
        double timecaculate = 0;
        public RushRedEffect(TextureManager _t, Vector2D Pos, float offsetangle, float w = 16, float h = 16)
        {
            Position = Pos;
            OffsetAngle = offsetangle;
            Sprites = new Sprite[9];
            width = w;
            height = h;
            initTextures(_t);
        }
        void initTextures(TextureManager texturemanager)
        {
            string head = "RushRed_Reimu_EF";
            string middle = "";
            string num;
            for (int i = 0; i < 9; i++)
            {
                num = i.ToString();
                if (num.Length == 2)
                {
                    middle = "0" + num;
                }
                else
                    middle = "00" + num;
                Sprites[i] = new Sprite();
                Sprites[i].Texture = texturemanager.Get(head + middle);
                Sprites[i].SetWidth(width);
                Sprites[i].SetHeight(height);
            }
        }
        public override void Update(double ElapsedTime)
        {
            if (working)
            {
                timecaculate += ElapsedTime;
                if (timecaculate > timeInterval)
                {
                    timecaculate -= timeInterval;
                    Index++;
                }
                if (Index > 8)
                {
                    working = false;
                    disposed = true;
                    Index = 8;
                }
            }
        }
        public override void Render(Renderer renderer)
        {
            Sprite sp = Sprites[Index];
            sp.SetPosition(Position.X, Position.Y);
            sp.SetColor(new Color(1, 1, 1, 0.5f));
            renderer.DrawSprite(sp, Position.X, Position.Y, OffsetAngle + 90);
        }
    }

    public class WindBlueEffect : Effect
    {
        //width
        float width;
        //height
        float height;
        //当前动画播放到的索引值
        int Index = 0;
        //当前动画所要面向的方向的偏移值
        float OffsetAngle = 0;
        //帧之间的间隔时间
        double timeInterval = 0.02;
        //生命周期计时器
        double timecaculate = 0;
        public WindBlueEffect(TextureManager _t, Vector2D Pos, float offsetangle, float w = 16, float h = 16)
        {
            Position = Pos;
            OffsetAngle = offsetangle;
            Sprites = new Sprite[18];
            width = w;
            height = h;
            initTextures(_t);
        }
        void initTextures(TextureManager texturemanager)
        {
            string head = "WindBlue_Reimu_Ef";
            string middle = "";
            string num;
            for (int i = 0; i < 18; i++)
            {
                num = i.ToString();
                if (num.Length == 2)
                {
                    middle = "0" + num;
                }
                else
                    middle = "00" + num;
                Sprites[i] = new Sprite();
                Sprites[i].Texture = texturemanager.Get(head + middle);
                Sprites[i].SetWidth(width);
                Sprites[i].SetHeight(height);
            }
        }
        public override void Update(double ElapsedTime)
        {
            if (working)
            {
                timecaculate += ElapsedTime;
                if (timecaculate > timeInterval)
                {
                    timecaculate -= timeInterval;
                    Index++;
                }
                if (Index > 17)
                {
                    working = false;
                    disposed = true;
                    Index = 17;
                }
            }
        }
        public override void Render(Renderer renderer)
        {
            Sprite sp = Sprites[Index];
            sp.SetPosition(Position.X, Position.Y);
            sp.SetColor(new Color(1, 1, 1, 0.4f));
            renderer.DrawSprite(sp, Position.X, Position.Y, OffsetAngle );
        } 
    }


}
