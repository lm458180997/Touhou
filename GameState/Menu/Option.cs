using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FastLoopExample.GameState.Menu.Items;

namespace FastLoopExample.GameState.Menu
{
    public struct itemUsed
    {
        public int Item;
        public bool used;
        public itemUsed(int i, bool s = true)
        { Item = i; used = s; }
    }
    public struct DoubleSprites
    {
        public Sprite Sprite1;
        public Sprite Sprite2;
        public DoubleSprites(Sprite s1, Sprite s2)
        {
            Sprite1 = s1;
            Sprite2 = s2;
        }
        public void  setPosition(double x, double y, double  z= 0)
        {
            Sprite1.SetPosition(x, y, z);
            Sprite2.SetPosition(x, y, z);
        }
    }

    public class Option : IGameObject , RenderLevel
    {
        public const float unit_for512 = 0.015625f;
        public const int _START_ = 0, _RESULT__ = 1,_MUSIC_  = 2,
           _REPLY_ = 3, _OPTION_ = 4, _QUIT_ = 5,
          _PRACTICE_  = 6, _ExtraStart = 7, _SpellPractice_ = 8, _NONE_ = 9;
        List<itemUsed> itemsUsed;

        Dictionary<int, DoubleSprites> Sprits;  //主菜单的所有选项

        Point[] Positions;

        public static int _currentstate = _NONE_;

        int selected = 0;               //当前选择项

        TextureManager _texturemanager;

        SoundManagerEx _soundmanager;

        BackGround _backgroundImage;

        Renderer renderer;

        OStart startmodel;

        ChangeOption optionmodel;

        int _render_level = 10;

        public static Menu_Command Command = Menu_Command.None;

        public Option(TextureManager texturemanager , SoundManagerEx soundmanager ,BackGround _b, int renderlevel =10)
        {
            _texturemanager = texturemanager;
            _soundmanager = soundmanager;
            _render_level = renderlevel;
            _backgroundImage = _b;

            Command = Menu_Command.None;

            //储存禁用的名单（暂留）
            itemsUsed = new List<itemUsed>();
            for (int i = 0; i < 10; i++)
            {
                itemsUsed.Add(new itemUsed(i,true));
            }

           
            //当前的功能选择项
            selected = 0;

            renderer = new Renderer();
            initiazeSprits();

            //option模块
            optionmodel = new ChangeOption(texturemanager, soundmanager);
            //start模块
            startmodel = new OStart(texturemanager, soundmanager);

        }

        void initPosition()
        {
            Positions = new Point[9];
            int x = 0;
            int y = 0;
            int dis = 40;
            for (int i = 0; i < 9; i++)
            {
                Positions[i] = new Point(x, y);
                y -= dis;
            }
        }

        void SetPosition(DoubleSprites doublesprite, double x , double y)
        {
            doublesprite.Sprite1.SetPosition(x, y);
            doublesprite.Sprite2.SetPosition(x, y);
        }
        void SetPosition(DoubleSprites doublesprite, Vector vct)
        {
            doublesprite.Sprite1.SetPosition(vct);
            doublesprite.Sprite2.SetPosition(vct);
        }

        void initiazeSprits()
        {
            initPosition();
            Sprits = new Dictionary<int, DoubleSprites>();
            Texture tex = _texturemanager.Get("Title_01");
            for (int i = 0; i < 9; i++)
            {
                Sprite spr1 = new Sprite();
                Sprite spr2 = new Sprite();
                spr1.Texture = tex;
                spr2.Texture = tex;
                if (i == _START_)
                {
                    spr1.SetUVs(new Point(0, 0), new Point(0.3125f, 0.0625f));
                    spr2.SetUVs(new Point(0.3125f, 0), new Point(0.625f, 0.0625f));
                    spr1.SetWidth(160); spr1.SetHeight(32);
                    spr2.SetWidth(160); spr2.SetHeight(32);
                }
                else if (i == _RESULT__)
                {
                    changeUnit(ref spr1, 0, 16, 22, 20, unit_for512);
                    changeUnit(ref spr2, 22, 16, 44, 20, unit_for512);
                    spr1.SetWidth(22*8); spr1.SetHeight(32);
                    spr2.SetWidth(22*8); spr2.SetHeight(32);
                }
                else if (i == _MUSIC_)
                {
                    changeUnit(ref spr1, 0, 20, 22, 24, unit_for512);
                    changeUnit(ref spr2, 22, 20, 44, 24, unit_for512);
                    spr1.SetWidth(22 * 8); spr1.SetHeight(32);
                    spr2.SetWidth(22 * 8); spr2.SetHeight(32);
                }
                else if (i == _REPLY_)
                {
                    changeUnit(ref spr1, 0, 12, 12, 16, unit_for512);
                    changeUnit(ref spr2, 12, 12, 24, 16, unit_for512);
                    spr1.SetWidth(12 * 8); spr1.SetHeight(32);
                    spr2.SetWidth(12 * 8); spr2.SetHeight(32);
                   
                }
                else if (i == _OPTION_)
                {
                    changeUnit(ref spr1, 0, 28, 14, 32, unit_for512);
                    changeUnit(ref spr2, 14, 28, 28, 32, unit_for512);
                    spr1.SetWidth(14 * 8); spr1.SetHeight(32);
                    spr2.SetWidth(14 * 8); spr2.SetHeight(32);
                }
                else if (i == _QUIT_)
                {
                    changeUnit(ref spr1, 0, 32, 10, 36, unit_for512);
                    changeUnit(ref spr2, 10, 32, 20, 36, unit_for512);
                    spr1.SetWidth(10 * 8); spr1.SetHeight(32);
                    spr2.SetWidth(10 * 8); spr2.SetHeight(32);
                }
                spr1.SetPosition(Positions[i].X, Positions[i].Y);
                spr2.SetPosition(Positions[i].X, Positions[i].Y);
                Sprits.Add(i, new DoubleSprites(spr1, spr2));
            }

        }
        void changeUnit(ref Sprite  spr, int x, int y, int x2, int y2 , float eachpercent)  //将单位换算成百分比
        {
            spr.SetUVs(new Point(x * eachpercent, y * eachpercent),
                new Point(x2 * eachpercent, y2 * eachpercent));
        }

        //Start
        public void Start()
        {
            //播放一下背景音乐
            _soundmanager.Play("victor01"); 

            Command = Menu_Command.None;
            optionmodel.Start();
            startmodel.Start();
            
        }

        public void Update(double elapsedTime)
        {
            DealwithState(_currentstate,elapsedTime);         //处理当前状态事务   
        }

        //处理各种状态
        void DealwithState(int state , double elapsedtime)
        {

            #region None
            if (_currentstate == _NONE_)
            {
                if (Input.getKeyDown("Up"))
                {
                    _soundmanager.Play("002-System02");
                    selected--;
                    if (selected < 0)
                        selected = 5;
                }
                if (Input.getKeyDown("Down"))
                {
                    _soundmanager.Play("002-System02");
                    selected++;
                    if (selected > 5)
                        selected = 0;
                }
                if (Input.getKeyDown("Escape"))
                {

                }
                if (Input.getKeyDown("Space"))
                {
                    if (selected == _OPTION_)
                    {
                        optionmodel.Start();
                        _currentstate = _OPTION_;
                    }
                    if (selected == _QUIT_)
                    {
                        Command = Menu_Command.Quit;
                    }
                    if (selected == _START_)
                    {
                       // startmodel.Start();
                       // _currentstate = _START_;
                        Command = Menu_Command.Start;
                    }
                }
            }
            #endregion

            #region Start

            if (_currentstate == _START_)
            {
                startmodel.Update(elapsedtime);
            }

            #endregion

            #region Option

            if (_currentstate == _OPTION_)
            {
                optionmodel.Update(elapsedtime);
            }

            #endregion

        }

        public void Render()
        {
            if (_currentstate == _NONE_)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (i != selected)
                    {
                        renderer.DrawSprite(Sprits[i].Sprite2);
                    }
                    else
                    {
                        renderer.DrawSprite(Sprits[i].Sprite1);
                    }
                }
            }
            if (_currentstate == _OPTION_)
            {
                optionmodel.Render();
            }
            if (_currentstate == _START_)
            {
                startmodel.Render();
            }

        }

        public int getLevel()
        {
            return _render_level;
        }

        public void setLevel(int i)
        {
            _render_level = i;
        }

        public void RenderByLevel()
        {
            Render();
        }
    }

    public abstract class Animation
    {
        public static List<Animation> Animations = new List<Animation>();
        public static List<Animation> toAdd = new List<Animation>();
        public static List<Animation> toRemove = new List<Animation>();

        public bool working = true;           //是否在正常工作中

        public void Run()
        {
            Animations.Add(this);
        }
        public static void Check()
        {
            foreach (Animation a in Animations)
                if (!a.working)
                    toRemove.Add(a);
        }

        //对所有的动画进行逻辑更新
        public static void _Update(double t)
        {
            Check();

            foreach (Animation a in Animation.toAdd)
                Animation.Animations.Add(a);
            Animation.toAdd.Clear();
            foreach (Animation a in Animation.toRemove)
                Animation.Animations.Remove(a);
            Animation.toRemove.Clear();
            foreach (Animation a in Animation.Animations)
                a.Update(t);
        }

        public virtual void Render() { }

        public virtual void Update(double t) {  }

        public static void Clear()
        {
            Animations.Clear();
            toAdd.Clear(); 
            toRemove.Clear();
        }
    }



}
