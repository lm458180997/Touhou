using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tao.OpenGl;
using FastLoopExample.GameState.Menu;

namespace FastLoopExample
{
    public enum Menu_Command
    {
        Start,
        Quit,
        CharactorSelect,
        OptionChanged,
        None
    }

    class GameMenuState : IGameObject
    {

        Sprite _testSprite = new Sprite();        

        Renderer _renderer = new Renderer();        // 着色器

        StateSystem _system;                        // 状态机

        TextureManager _textureManager;             // 纹理控制器

        SoundManagerEx soundmanager;                // 声音控制器

        BackGround backgroundImage;                 // 背景图片

        Option option;                              // 各项管理

        Form1 form;                                 // 此状态具备管理窗体的权限

        public GameMenuState(StateSystem sys, TextureManager _t, SoundManagerEx _s , Form1 form = null)
        {
            _system = sys;
            _textureManager = _t;
            soundmanager = _s;
            backgroundImage = new BackGround(_t, 0);        //背景更改为0级（最底层）
            option = new Option(_t, _s , backgroundImage , 10); //10级，最顶层
            if(form !=null)
            this.form = form;
        }

        //逻辑更新
        public void Update(double elapsedTime)
        {
            backgroundImage.Update(elapsedTime);
            option.Update(elapsedTime);

            DealWithCommand();//处理选项命令

        }

        //处理各项命令
        public void DealWithCommand()
        {
            switch (Option.Command)
            {
                case Menu_Command.Start:
                    _system.ChangeState("Stage1");
                    soundmanager.Stop("victor01");
                    break;
                case Menu_Command.OptionChanged:
                    System.IO.StreamWriter writer = new System.IO.StreamWriter("opst.dat");
                    writer.WriteLine("$volume#" + (soundmanager.Volume+1).ToString());
                    writer.Dispose();
                    break;
                case Menu_Command.CharactorSelect:

                    break;
                case Menu_Command.Quit:
                    if(form!=null)
                       form.Close();
                    break;
            }
            Option.Command = Menu_Command.None;
        }

        public void Render()
        {
            Gl.glShadeModel(Gl.GL_LINE_SMOOTH);
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            for (int i = 0; i < 11; i++)
            {
                RenderLevel render = backgroundImage;
                if (render.getLevel() == i)            //按照等级来决定绘画顺序，只划分0-10级
                    render.RenderByLevel();
                render = option;
                if (render.getLevel() == i)            //按照等级来决定绘画顺序，只划分0-10级
                    render.RenderByLevel();
            }
            
            Gl.glFinish();
        }

        //切换场景时会自动调用Start
        public void Start()
        {
            Option.Command = Menu_Command.None;            //无命令

            backgroundImage.Start();
            option.Start();
        }
    }



}
