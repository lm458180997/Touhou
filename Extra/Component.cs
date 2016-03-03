using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{

    //组件
    public class Component
    {
        //组件命名
        public string name= "none";
        //组件是否在正常工作
        public bool working = true;
        //组件是否需要注销
        public bool disabled = false;
        //组件逻辑更新
        public virtual void Update(float ElapsedTime) { }
    }

    //碰撞组件（基）
    public class CollisionComponent : Component
    {
        /// <summary>
        /// //碰撞组件（基）
        /// </summary>
        public CollisionComponent()
        {
            name = "CollisionComponent";
        }
        /// <summary>
        /// 组件对子弹的碰撞判定
        /// </summary>
        /// <param name="bullet">子弹的实体</param>
        /// <param name="msg">集中目标后产生的信息数据，后传递给其它实体获得信息</param>
        /// <returns>是否产生了碰撞</returns>
        public virtual bool Collision(Bullet bullet, ref MessageManager msg) { return false; }
        /// <summary>
        /// 组件对敌人的碰撞判定
        /// </summary>
        /// <param name="e">敌人的实体</param>
        /// <param name="msg">集中目标后产生的信息数据，后传递给其它实体获得信息</param>
        /// <returns>是否产生了碰撞</returns>
        public virtual bool Collision(Enemy e, ref MessageManager msg) { return false; }
        /// <summary>
        /// 逻辑更新
        /// </summary>
        /// <param name="elapsedTime">逻辑更新所提供的间隔时间</param>
        public override void Update(float elapsedTime) { }
        /// <summary>
        /// 纹理渲染
        /// </summary>
        /// <param name="renderer">着色器</param>
        public virtual void Render(Renderer renderer) { }

    }

}
