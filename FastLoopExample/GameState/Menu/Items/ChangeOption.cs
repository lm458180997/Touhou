using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.GameState.Menu.Items
{
    public class ChangeOption : IGameObject , RenderLevel
    {

        enum SelectState
        {
            volume =0, quit =1
        }

        SelectState CurrentState;

        public const float unit_for512 = 0.015625f;

        int renderLevel = 10;

        TextureManager texturemanager;

        SoundManagerEx soundmanager;

        Dictionary<string , DoubleSprites> Sprits;  //菜单含有的Sprites

        Renderer renderer = new Renderer();

        public volume _volume;                  //音量栏



        public ChangeOption(TextureManager _t, SoundManagerEx _s)
        {
            this.texturemanager = _t;
            this.soundmanager = _s;
            CurrentState = new SelectState();

            initSprites();
            _volume = new volume(Sprits);
            _volume.Level = soundmanager.Volume / 10;

            System.Diagnostics.Debug.Print(soundmanager.Volume.ToString () +"and"+ _volume.Level.ToString());
            initPosition();
        }

        public void Start()
        {
            CurrentState = SelectState.volume;                   //默认的选择项是volume

            _volume.Start();                                     //volume 的Start 初始化
            initPosition();
        }

        //初始化坐标
        void initPosition()
        {
            _volume.SetPosition(-100, 100, 0);
            Sprits["Quit"].setPosition(-100, 50, 0);
        }

        void changeUnit(ref Sprite spr, int x, int y, int x2, int y2)  //将单位换算成百分比
        {
            spr.SetUVs(new Point(x * unit_for512, y * unit_for512),
                new Point(x2 * unit_for512, y2 * unit_for512));
        }

        void initSprites()
        {
            Sprits = new Dictionary<string, DoubleSprites>();
            Texture texture = texturemanager.Get("Title_01");
            Sprite spr1 = new Sprite();
            Sprite spr2 = new Sprite();

            #  region   0——9
            spr1.Texture = texture; spr2.Texture = texture;
            changeUnit(ref spr1, 0, 60, 3, 64);
            changeUnit(ref spr2, 30, 60, 33, 64);
            spr1.SetWidth(3 * 8); spr1.SetHeight(4 * 8);
            spr2.SetWidth(3 * 8); spr2.SetHeight(4 * 8);
            Sprits.Add("0", new DoubleSprites(spr1, spr2));

            spr1 = new Sprite(); spr2 = new Sprite();
            spr1.Texture = texture; spr2.Texture = texture;
            changeUnit(ref spr1, 3, 60, 6, 64);
            changeUnit(ref spr2, 33, 60, 36, 64);
            spr1.SetWidth(3 * 8); spr1.SetHeight(4 * 8);
            spr2.SetWidth(3 * 8); spr2.SetHeight(4 * 8);
            Sprits.Add("1", new DoubleSprites(spr1, spr2));

            spr1 = new Sprite(); spr2 = new Sprite();
            spr1.Texture = texture; spr2.Texture = texture;
            changeUnit(ref spr1, 6, 60, 9, 64);
            changeUnit(ref spr2, 36, 60, 39, 64);
            spr1.SetWidth(3 * 8); spr1.SetHeight(4 * 8);
            spr2.SetWidth(3 * 8); spr2.SetHeight(4 * 8);
            Sprits.Add("2", new DoubleSprites(spr1, spr2));

            spr1 = new Sprite(); spr2 = new Sprite();
            spr1.Texture = texture; spr2.Texture = texture;
            changeUnit(ref spr1, 9, 60, 12, 64);
            changeUnit(ref spr2, 39, 60, 42, 64);
            spr1.SetWidth(3 * 8); spr1.SetHeight(4 * 8);
            spr2.SetWidth(3 * 8); spr2.SetHeight(4 * 8);
            Sprits.Add("3", new DoubleSprites(spr1, spr2));

            spr1 = new Sprite(); spr2 = new Sprite();
            spr1.Texture = texture; spr2.Texture = texture;
            changeUnit(ref spr1, 12, 60, 15, 64);
            changeUnit(ref spr2, 42, 60, 45, 64);
            spr1.SetWidth(3 * 8); spr1.SetHeight(4 * 8);
            spr2.SetWidth(3 * 8); spr2.SetHeight(4 * 8);
            Sprits.Add("4", new DoubleSprites(spr1, spr2));

            spr1 = new Sprite(); spr2 = new Sprite();
            spr1.Texture = texture; spr2.Texture = texture;
            changeUnit(ref spr1, 15, 60, 18, 64);
            changeUnit(ref spr2, 45, 60, 48, 64);
            spr1.SetWidth(3 * 8); spr1.SetHeight(4 * 8);
            spr2.SetWidth(3 * 8); spr2.SetHeight(4 * 8);
            Sprits.Add("5", new DoubleSprites(spr1, spr2));

            spr1 = new Sprite(); spr2 = new Sprite();
            spr1.Texture = texture; spr2.Texture = texture;
            changeUnit(ref spr1, 18, 60, 21, 64);
            changeUnit(ref spr2, 48, 60, 51, 64);
            spr1.SetWidth(3 * 8); spr1.SetHeight(4 * 8);
            spr2.SetWidth(3 * 8); spr2.SetHeight(4 * 8);
            Sprits.Add("6", new DoubleSprites(spr1, spr2));

            spr1 = new Sprite(); spr2 = new Sprite();
            spr1.Texture = texture; spr2.Texture = texture;
            changeUnit(ref spr1, 21, 60, 24, 64);
            changeUnit(ref spr2, 51, 60, 54, 64);
            spr1.SetWidth(3 * 8); spr1.SetHeight(4 * 8);
            spr2.SetWidth(3 * 8); spr2.SetHeight(4 * 8);
            Sprits.Add("7", new DoubleSprites(spr1, spr2));

            spr1 = new Sprite(); spr2 = new Sprite();
            spr1.Texture = texture; spr2.Texture = texture;
            changeUnit(ref spr1, 24, 60, 27, 64);
            changeUnit(ref spr2, 54, 60, 57, 64);
            spr1.SetWidth(3 * 8); spr1.SetHeight(4 * 8);
            spr2.SetWidth(3 * 8); spr2.SetHeight(4 * 8);
            Sprits.Add("8", new DoubleSprites(spr1, spr2));

            spr1 = new Sprite(); spr2 = new Sprite();
            spr1.Texture = texture; spr2.Texture = texture;  //必须要先建立texture，再设置大小才能正确的进行工作
            changeUnit(ref spr1, 27, 60, 30, 64);
            changeUnit(ref spr2, 57, 60, 60, 64);
            spr1.SetWidth(3 * 8); spr1.SetHeight(4 * 8);
            spr2.SetWidth(3 * 8); spr2.SetHeight(4 * 8);
            Sprits.Add("9", new DoubleSprites(spr1, spr2));
            #endregion

            spr1 = new Sprite(); spr2 = new Sprite();
            spr1.Texture = texture; spr2.Texture = texture;
            changeUnit(ref spr1, 0, 32, 10, 36);
            changeUnit(ref spr2, 10, 32, 20, 36);
            spr1.SetWidth(10 * 8); spr1.SetHeight(32);
            spr2.SetWidth(10 * 8); spr2.SetHeight(32);
            Sprits.Add("Quit", new DoubleSprites(spr1, spr2));

            spr1 = new Sprite(); spr2 = new Sprite();
            spr1.Texture = texture; spr2.Texture = texture;
            changeUnit(ref spr1, 0, 44, 22, 48);
            changeUnit(ref spr2, 22, 44, 44, 48);
            spr1.SetWidth(22 * 8); spr1.SetHeight(32);
            spr2.SetWidth(22 * 8); spr2.SetHeight(32);
            Sprits.Add("BGM Volume", new DoubleSprites(spr1, spr2));

        }

        void SetPosition(DoubleSprites doublesprite, double x, double y)
        {
            doublesprite.Sprite1.SetPosition(x, y);
            doublesprite.Sprite2.SetPosition(x, y);
        }
        void SetPosition(DoubleSprites doublesprite, Vector vct)
        {
            doublesprite.Sprite1.SetPosition(vct);
            doublesprite.Sprite2.SetPosition(vct);
        }

        public void Update(double elapsedTime)
        {
            _volume.Update(elapsedTime);

            if (Input.getKeyDown("Space"))
            {
                soundmanager.Play("002-System02");
                if (CurrentState == SelectState.quit)
                {
                    Option.Command = Menu_Command.OptionChanged;
                    Option._currentstate = Option._NONE_;
                }
            }
            if (Input.getKeyDown("Up"))
            {
                if (CurrentState != SelectState.volume)
                {
                    CurrentState--;
                    soundmanager.Play("002-System02");
                    if (CurrentState == SelectState.volume)
                    {
                        _volume.Selected = true;
                    }
                    else
                    {
                        _volume.Selected = false;
                    }
                }
            }
            if (Input.getKeyDown("Down"))
            {
                if (CurrentState != SelectState.quit)
                {
                    CurrentState++;
                    soundmanager.Play("002-System02");
                    if (CurrentState == SelectState.volume)
                    {
                        _volume.Selected = true;
                    }
                    else
                    {
                        _volume.Selected = false;
                    }

                }
                
            }
            if (Input.getKeyDown("Left"))
            {
                if (CurrentState == SelectState.volume)
                {
                    _volume.GoLeft();
                    soundmanager.Play("002-System02");
                    soundmanager.Volume = _volume.Level * 10;
                }
            }
            if (Input.getKeyDown("Right"))
            {
                if (CurrentState == SelectState.volume)
                {
                    _volume.GoRight();
                    soundmanager.Play("002-System02");
                    soundmanager.Volume = _volume.Level * 10;
                }
            }


        }

        public void Render()
        {
            _volume.Render();
            if (CurrentState == SelectState.quit)
            {
                renderer.DrawSprite(Sprits["Quit"].Sprite1);
            }
            else
            {
                renderer.DrawSprite(Sprits["Quit"].Sprite2);
            }
        }

        public int getLevel()
        {
            return renderLevel;
        }

        public void setLevel(int i)
        {
            renderLevel = i;
        }

        public void RenderByLevel()
        {
            Render();
        }

    }

    //volume 控制条
    public class volume : IGameObject
    {
        int selectvalue  = 5;
        bool selected;                          //是否被选中
        Vector Position;                        // Volume 行的坐标
        DoubleSprites Title;                      // 标题框
        Renderer renderer;
        Dictionary<int, DoubleSprites> Numbers;

        public volume( Dictionary<string , DoubleSprites> list) 
        {
            Position = new Vector();
            renderer = new Renderer();
            Numbers = new Dictionary<int, DoubleSprites>();

            for (int i = 0; i < 10; i++)
            {
                Numbers.Add(i, list[i.ToString()]);
            }
            Title = list["BGM Volume"];

            SetPosition(0, 0, 0);
        }

        public void SetPosition(double x, double y, double z)
        {
            Position.X = x;
            Position.Y = y;
            Position.Z = z;
            Title.setPosition(x, y, z);

            x+= 100;
            for (int i = 0; i < 10; i++)
            {
                Numbers[i].setPosition(x , y, z);
                x += 30;
            }

        }

        public void GoLeft()
        {
            Level -= 1;
        }

        public void GoRight()
        {
            Level += 1;
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public int Level
        {
            get { return selectvalue; }
            set { setvolume(value); }
        }

        public void setvolume(int level)
        {
            if (level < 0)
                level = 0;
            if (level > 9)
                level = 9;
            selectvalue = level;
        }

        public void Start()
        {
            Selected = true;
        }

        public void Update(double elapsedTime)
        {
           
        }

        public void Render()
        {
            if (selected)
                renderer.DrawSprite(Title.Sprite1);
            else
                renderer.DrawSprite(Title.Sprite2);

           for (int i = 0; i < 10; i++)
           {
               if (i != selectvalue)
               { 
                   renderer.DrawSprite(Numbers[i].Sprite2);
               }
               else
               {
                   renderer.DrawSprite(Numbers[i].Sprite1);
               }
           }
        }
    }

}
