using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{

    public class Effect
    {
        public Vector2D Position;
        public Sprite[] Sprites;               //仅用一次的sprite组
        public bool working = true;            //是否正常工作中
        public bool disposed = false;          //是否可以被注销
        public Sprite sprite;                  //静止的特效专用的sprite
        public MultiSprite multisprite;        //动态特效专用的multisprite
        public const float per_512 = 0.015625f, per_256 = 0.03125f;
        public virtual void Update(double elapsedTime) { }
        public virtual void Render(Renderer renderer) { }
        public virtual void Render(Renderer renderer, float x, float y) { }
        public virtual void Start() { }
    }

    public class NormalSlowEffect : Effect
    {
        double R_angle = 0;
        //允许绑定position，也可以不绑定（不绑定则需要自己指定坐标）
        public NormalSlowEffect(TextureManager texturemanager , Vector2D position = null)
        {
            Texture texture = texturemanager.Get("Ef_etama2");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(64);
            sprite.SetHeight(64);
            sprite.SetUVs(0, 14 * per_256, 8 * per_256, 22 * per_256);
            if( position!= null)
             Position = position;
        }
        public override void Start()
        {
            R_angle = 0;
        }
        public override void Update(double elapsedTime)
        {
            R_angle += elapsedTime * 60;
            if (R_angle > 360)
                R_angle -= 360;
        }
        public override void Render(Renderer renderer)
        {
            sprite.SetPosition(Position.X , Position.Y);
            renderer.DrawSprite(sprite, (float)Position.X, (float)Position.Y, (float)R_angle);
        }
        public override void Render(Renderer renderer, float x, float y)
        {
            sprite.SetPosition(x,y);
            renderer.DrawSprite(sprite, x, y, (float)R_angle);
        }

    }

    public class StartEffect_Blue : Effect
    {
        double R_angle = 0;
        //允许绑定position，也可以不绑定（不绑定则需要自己指定坐标）
        public StartEffect_Blue(TextureManager texturemanager, Vector2D position = null)
        {
            Texture texture = texturemanager.Get("etama2");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(64);
            sprite.SetHeight(64);
            sprite.SetUVs(16*per_256, 14 * per_256, 24 * per_256, 22 * per_256);
            if (position != null)
                Position = position;
        }
        public override void Start()
        {
            R_angle = 0;
        }
        public override void Update(double elapsedTime)
        {
            R_angle += elapsedTime * 60;
            if (R_angle > 360)
                R_angle -= 360;
        }
        public override void Render(Renderer renderer)
        {
            sprite.SetPosition(Position.X, Position.Y);
            renderer.DrawSprite(sprite, Position.X, Position.Y, (float)R_angle);
        }
        public override void Render(Renderer renderer, float x, float y)
        {
            sprite.SetPosition(x, y);
            renderer.DrawSprite(sprite, x, y, (float)R_angle);
        }
    }

    public class StartEffect_Red : Effect
    {
        double R_angle = 0;
        //允许绑定position，也可以不绑定（不绑定则需要自己指定坐标）
        public StartEffect_Red(TextureManager texturemanager, Vector2D position = null)
        {
            Texture texture = texturemanager.Get("etama2");
            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.SetWidth(64);
            sprite.SetHeight(64);
            sprite.SetUVs(16 * per_256, 14 * per_256, 24 * per_256, 22 * per_256);
            if (position != null)
                Position = position;
        }
        public override void Start()
        {
            R_angle = 0;
        }
        public override void Update(double elapsedTime)
        {
            R_angle += elapsedTime * 120;
            if (R_angle > 360)
                R_angle -= 360;
        }
        public override void Render(Renderer renderer)
        {
            sprite.SetPosition(Position.X, Position.Y);
            renderer.DrawSprite(sprite, Position.X, Position.Y, (float)R_angle);
        }
        public override void Render(Renderer renderer, float x, float y)
        {
            sprite.SetPosition(x, y);
            renderer.DrawSprite(sprite, x, y, (float)R_angle);
        }
    }

    public class BreakOut_Blue : Effect
    {

        float time_caculate = 0;                 //帧计时器
        float living_time = 15;                  //生存时间
        public bool working = false;                  //是否进行工作
        public bool work_over = false;                //工作是否进行完毕 
        float percent = 0;                    //执行进度百分比
        float width = 32;                      //最小宽度
        float height = 32;                     //最小高度
        float width_max = 64;                  //最大宽度
        float height_max = 64;                 //最大高度
        Vector2D Pos_BindObject;               //绑定对象上的坐标组件

        /// <summary>
        /// 爆炸特效
        /// </summary>
        /// <param name="texturemanager">纹理管理器</param>
        /// <param name="pos">绑定对象上的坐标组件</param>
        /// <param name="live_time">存活时间</param>
        /// <param name="min_r">特效半径最小大小</param>
        /// <param name="max_r">特效半径最大大小</param>
        public BreakOut_Blue(TextureManager texturemanager,Vector2D pos,int live_time = 15,float min_r=32,float max_r=64)
        {
            Pos_BindObject = pos;
            living_time = live_time;
            Texture texture = texturemanager.Get("Ef_etama2");
            sprite = new Sprite();
            sprite.Texture = texture;
            width = min_r; height = min_r;
            width_max = max_r; height_max = max_r;
            sprite.SetUVs(24 * per_256, 22 * per_256, 32 * per_256, 30 * per_256);
        }

        public override void Update(double elapsedTime)
        {
            if (!working)
                return;
            time_caculate++;
            percent = time_caculate / living_time;
            if (percent > 1)
            {
                percent = 1;
                work_over = true;
            }
        }
        public override void Render(Renderer renderer)
        {
            sprite.SetWidth(width + percent * (width_max - width));
            sprite.SetHeight(height + percent * (height_max - height));
            sprite.SetColor(new Color(1, 1, 1, 1 - percent));
            sprite.SetPosition(Pos_BindObject.X, Pos_BindObject.Y);
            renderer.DrawSprite(sprite);
        }
        public override void Render(Renderer renderer, float x, float y)
        {
            sprite.SetWidth(width + (1 - percent) * (1 - percent) * (width_max - width));
            sprite.SetHeight(height + (1 - percent)*(1 - percent) *(height_max - height));
            sprite.SetColor(new Color(1, 1, 1, 1));
            sprite.SetPosition(x, y);
            renderer.DrawSprite(sprite);
        }
    }

}
