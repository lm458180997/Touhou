using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Battle.Stages.Stage1
{
    //战斗场景1的共享域空间（也可以使用构造依赖来完成，这里是防止每个实体的构造参数过长 ）
    public class BS1Shared
    {
        //静态分享资源
        public static TextureManager texturemanager;
        //静态描述玩家信息
        public static Player player1;        
        public static Player player2;

        /// <summary>
        /// 静态构造函数，用于数据的初始化
        /// </summary>
        static BS1Shared()
        {
            initcomponents();
        }


        static void initcomponents()
        {
            
        }

        /// <summary>
        /// 绑定纹理管理器
        /// </summary>
        /// <param name="_t">纹理管理器</param>
        public static void SetTexturemanager(TextureManager _t)
        {
            texturemanager = _t;
        }
       


    }
}
