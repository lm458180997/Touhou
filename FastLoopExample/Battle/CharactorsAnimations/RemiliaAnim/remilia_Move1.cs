using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Battle.CharactorsAnimations
{
    /// <summary>
    /// 纹理帮助工具（用于快速部署纹理资源）
    /// </summary>
    public class BindTextureTools
    {
        /// <summary>
        /// 给BattleAnimation快速部署纹理资源
        /// </summary>
        /// <param name="_t">纹理管理器</param>
        /// <param name="name">纹理的普遍头命名</param>
        /// <param name="Sprites">需要部署的sprite数组</param>
        /// <param name="startindex">第一张纹理的序号</param>
        /// <param name="count">部署的纹理张数</param>
        public static void BindTexures(TextureManager _t, string name, Sprite[] Sprites, 
            int startindex,int count,int width = 32, int height = 32)
        {
            string head = name;
            string middle = "";
            string num;
            Sprite sp;
            for (int i = 0; i < count; i++)
            {
                num = i.ToString();
                if (num.Length == 2)
                {
                    middle = "0" + num;
                }
                else
                    middle = "00" + num;
                sp = new Sprite();
                sp.Texture = _t.Get(head + middle);
                sp.SetWidth(width);
                sp.SetHeight(height);
                Sprites[i+startindex] = sp;
            }
        }
    }


    /// <summary>
    /// 蕾米利亚的一般站立动画
    /// </summary>
    public class remilia_stand : BattleAnimation 
    {
        public remilia_stand(TextureManager texturemanager)
        {
            //站立动画一般都为循环式的
            needloop = true;
            //实例化动画数组
            Sprites = new Sprite[8];
            //设定起始索引，以及最后索引（这里举例为0-7）
            startIndex = 0;
            lastIndex = 7;
            name = "data_character_remilia_stand";
            //调用工具函数快速部署8张纹理至Sprites
            BindTextureTools.BindTexures(texturemanager, name, Sprites, 0, 8);
        }
        /// <summary>
        /// 动画更新
        /// </summary>
        /// <param name="ElapsedTime">帧间时间</param>
        public override void AnimUpdate(double ElapsedTime)
        {
            caculatetime += ElapsedTime;
            //每过一个时间[changeinterval]间隔更新一帧图片
            if (caculatetime >= changeinterval)
            {
                caculatetime -= changeinterval;
                if (CurrentIndex < lastIndex)
                {
                    CurrentIndex++;
                }
                else
                {
                    if (needloop)
                    {
                        CurrentIndex = startIndex;
                    }
                    else
                    {
                        CurrentIndex = lastIndex;
                        IsOver = true;
                    }
                }
            }
        }
        /// <summary>
        /// 获取Sprite
        /// </summary>
        /// <returns>返回当前sprite</returns>
        public override Sprite GetSprite()
        {
           Sprite sp= Sprites[CurrentIndex];
           sp.SetWidth(128);
           sp.SetHeight(128);
           return sp;
        }

    }

}
