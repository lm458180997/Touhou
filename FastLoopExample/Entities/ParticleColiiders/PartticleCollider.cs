using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FastLoopExample.Particle;

namespace FastLoopExample.ParticleColiiders
{
    /// <summary>
    /// 粒子碰撞器（在指定范围的区域内的粒子会受到碰撞器的行为约束【特殊情况下，不能使用含有插值算法的行为粒子】）
    /// </summary>
    public class ParticleCollider
    {
        public virtual void Collition(Particles particles , double elapsedTime)
        {
        }
    }

    /// <summary>
    /// 会把周围的粒子强制弹开的粒子碰撞器（碰撞范围为圆形）
    /// </summary>
    public class HitOut_ParticleCollider : ParticleCollider
    {
        Vector2D Position;
        float radius = 45;
        float radius_pow = 900;
        float speed = 100;

        public HitOut_ParticleCollider(Vector2D Pos)
        {
            Position = Pos;
            Radius = radius;
        }

        public float Radius
        {
            get { return radius; }
            set {
                radius = value;
                radius_pow = radius * radius;
            }
        }
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public override void Collition(Particles particles, double elapsedTime)
        {
            if (particles == null)
                return;
            if (particles.Enabled == false)
                return;
            double distance = 0;
            foreach (particle p in particles.particles)
            {
                distance = (p.Position.X - Position.X) * (p.Position.X - Position.X) +
                    (p.Position.Y - Position.Y) * (p.Position.Y - Position.Y);
                //如果进入范围以内则触发碰撞器
                if (distance < radius_pow)
                {
                    Vector2D vct = new Vector2D((p.Position.X - Position.X ), 
                        (p.Position.Y - Position.Y)).GetNormalize();
                    double dx = elapsedTime * speed * vct.X;
                    double dy = elapsedTime * speed * vct.Y;
                    double tx = p.Position.X + dx;
                    double ty = p.Position.Y + dy;
                    p.Position.X = tx;
                    p.Position.Y = ty;
                }

            }

        }

    }


}
