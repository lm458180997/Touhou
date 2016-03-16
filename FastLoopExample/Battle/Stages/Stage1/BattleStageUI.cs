using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Battle
{
    //用于显示前景信息（血量，能量，计分以及时间等内容）
    public class BattleStageUI : IGameObject
    {

        //纹理管理器
        public TextureManager texturemanager;

        /// <summary>
        /// 战斗场景的UI设置
        /// </summary>
        /// <param name="tmng">纹理管理器</param>
        public BattleStageUI(TextureManager tmng)
        {
            texturemanager = tmng;

        }

        /// <summary>
        /// 实体载入时调用
        /// </summary>
        public void Start()
        {

        }

        /// <summary>
        /// 逻辑更新
        /// </summary>
        /// <param name="elapsedTime">帧间插值</param>
        public void Update(double elapsedTime)
        {

        }

        /// <summary>
        /// 渲染函数
        /// </summary>
        public void Render()
        {

        }

        /// <summary>
        /// 渲染函数，参数提供着色器
        /// </summary>
        /// <param name="renderer"></param>
        public void Render(Renderer renderer)
        {

        }

    }

}
