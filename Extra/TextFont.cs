using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{
    public struct Charactor
    {
        public string id;
        public int x, y;
        public Charactor(int x, int y, string value)
            : this()
        {
            id = value; this.x = x; this.y = y;
        }
    }
    public class TextFont : CharFont
    {
        TextureManager _textureManager;
        Dictionary<string, Sprite> Font_Sprites = new Dictionary<string, Sprite>();
        List<Charactor> charactors = new List<Charactor>();
        public TextFont(TextureManager textureManager)
        {
            _textureManager = textureManager;
            AddCharactors();
            CreateSprites();
        }
        void AddCharactors()
        {
            charactors.Add(new Charactor(1, 0, "!"));
            charactors.Add(new Charactor(2, 0, "\""));
            charactors.Add(new Charactor(3, 0, "#"));
            charactors.Add(new Charactor(4, 0, "$"));
            charactors.Add(new Charactor(5, 0, "%"));
            charactors.Add(new Charactor(6, 0, "&"));
            charactors.Add(new Charactor(7, 0, "'"));
            charactors.Add(new Charactor(8, 0, "("));
            charactors.Add(new Charactor(9, 0, ")"));
            charactors.Add(new Charactor(10, 0, "*"));
            charactors.Add(new Charactor(11, 0, "+"));
            charactors.Add(new Charactor(12, 0, ","));
            charactors.Add(new Charactor(13, 0, "-"));
            charactors.Add(new Charactor(14, 0, "."));
            charactors.Add(new Charactor(15, 0, "/"));
            charactors.Add(new Charactor(16, 0, "0"));
            charactors.Add(new Charactor(17, 0, "1"));

            charactors.Add(new Charactor(0, 1, "2"));
            charactors.Add(new Charactor(1, 1, "3"));
            charactors.Add(new Charactor(2, 1, "4"));
            charactors.Add(new Charactor(3, 1, "5"));
            charactors.Add(new Charactor(4, 1, "6"));
            charactors.Add(new Charactor(5, 1, "7"));
            charactors.Add(new Charactor(6, 1, "8"));
            charactors.Add(new Charactor(7, 1, "9"));
            charactors.Add(new Charactor(8, 1, ":"));
            charactors.Add(new Charactor(9, 1, ";"));
            charactors.Add(new Charactor(10, 1, "<"));
            charactors.Add(new Charactor(11, 1, "="));
            charactors.Add(new Charactor(12, 1, ">"));
            charactors.Add(new Charactor(13, 1, "?"));
            charactors.Add(new Charactor(14, 1, "@"));
            charactors.Add(new Charactor(15, 1, "A"));
            charactors.Add(new Charactor(16, 1, "B"));
            charactors.Add(new Charactor(17, 1, "C"));

            charactors.Add(new Charactor(0, 2, "D"));
            charactors.Add(new Charactor(1, 2, "E"));
            charactors.Add(new Charactor(2, 2, "F"));
            charactors.Add(new Charactor(3, 2, "G"));
            charactors.Add(new Charactor(4, 2, "H"));
            charactors.Add(new Charactor(5, 2, "I"));
            charactors.Add(new Charactor(6, 2, "J"));
            charactors.Add(new Charactor(7, 2, "K"));
            charactors.Add(new Charactor(8, 2, "L"));
            charactors.Add(new Charactor(9, 2, "M"));
            charactors.Add(new Charactor(10, 2, "N"));
            charactors.Add(new Charactor(11, 2, "O"));
            charactors.Add(new Charactor(12, 2, "P"));
            charactors.Add(new Charactor(13, 2, "Q"));
            charactors.Add(new Charactor(14, 2, "R"));
            charactors.Add(new Charactor(15, 2, "S"));
            charactors.Add(new Charactor(16, 2, "T"));
            charactors.Add(new Charactor(17, 2, "U"));

            charactors.Add(new Charactor(0, 3, "V"));
            charactors.Add(new Charactor(1, 3, "W"));
            charactors.Add(new Charactor(2, 3, "X"));
            charactors.Add(new Charactor(3, 3, "Y"));
            charactors.Add(new Charactor(4, 3, "Z"));
            charactors.Add(new Charactor(5, 3, "["));
            charactors.Add(new Charactor(6, 3, "\\"));
            charactors.Add(new Charactor(7, 3, "]"));
            charactors.Add(new Charactor(8, 3, "^"));
            charactors.Add(new Charactor(9, 3, "_"));
            charactors.Add(new Charactor(10, 3, " "));
            charactors.Add(new Charactor(11, 3, "a"));
            charactors.Add(new Charactor(12, 3, "b"));
            charactors.Add(new Charactor(13, 3, "c"));
            charactors.Add(new Charactor(14, 3, "d"));
            charactors.Add(new Charactor(15, 3, "e"));
            charactors.Add(new Charactor(16, 3, "f"));
            charactors.Add(new Charactor(17, 3, "g"));

            charactors.Add(new Charactor(0, 4, "h"));
            charactors.Add(new Charactor(1, 4, "i"));
            charactors.Add(new Charactor(2, 4, "j"));
            charactors.Add(new Charactor(3, 4, "k"));
            charactors.Add(new Charactor(4, 4, "l"));
            charactors.Add(new Charactor(5, 4, "m"));
            charactors.Add(new Charactor(6, 4, "n"));
            charactors.Add(new Charactor(7, 4, "o"));
            charactors.Add(new Charactor(8, 4, "p"));
            charactors.Add(new Charactor(9, 4, "q"));
            charactors.Add(new Charactor(10, 4, "r"));
            charactors.Add(new Charactor(11, 4, "s"));
            charactors.Add(new Charactor(12, 4, "t"));
            charactors.Add(new Charactor(13, 4, "u"));
            charactors.Add(new Charactor(14, 4, "v"));
            charactors.Add(new Charactor(15, 4, "w"));
            charactors.Add(new Charactor(16, 4, "x"));
            charactors.Add(new Charactor(17, 4, "y"));

            charactors.Add(new Charactor(0, 5, "z"));
            charactors.Add(new Charactor(1, 5, "{"));
            charactors.Add(new Charactor(2, 5, "|"));
            charactors.Add(new Charactor(3, 5, "}"));
            charactors.Add(new Charactor(4, 5, "~"));
            charactors.Add(new Charactor(5, 5, "口"));
            charactors.Add(new Charactor(6, 5, "终"));
         
        }
        void CreateSprites()
        {
            Texture texture = _textureManager.Get("Font");
            foreach(Charactor c in charactors)
            {
                Sprite spt = new Sprite();
                spt.Texture = texture;
                spt.SetUVs(new Point(0.05467f*c.x,0.05467f*c.y),
                    new Point(0.05467f*c.x+0.05467f,0.05467f*c.y+0.05467f));
                Font_Sprites.Add(c.id, spt);
            }
        }

        public Sprite GetAci(string key)
        {
            if (!Font_Sprites.ContainsKey(key)) return Font_Sprites["_"];
            return new Sprite(Font_Sprites[key]);
        }
       

    }
}
