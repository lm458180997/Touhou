using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace FastLoopExample
{
    public interface CharFont
    {
        Sprite GetAci(string key);
        string GetName();
    }

    public class TextFontEx : CharFont
    {
        const float per_256 = 0.03125f;
        TextureManager texturemanager;
        Dictionary<string, Sprite> Font_Sprites = new Dictionary<string, Sprite>();
        List<Charactor> charactors = new List<Charactor>();

        public TextFontEx(TextureManager _t)
        {
            texturemanager = _t;
            AddCharactor();
            CreateSprites();
        }

        void AddCharactor()
        {
            
            for (int i = 0; i < 10; i++)
            {
                charactors.Add(new Charactor(0+i*2,6,i.ToString()));
            }
            charactors.Add(new Charactor(30, 4, "/"));
            charactors.Add(new Charactor(0,4," "));

        }
        void CreateSprites()
        {
            Texture texture = texturemanager.Get("fontex");
            foreach (Charactor c in charactors)
            {
                Sprite spt = new Sprite();
                spt.Texture = texture;
                spt.SetUVs(c.x * per_256, c.y * per_256, (c.x + 2) * per_256, (c.y + 2) * per_256);
                Font_Sprites.Add(c.id, spt);
            }
        }

        public Sprite GetAci(string key) 
        {
            return Font_Sprites[key];
        }


        public string GetName()
        {
            return "Aci1";
        }
    }

    //华康少女体字体库
    public class FontAciHuaKang : CharFont
    {

        static Dictionary<string, FontCharactor> Fonts = new Dictionary<string, FontCharactor>();
        static Sprite sprite;            //用于记录着色数据的块
        static Texture[] Textures;       //纹理组
        static bool inited = false;      //是否已经初始化
        static TextureManager texturemanager;

        public FontAciHuaKang(TextureManager texturemanager)
        {
            FontAciHuaKang.texturemanager = texturemanager;
            if (!inited)
                init();
        }
        void init()
        {
            Textures = new Texture[5];
            Textures[0] = texturemanager.Get("FontAciP1");
            Textures[1] = texturemanager.Get("FontAciP2");
            Textures[2] = texturemanager.Get("FontAciP3");
            Textures[3] = texturemanager.Get("FontAciP4");
            Textures[4] = texturemanager.Get("FontAciP5");
            sprite = new Sprite();
            sprite.SetWidth(24);
            sprite.SetHeight(24);
            LoadData();
            inited = true;
        }

        void LoadData()
        {
            int ID = 0;
            LoadXml("res\\Fonts\\Aci_P1.xml", ref ID, 1,1024,2048);
            LoadXml("res\\Fonts\\Aci_P2.xml", ref ID, 2, 1024, 2048);
            LoadXml("res\\Fonts\\Aci_P3.xml", ref ID, 3, 1024, 2048);
            LoadXml("res\\Fonts\\Aci_P4.xml", ref ID, 4, 1024, 2048);
            LoadXml("res\\Fonts\\Aci_P5.xml", ref ID, 5,1024,1024);
            Fonts.Add("，", Fonts[","]);
            Fonts.Add("！", Fonts["!"]);
            Fonts.Add("ー", Fonts["—"]);
            Fonts.Add("（", Fonts["("]);
            Fonts.Add("）", Fonts[")"]);
            Fonts.Add("～", Fonts["~"]);
            Fonts.Add("・", Fonts["-"]);
            
        }

        void LoadXml(string path, ref int startID, int selecttexture, 
            float max_width , float max_height)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);    //加载Xml文件
            XmlElement rootElem = doc.DocumentElement;   //获取根节点 
            XmlNodeList personNodes = rootElem.GetElementsByTagName("KeyValuePair");
            string result = "";
            string Name;
            float x_offset, y_offset, width=0, height=0;
            float uvs1 = 0, uvs2 = 0, uvs3 = 0, uvs4 = 0;
            float offsetH = 2 / max_height;
            foreach (XmlNode node in personNodes)
            {
                string strName = ((XmlElement)node).GetAttribute("Character");   //获取name属性值 
                result += strName;
                Name = strName;
                XmlNodeList childnodes = node.ChildNodes;
                foreach (XmlNode cnode in childnodes)
                {
                    strName = ((XmlElement)cnode).GetAttribute("XOffset");   //获取XOffset属性值 
                    x_offset = Convert.ToInt32(strName);
                    strName = ((XmlElement)cnode).GetAttribute("YOffset");   //获取YOffset属性值 
                    y_offset = Convert.ToInt32(strName);
                    strName = ((XmlElement)cnode).GetAttribute("width");   //获取width属性值 
                    width = Convert.ToInt32(strName);
                    strName = ((XmlElement)cnode).GetAttribute("height");   //获取height属性值 
                    height = Convert.ToInt32(strName);
                    uvs1 = x_offset / max_width;
                    uvs2 = y_offset / max_height;
                    uvs3 = (x_offset + width) / max_width;
                    uvs4 = (y_offset + 24) / max_height;
                }
                Fonts.Add(Name, new FontCharactor(startID, Name, uvs1, uvs2, uvs3, uvs4 - offsetH, 
                    selecttexture, width, 24));
                startID++;
            }
        }

        public Sprite GetAci(string key)
        {
            try
            {
                FontCharactor c = Fonts[key];
                sprite.Texture = Textures[c.SelectTexture - 1];
                sprite.SetUVs(c.uvs1, c.uvs2, c.uvs3, c.uvs4);
                sprite.SetWidth(c.width);
                sprite.SetHeight(c.height);
                return sprite;
            }
            catch (Exception e)
            {
                FontCharactor c = Fonts["口"];
                sprite.Texture = Textures[c.SelectTexture - 1];
                sprite.SetUVs(c.uvs1, c.uvs2, c.uvs3, c.uvs4);
                sprite.SetWidth(c.width);
                sprite.SetHeight(c.height);
                return sprite;
            }
        }

        public static Sprite Static_GetAci(string key)
        {
            try
            {
                FontCharactor c = Fonts[key];
                sprite.Texture = Textures[c.SelectTexture - 1];
                sprite.SetUVs(c.uvs1, c.uvs2, c.uvs3, c.uvs4);
                sprite.SetWidth(c.width);
                sprite.SetHeight(c.height);
                return sprite;
            }
            catch (Exception e)
            {
                FontCharactor c = Fonts["口"];
                sprite.Texture = Textures[c.SelectTexture - 1];
                sprite.SetUVs(c.uvs1, c.uvs2, c.uvs3, c.uvs4);
                sprite.SetWidth(c.width);
                sprite.SetHeight(c.height);
                return sprite;
            }
        }

        public string GetName()
        {
            return "HuaKang";
        }
    }

    //xml表中的字体映射数据
    public class FontCharactor
    {
        public string Name;
        public float uvs1, uvs2, uvs3, uvs4;
        public float width, height;
        public int CharactorID;
        public int SelectTexture;
        public FontCharactor(int ID, string name, float u1, float u2, float u3, float u4,
            int select, float w, float h)
        {
            CharactorID = ID; Name = name; uvs1 = u1; uvs2 = u2;
            uvs3 = u3; uvs4 = u4;
            SelectTexture = select;
            width = w;
            height = h;
        }
        public override string ToString()
        {
            return "Name: " + Name + "  ID: " + CharactorID.ToString()
                + "\nslecectTexture: " + SelectTexture.ToString()
                + "\nuvs1: " + uvs1.ToString() + " uvs2: " + uvs2.ToString()
                + "\nuvs3: " + uvs3.ToString() + " uvs4: " + uvs4.ToString() + "\n";
        }

    }

    //字体库渲染工具
    public class FontWriterTool
    {
        public const int HuaKang = 0;
        int Type = HuaKang;            //字体类型
        public string Name = "HuaKang";//名字标签
        CharFont SelectFont;           //当前选择的字体库 
        TextMassege massege = new TextMassege();         //信息工具
        public bool Binded= false;     //是否已经绑定
        public FontWriterTool() {}
        public FontWriterTool(CharFont c) { SelectFont = c; Binded = true; }
        public void BindFont(CharFont cf)
        {
            SelectFont = cf;
            if (cf.GetName() == "HuaKang")
            {
                Type = HuaKang;
                Name = "HuaKang";
            }
            Binded = true;
        }

        /// <summary>
        /// 绘制字符串
        /// </summary>
        /// <param name="str">将要绘画的字符串</param>
        /// <param name="x">首字的对应显示坐标x</param>
        /// <param name="y">首字的对应显示坐标y</param>
        /// <param name="percent">字体显示的百分比（高度参照24，宽度自适应）</param>
        /// <param name="distance">字间的间距</param>
        /// <param name="ColMax">每行最多能显示的字的数量</param>
        public virtual void DrawString(Renderer renderer, string str, double x, double y,float percent,
            float distance , int ColMax )
        {
            if (!Binded)
                throw new Exception("当前工具没有绑定字体");
            Sprite sprite;
            Char[] Arr = str.ToCharArray();
            int count = Arr.Length;
            int offsetx = 0;
            int offsety = 0;
            float h_distance = percent * 24;
            for (int i = 0; i < count; i++)
            {
                string s = Arr[i].ToString();
                if (s == "\n")
                {
                    offsetx = 0;
                    offsety++;
                    continue;
                }
                else if (s == " ")
                {

                }
                else
                {
                    sprite = SelectFont.GetAci(s);
                    sprite.SetWidth((float)sprite.GetWidth() * percent);
                    sprite.SetHeight((float)sprite.GetHeight() * percent);
                    sprite.SetPosition(x + offsetx * distance, y - offsety * h_distance);
                    renderer.DrawSprite(sprite);
                }
                offsetx++;
                if (offsetx >= ColMax)
                {
                    offsetx = 0;
                    offsety++;
                }
            }
        }
        public virtual void DrawString(Renderer renderer, string str, double x, double y, float percent,
            float distance, int ColMax, Color color)
        {
            if (!Binded)
                throw new Exception("当前工具没有绑定字体");
            if (!Binded)
                throw new Exception("当前工具没有绑定字体");
            Sprite sprite;
            Char[] Arr = str.ToCharArray();
            int count = Arr.Length;
            int offsetx = 0;
            int offsety = 0;
            float h_distance = percent * 24;
            for (int i = 0; i < count; i++)
            {
                string s = Arr[i].ToString();
                if (s == "\n")
                {
                    offsetx = 0;
                    offsety++;
                    continue;
                }
                else if (s == " ")  {}
                else
                {
                    sprite = SelectFont.GetAci(s);
                    sprite.SetWidth((float)sprite.GetWidth() * percent);
                    sprite.SetHeight((float)sprite.GetHeight() * percent);
                    sprite.SetPosition(x + offsetx * distance, y - offsety * h_distance);
                    sprite.SetColor(color);
                    renderer.DrawSprite(sprite);
                }
                offsetx++;
                if (offsetx >= ColMax)
                {
                    offsetx = 0;
                    offsety++;
                }
            }
        }
        
        public virtual void DrawString(Renderer renderer, string str, double x, double y, float percent,
            double Col_diatanceMax, Color color)
        {
            if (!Binded)
                throw new Exception("当前工具没有绑定字体");
            Sprite sprite;
            Char[] Arr = str.ToCharArray();
            int count = Arr.Length;
            double offsetx = 0;
            int offsety = 0;
            float h_distance = percent * 24;
            for (int i = 0; i < count; i++)
            {
                string s = Arr[i].ToString();
                if (s == "\n")
                {
                    offsetx = 0;
                    offsety++;
                    continue;
                }
                else if (s == " ") { offsetx += 24*percent; }
                else
                {
                    sprite = SelectFont.GetAci(s);
                    sprite.SetWidth((float)sprite.GetWidth() * percent);
                    sprite.SetHeight((float)sprite.GetHeight() * percent);
                    sprite.SetPosition(x + offsetx, y - offsety * h_distance);
                    sprite.SetColor(color);
                    double width = sprite.GetWidth();
                    if (width < 16 * percent)
                        offsetx += 16 * percent+1;
                    else
                        offsetx += width+1;       //至少保留1象素
                    renderer.DrawSprite(sprite);
                }
                if (offsetx >= Col_diatanceMax)
                {
                    offsetx = 0;
                    offsety++;
                }
            }
        }

        /// <summary>
        /// 绘制文本
        /// </summary>
        /// <param name="renderer">着色器</param>
        /// <param name="str">提供字符串</param>
        /// <param name="x">书写坐标</param>
        /// <param name="y">书写坐标</param>
        /// <param name="percent">字体显示大小所占百分比</param>
        /// <param name="Col_diatanceMax">列的最大范围</param>
        /// <param name="color">想要展示的颜色</param>
        /// <param name="CharacterPercent">展示字体的百分比</param>
        public virtual void DrawString(Renderer renderer, string str, double x, double y, float percent,
           double Col_diatanceMax, Color color ,float CharacterPercent)
        {
            if (!Binded)
                throw new Exception("当前工具没有绑定字体");
            Sprite sprite;
            Extra.VariableSprite Chac = new Extra.VariableSprite();          //可变倍率纹理精灵
            massege.SetString(str);                          //绑定字符串
            int CurrentCharIndex = 0;                        //当前字符所在索引项
            Char[] Arr = str.ToCharArray();
            int count = Arr.Length;
            double offsetx = 0;
            int offsety = 0;
            float h_distance = percent * 24;
            for (int i = 0; i < count; i++)
            {
                string s = Arr[i].ToString();
                if (s == "\n")
                {
                    offsetx = 0;
                    offsety++;
                    continue;
                }
                else if (s == " ") { offsetx += 24 * percent; CurrentCharIndex += 1; }
                else
                {
                    double showP = massege.GetAttribute(CharacterPercent, CurrentCharIndex);
                    sprite = SelectFont.GetAci(s);
                    Chac.BindSprite(sprite);
                    Chac.SetAttribute(x + offsetx, y - offsety * h_distance, showP,
                        (float)sprite.GetWidth() * percent, (float)sprite.GetHeight() * percent);
                    showP = showP * showP * showP;
                    Chac.SetColorVol(new Color(color.Red, color.Green, color.Blue, color.Alpha),
                        new Color(color.Red, color.Green, color.Blue, color.Alpha * (float)showP));
                    sprite = Chac.GetSprite();
                    double width = sprite.GetWidth();
                    if (width < 16 * percent)
                        offsetx += 16 * percent + 1;
                    else
                        offsetx += width + 1;       //至少保留1象素
                    renderer.DrawSprite(sprite);
                    CurrentCharIndex += 1;
                }
                if (offsetx >= Col_diatanceMax)
                {
                    offsetx = 0;
                    offsety++;
                }
            }
        }
    }

    /// <summary>
    /// 储存一条语句信息的设置信息。用于描述渐变动画
    /// </summary>
    public class TextMassege
    {
        String TargetString = "";
        int Lenth = 0;
        public TextMassege() { }
        public void SetString(string str)
        {
            TargetString = str;
            Lenth = str.Length;
        }
        public String GetString()
        {
            return TargetString;
        }
        /// <summary>
        /// 通过当前信息展现的百分比，来返回当前锁定字符所需要的百分比
        /// </summary>
        public double GetAttribute(double percent, int index)
        {
            if (index > Lenth)
                return 0;
            double r = percent * Lenth;
            int Ir = (int)r;
            if (index <= Ir-1)
                return 1;
            else if (index > Ir )
                return 0;
            r = r - Ir;
            return r;
        }
    }

}
