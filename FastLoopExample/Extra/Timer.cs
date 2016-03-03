using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{
    public delegate void Loopevent();        //定义一种委托类型 
    public class Timer
    {
        Loopevent loopevent;
        public bool enabled = false;
        public float interval;
        public float time_caculate;

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public Timer(Loopevent loop, float interval)
        {
            loopevent = loop;
            this.interval = interval; time_caculate = 0;
        }

        public void Update(float dt)
        {
            time_caculate += dt;
            if (time_caculate >= interval)
            {
                if (enabled)
                    loopevent();
                time_caculate = 0;
            }
        }

        public void start()
        {
            Enabled = true;
        }

        public void stop()
        {
            Enabled = false;
        }


    }


}
