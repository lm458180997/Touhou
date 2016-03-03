using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Entities.Players
{
    //因为会遇到速度放缓的特效，所有有关的变幻内容应该结合Update(ElapsedTime)
    public class  WriggleCmp1 : CollisionComponent
    {

        Vector2D Position;                    //坐标
        Vector2D FollowPosition;              //跟踪的目标坐标
        List<Sprite> Sprites;                 //动画的纹理
        int PicCount = 20;                    //使用图片的张书
        int firstIndex = 0;                   //循环动画的首编号
        int lastIndex = 19;                   //循环动画的末编号
        int currentIndex = 0;                 //当前帧所使用的编号
        double caculateTime = 0;              //从生成开始的有效记录时间
        double changeInterval = 0.01;         //帧数切换的速率
        double changeTimer = 0;               //帧数切换计时器
        float width = 128;                    //显示宽度
        float height = 128;                   //显示高度
        float attackRadius = 64;              //攻击的判断半径^2
        float AttckRadiusDouble = 4096;       //两倍判断半径
        float JudgeBegin = 160;                //弧形判定的起始角（逆时针）
        float JudgeEnd = 180;                  //弧形判定的目标角度（逆时针）
        float AverageAngle = 17;               //平均每帧的变化角度
        float JudgeArea = 30;                 //角度判定域[单边]（在区域以内的才视为正确判定）

        /// <summary>
        /// 莉格露的第一段技能，横扫一圈的攻击
        /// </summary>
        /// <param name="pos"></param>
        public WriggleCmp1(TextureManager texturemanager,  Vector2D pos,int AttackRadius =60)
        {
            name = "WriggleCmp1";
            Sprites = new List<Sprite>();
            Position = new Vector2D(pos.X, pos.Y);
            FollowPosition = pos;

            AverageAngle = (360 - (JudgeEnd - JudgeBegin))/PicCount;

            SetAttackRaiuds(AttackRadius);
            initTextures(texturemanager);
        }

        /// <summary>
        /// 设定技能判定半径
        /// </summary>
        /// <param name="r">半径</param>
        public void SetAttackRaiuds(float r)
        {
            attackRadius = r;
            width = 2 * r;
            height = width;
            AttckRadiusDouble = attackRadius * attackRadius;
        }

        void initTextures(TextureManager texturemanager)
        {
            string head = "WriggleAttack1_st";
            string middle = "";
            string num;
            Sprite sp;
            for (int i = 1; i <= PicCount; i++)
            {
                num = i.ToString();
                if (num.Length == 2)
                {
                    middle = "0" + num;
                }
                else
                    middle = "00" + num;
                sp = new Sprite();
                sp.Texture = texturemanager.Get(head + middle);
                sp.SetWidth(32);
                sp.SetHeight(32);
                Sprites.Add(sp);
            }
        }

        public void BindFollowPosition(Vector2D pos)
        {
            FollowPosition = pos;
        }

        public override bool Collision(Bullet bullet, ref MessageManager msgManager)
        {
            float x = (float)bullet.Position.X;
            float y = (float)bullet.Position.Y;
            double distance = (Position.X - x) * (Position.X - x) + (Position.Y - y) * (Position.Y - y);
            if (distance > AttckRadiusDouble)
            {
                return false;
            }
            Vector2D vct = new Vector2D(Position.X - x, Position.Y - y);
            vct.Normalize();
            double ag = vct.GetAngle();
            int changecount = currentIndex - firstIndex;
            double oriag = JudgeBegin - AverageAngle * changecount;
            if (oriag < 0)
            {
                oriag = 360 + oriag;
                if (oriag + JudgeArea > 360)
                {
                    if (ag < (oriag - JudgeArea) && ag > (oriag + JudgeArea - 360))
                        return false;
                }
                else
                {
                    if (ag < (oriag - JudgeArea) && ag > (oriag + JudgeArea))
                        return false;
                }
            }
            else
            {
                if (oriag - JudgeArea < 0)
                {
                    if (ag < (360 + oriag - JudgeArea) && ag > (oriag + JudgeArea))
                    {
                        return false;
                    }
                }
                else
                {
                    if (ag < (oriag - JudgeArea) && ag > (oriag + JudgeArea))
                        return false;
                }
            }
            CollisionMessages.CollisionMasseageNormal mess = new CollisionMessages.CollisionMasseageNormal();
            mess.Position.X = Position.X;
            mess.Position.Y = Position.Y;
            mess.SetColor(0);
            msgManager.AcceptMessage(mess);

            return true;
        }

        public override bool Collision(Enemy e, ref MessageManager msg)
        {
            return base.Collision(e, ref msg);
        }

        void ChangeFlash()
        {
            int index = currentIndex + 1;
            if (index > lastIndex)
            {
                currentIndex = firstIndex;
                //过时删除组件
                this.working = false;
                this.disabled = true;              
            }
            else
                currentIndex = index;
        }

        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);
            caculateTime += elapsedTime;

            changeTimer += elapsedTime;
            while (changeTimer >= changeInterval)
            {
                changeTimer -= changeInterval;
                ChangeFlash();
            }
            Position.X = FollowPosition.X;
            Position.Y = FollowPosition.Y;

        }

        public override void Render(Renderer renderer)
        {
            base.Render(renderer);
            Sprite sp= Sprites[currentIndex];
            sp.SetWidth(width);
            sp.SetHeight(height);
            sp.SetColor(new Color(1, 1, 1, 0.8f));
            sp.SetPosition(Position.X,Position.Y);
            renderer.DrawSprite(sp);

        }

    }


}
