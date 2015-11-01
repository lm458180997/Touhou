using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{
    public interface CharFont
    {
        Sprite GetAci(string key);
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
    }


}
