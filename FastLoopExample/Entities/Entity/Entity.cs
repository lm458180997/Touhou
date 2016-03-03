using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{

    //所有实体类的基  entity
    public abstract class Entity:IGameObject
    {
        //坐标
        public Vector2D Position = new Vector2D();
        //运动方向
        public Vector2D Direction = new Vector2D(); 
        //旋转方向
        public Vector2D Rotation = new Vector2D();
        //缩放
        public Vector2D Scale = new Vector2D();
        //宽与高
        public float Width;
        public float Height;
        public Sprite sprite;
        public virtual void Start(){ }
        public virtual void Update(double elapsedTime) { }
        public virtual void Render(){}
    }


    // 碰撞体结构  （含有碰撞判定的物体都必须实现此接口）  ， 
    // 碰撞判定的时候，尽量是用子弹来完成与玩家（敌人）的判定, 而非玩家来指向子弹，
    // 因为玩家和敌人的碰撞判定是比较简单的， 这样可以提高效率
    public interface Collider 
    {
        bool Collision(Collider c);           //判断碰撞体之间的碰撞
    }

    public interface RectCollider : Collider   //矩形碰撞体，所不同的是它含有了坐标以及大小属性
    {
        double GetX();
        double GetY();
        double GetWidth();
        double GetHeight();
    }

    public interface RoundCollider : Collider   //圆形碰撞体，所不同的是它含有了具有圆的属性
    {
        double GetX();
        double GetY();
        double GetRadius();
    }

    public struct RecTangleF
    {
        public float left, top, right, bottom;
        public RecTangleF(float l, float t, float r, float b)
        {
            left = l; top = t; right = r; bottom = b;
        }

    }

    //动画纹理
    public class MultiSprite 
    {

        int state = -1;         //所处状态（根据不同类型的有可能会出现多种状态的纹理） ， -1时表示不使用

        int selectedindex = 0;        //当前所选择的rectangle项索引值

        public Sprite sprite;     //核心Sprite ， 所有的动画都是从中获取

        public Sprite[] sprites;  //核心Sprite序列，动画所同时需要的一个精灵列表

        public double changeinterval = 0.15f;  //切换速度（每间隔多少可以更换一张图片）

        public bool ManySprites = false;              //是否含有多个精灵   

        Dictionary<int, RecTangleF[]> SpritesUvs;     //根据不同的状态，设定不同的Uvs组

        Dictionary<int, int> AniCollection = new Dictionary<int,int>();           //动画连接

        double time_caculate = 0;
        double lasttime = 0;

        public MultiSprite(Sprite sprite)
        {
            this.sprite = sprite;
            SpritesUvs = new Dictionary<int, RecTangleF[]>();
        }

        public MultiSprite(Sprite[] sprites)
        {
            this.sprites = sprites;
            this.sprite = sprites[0];
            SpritesUvs = new Dictionary<int, RecTangleF[]>();
            ManySprites = true;
        }

        public int State
        {
            get { return state; }
            set 
            {
                selectedindex = 0;
                if (SpritesUvs.Keys.Contains(value))
                {
                    state = value;
                    RecTangleF rect = SpritesUvs[state][selectedindex];
                    if (ManySprites)
                    {
                        for (int i = 0; i < sprites.Length; i++)
                        {
                            sprites[i].SetUVs(rect.left, rect.top, rect.right, rect.bottom);
                        }
                    }
                    else
                            sprite.SetUVs(rect.left, rect.top, rect.right, rect.bottom);
                }
                else
                {
                    state = -1;
                    throw new Exception("动画纹理序列中不存在此State!");
                }
            }
        }

        //注册状态
        public void RegeditState(int state, RecTangleF[] rect)
        {
            SpritesUvs.Add(state, rect);
        }
        //注册连接
        public void RegeditCollection(int fore, int next)
        {
            AniCollection.Add(fore, next);
        }


        //逻辑更新
        public void Update(double elapsedTime)
        {
            if (state == -1)       //状态为-1的时候表示停用
                return;
            time_caculate += elapsedTime;
            if (time_caculate - lasttime >= changeinterval)
            {
                lasttime += changeinterval;
                selectedindex++;                           //索引值前翻，顺序播放纹理
                if (selectedindex >= SpritesUvs[state].Length) 
                {
                    selectedindex = 0;
                    if (AniCollection.Keys.Contains(state))
                    {
                        State = AniCollection[state];
                    }
                }
                RecTangleF rect = SpritesUvs[state][selectedindex];
                if (ManySprites)
                {
                    for (int i = 0; i < sprites.Length; i++)
                    {
                        sprites[i].SetUVs(rect.left, rect.top, rect.right, rect.bottom);
                    }
                }
                else
                   sprite.SetUVs(rect.left, rect.top, rect.right, rect.bottom);
            }

        }

    }




}
