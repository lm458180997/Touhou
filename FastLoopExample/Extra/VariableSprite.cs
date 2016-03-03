using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Extra
{
    /// <summary>
    /// 可变Sprite ，提供可变化的sprite（基于一个已存在的sprite）
    /// </summary>
    public class VariableSprite : ImportSprite
    {
        public Vector2D Postion= new Vector2D();
        public float Uvs1, Uvs2, Uvs3, Uvs4;      //绑定sprite中固有的Uvs属性
        public Sprite CurrentSprite;              //当前已经绑定了的Sprite副本对象
        public double width, height;               //原生width、hight属性
        public double ShowPercent =1;                //展示出的百分比[默认为横向从左到右]
        bool binded= false;
        public bool Binded { get { return binded; } }

        public VariableSprite(){}

        /// <summary>
        /// 可变Sprite
        /// </summary>
        /// <param name="orisprite">原生绑定对象</param>
        public VariableSprite(Sprite orisprite)
        {
            BindSprite(orisprite);
        }
        public void setsize(double w, double h)
        {
            width = w;
            height = h;
        }

        public Sprite SetAttribute(double x, double y, double percent)
        {
            ShowPercent = percent;
            double w = ShowPercent * width;
            CurrentSprite.SetUVs(Uvs1, Uvs2, Uvs1 + (Uvs3 - Uvs1) * (float)ShowPercent, Uvs4);
            CurrentSprite.SetWidth((float)w);
            CurrentSprite.SetHeight((float)height);
            CurrentSprite.SetPosition(x - 0.5f * width * (1 - ShowPercent), y);     //平移至边上
            return CurrentSprite;
        }
        public Sprite SetAttribute(double x, double y, double percent, double w, double h , int type = 0)
        {
            width = w;
            height = h;
            ShowPercent = percent;
            double _w = ShowPercent * width;
            CurrentSprite.SetUVs(Uvs1, Uvs2, Uvs1 + (Uvs3 - Uvs1) * (float)ShowPercent, Uvs4);
            CurrentSprite.SetWidth((float)_w);
            CurrentSprite.SetHeight((float)h);
            CurrentSprite.SetPosition(x - 0.5f * w * (1 - ShowPercent), y);     //平移至边上
            return CurrentSprite;
        }

        public void SetColorVol(Color left, Color right)
        {
            CurrentSprite.SetColorLeft(left);
            CurrentSprite.SetColorRight(right);
        }
        


        public void BindSprite(Sprite sp)
        {
            CurrentSprite = new Sprite();
            Uvs1 = sp.Uvs1;
            Uvs2 = sp.Uvs2;
            Uvs3 = sp.Uvs3;
            Uvs4 = sp.Uvs4;
            width = sp.GetWidth();
            height = sp.GetHeight();
            CurrentSprite.Texture = sp.Texture;
            binded = true;
        }

        //供应接口
        public Sprite GetSprite()
        {
            return CurrentSprite;
        }

    }
}
