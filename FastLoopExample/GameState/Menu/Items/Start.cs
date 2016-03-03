using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.GameState.Menu.Items
{
    public class OStart : IGameObject,RenderLevel
    {

        int renderLevel = 10;                 //最优先级别
        TextureManager texture_manager;    
        SoundManagerEx soundManager;

        public OStart( TextureManager _t , SoundManagerEx _s)
        {
            texture_manager = _t;
            soundManager = _s;

        }

        public void Update(double elapsedTime)
        {
            if (Input.getKeyDown("Space"))
            {
                    Option.Command = Menu_Command.Start;
                    Option._currentstate = Option._NONE_;
            }
        }

        public void Render()
        {
            
        }

        public int getLevel()
        {
            return renderLevel;
        }

        public void setLevel(int i)
        {
            
        }

        public void RenderByLevel()
        {
            
        }

        public void Start()
        {
            
        }
    }
}
