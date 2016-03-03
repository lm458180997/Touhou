using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{
    public class AnimationManager
    {
        public  List<Animation> Animations = new List<Animation>();
        public  List<Animation> toAdd = new List<Animation>();
        public  List<Animation> toRemove = new List<Animation>();
        public bool working = true;           //是否在正常工作中
        public void Check()
        {
            foreach (Animation a in Animations)
                if (!a.working)
                    toRemove.Add(a);
        }
        //对所有的动画进行逻辑更新
        public void _Update(double t)
        {
            Check();
            foreach (Animation a in toAdd)
                Animations.Add(a);
            toAdd.Clear();
            foreach (Animation a in toRemove)
                Animations.Remove(a);
            toRemove.Clear();
            foreach (Animation a in Animations)
                a.Update(t);
        }
        public  void Clear()
        {
            Animations.Clear();
            toAdd.Clear(); 
            toRemove.Clear();
        }
    }

    public class Animation
    {
        public bool working = true;           //是否在正常工作中
        public virtual void Render() { }
        public void Run(AnimationManager anim)
        {
            anim.toAdd.Add(this);
        }
        public virtual void Update(double elapsedTime) { }
    }

    public class LinearAnimation
    {

    }

}
