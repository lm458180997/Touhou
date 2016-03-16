using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Battle.Players
{

    public class BattlePlayer : Entity
    {

        public BattlePeoples CurrentCharactor;    //当前角色

        public int PlayerType = 0;         //人物的类型，0——player1 ，1——player2
       
        public BattlePlayer(TextureManager texturemanager,int type = 0)
        {
            //玩家类型（为Player1，或是为Player2）
            PlayerType = type;

            //生成一个人物数据实例
            BattlePeoples p = new Battle.Players.Remilia(texturemanager);

            //注册人物
            RegisteredRCharactors(p);

        }

        /// <summary>
        /// 注册人物
        /// </summary>
        /// <param name="fc">人物</param>
        public void RegisteredRCharactors(BattlePeoples fc)
        {
            CurrentCharactor = fc;
        }
        /// <summary>
        /// Start初始化
        /// </summary>
        public override void Start()
        {
            base.Start();
            CurrentCharactor.Start();
        }

        public override void Render()
        {
            base.Render();
            CurrentCharactor.Render();

        }

        public override void Update(double elapsedTime)
        {
            base.Update(elapsedTime);

            // CurrentCharactor  Update
            CurrentCharactor.Update(elapsedTime);

        }

        /// <summary>
        /// 被击中后的处理
        /// </summary>
        /// <returns>返回是否成功打击</returns>
        public bool Hitted()
        {

            return false;
        }

        //更新连击（产生攻击间隔判定）
        void UpdateAttack(double elapsedTime)
        {
            
        }

        //攻击连击
        public void Attack()
        {
            
        }
        /// <summary>
        /// 攻击判定
        /// </summary>
        /// <param name="p">传递的敌人数据</param>
        /// <param name="msg">消息管理机</param>
        /// <returns></returns>
        public bool AttackCollision(Player p, ref MessageManager msg)
        {
            return CurrentCharactor.AttackCollision(p, ref msg);
        }

    }

}
