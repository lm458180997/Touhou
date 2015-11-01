using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{
    public class SoundTestState:IGameObject
    {
        SoundManager _soundManager;
        double _count = 3;

        public SoundTestState(SoundManager soundManger)
        {
            _soundManager = soundManger;
         //   _soundManager.PlaySound("mymusic1");
        }
        public void Render()
        { }

        public void Update(double elapsedTime)
        {
            _count -= elapsedTime;
            if (_count < 0)
            {
                _count = 3;
              //  _soundManager.PlaySound("mymusic1");
            }
        }


        public void Start()
        {
           
        }
    }
}
