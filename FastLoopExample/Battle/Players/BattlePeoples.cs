using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Battle.Players
{

    //状态说明：
    //状态分为多种状态，一种为行走状态，一种为攻击状态
    //所有的逻辑都是根据状态的组合来决定。  
    //例如 running + ATK_Normal , 解释为人为跑动时触发的攻击

    /// <summary>
    /// 人物的行走状态
    /// </summary>
    public enum BP_MoveState
    {
        Ready,              //就绪状态
        Walking,            //行走状态
        Running,            //跑动状态
        Jumping_Walking,    //行走中的跳跃
        Running_Walking,    //跑动中的跳跃
        Lay,                //倒地状态
    }
    /// <summary>
    /// 人物的攻击状态
    /// </summary>
    public enum BP_ATKState
    {
        None,            //无攻击态
        ATK_Normal,      //普通攻击态
    }

    //战斗人物
    public class BattlePeoples : IGameObject
    {
        public Vector2D Position;               //人物的坐标
        public Vector2D Direction;              //人物的朝向   

        public TextureManager texturemanager;   //纹理管理器
        public const float per_512 = 0.015625f, per_256 = 0.03125f;       //用于将数据百分比化，从而便于制作渲染精灵
        protected string name = "default";       //人物的名字（便于在众多对象中需找到指定对象）
        public int level = 0;                    //人物的使用等级
        public float attack = 10;                //人物本身的攻击力（暂且归纳为基础攻击力）
        public float fastSpeed = 350;            //跑动时的速度
        public float slowSpeed = 175;            //走动时的速度
        public int Level = 0;                    //强化等级

        public Renderer renderer= new Renderer();                //一般着色器（中心点居中）

        public BattleAnimation animation;   //当前显示的动画(对应的纹理数组)

        //攻击组件（每一个组件都能对敌人具有技能判定）
        public List<CollisionComponent> AttackComponents = new List<CollisionComponent>();  //攻击组件
        public List<CollisionComponent> AttackComponents_ToRemove = new List<CollisionComponent>(); //组件删除缓冲池

        public virtual void Start()
        {
        }

        public virtual void Update(double elapsedTime)
        {
        }

        /// <summary>
        /// 攻击的判定
        /// </summary>
        /// <param name="p">另一个Player</param>
        /// <param name="msg">信息管理机</param>
        /// <returns>是否有满足判定的实体存在</returns>
        public virtual bool AttackCollision(Player p, ref MessageManager msg)
        {
            return false;
        }
        /// <summary>
        /// 攻击的判定
        /// </summary>
        /// <param name="b">另一个Player的飞行道具</param>
        /// <param name="msg">信息管理机</param>
        /// <returns>是否有满足判定的实体存在</returns>
        public virtual bool AttackCollision(Bullet b, ref MessageManager msg)
        {
            return false;
        }

        public virtual void Render()
        {
        }
    }

    //蕾米利亚
    public class Remilia : BattlePeoples
    {
        /// <summary>
        /// 蕾米利亚
        /// </summary>
        /// <param name="t">纹理机</param>
        public Remilia(TextureManager t)
        {
            texturemanager = t;
            Position = new Vector2D(0, 0);
            Direction = new Vector2D(1, 0);  //默认朝右边

            //暂时只运行站立动画
            animation = new Battle.CharactorsAnimations.remilia_stand(t);
        }

        void InitTextures()
        {
            
        }

        public override void Update(double elapsedTime)
        {
            base.Update(elapsedTime);
            animation.Update(elapsedTime);
        }

        public override void Render()
        {
            base.Render();
            //从动画中取出纹理，并设定属性值
            Sprite sp = animation.GetSprite();
            sp.SetPosition(Position.X,Position.Y);
            //使用一般着色器进行着色
            renderer.DrawSprite(sp);
        }

    }


}
