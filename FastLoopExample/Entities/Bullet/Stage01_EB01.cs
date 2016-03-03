using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{

    //Stage01_EB01类型子弹
    public class Stage01_EB01 : Bullet
    {
        float radius = 4;                         //判定半径
        Controler contorler;
        TextureManager texturemanager;
        public Stage01_EB01(TextureManager _t, Vector2D direction , Vector2D position,int color = 5)
        {
            texturemanager = _t;
            Direction.X = direction.X; Position.X = position.X;
            Direction.Y = direction.Y; Position.Y = position.Y;
            bulletbody = new BulletBodys.RoundB_B(_t, color);                        //敌机子弹皮肤（miniRice）
            speed = 100;
            BulletData bulletdata = new BulletData(Position , Direction , Scale , speed);
            contorler = new Entities.Controlors.DirectControler(bulletdata);      //选择直线控制器
            renderlevel = 0;

            //需要提前约定好
            MissTick = 61;
            MissRefresh = 60;
        }
        public override void Render()
        {
            float ag = Direction.getcurve();
            bulletbody.sprite.SetPosition(Position.X, Position.Y);
            Stage1State._renderer.DrawSprite(bulletbody.sprite, Position.X, Position.Y, ag);
        }
        public override void RenderByLevel()
        {
            Render();
        }
       
        public override void Hit(Entity loader)
        {
            working = false;            //停止工作
            disabled = true;            //允许销毁
        }
        public override void update(double ElapsedTime)
        {
            if (working)
                contorler.Update(ElapsedTime);
            if (Position.X < -200 || Position.X > 200 || Position.Y < -18 || Position.Y > 500)
            {
                speed = 0;
                working = false;
                disabled = true;
            }
        }
        double lenth = 0; 
        bool missed = false;
        Vector2D missedPosition;
        public override bool Collision(Collider c)
        {
            if (working)
                if (c is RoundCollider)
                {
                    RoundCollider rd = (RoundCollider)c;
                    double missDistance = 15;
                    if (Datas.CurrentPlayer.CurrentCharactor.Name == "")
                    {
                        missDistance = 25;
                    }
                    double r = rd.GetRadius();
                    double x = rd.GetX();
                    double y = rd.GetY();
                    lenth = ((x - Position.X) * (x - Position.X) + (y - Position.Y) * (y - Position.Y));
                    if (lenth < (missDistance + radius) * (missDistance + radius))
                    {
                        //擦弹
                        missed = true;
                        missedPosition = new Vector2D(x - Position.X, y - Position.Y).GetNormalize();
                        if (lenth < (r + radius) * (r + radius))
                        {
                            return true;
                        }
                    }
                }
            return false;
        }
        //判定距离，玩家坐标，反馈位置矢量(生成道具所需要的数据)
        protected override bool MissJudge(double missDistance, double x, double y, ref Vector2D vct, ref Item itm)
        {
            if (missed)
            {
                vct.X = missedPosition.X; vct.Y = missedPosition.Y;
                itm = new MissPoint(texturemanager, x, y, missedPosition, 1200);
                missed = false;
                return true;
            }
            return false;
        }
    }

    //Stage01_EB01类型子弹
    public class Stage01_EB02 : Bullet
    {
        float radius = 4;                         //判定半径
        Controler contorler;
        TextureManager texturemanager;
        public Stage01_EB02(TextureManager _t, Vector2D direction, Vector2D position, int color = 5)
        {
            texturemanager = _t;
            Direction.X = direction.X; Position.X = position.X;
            Direction.Y = direction.Y; Position.Y = position.Y;
            bulletbody = new BulletBodys.RoundB_B(_t, color);                        //敌机子弹皮肤（miniRice）
            speed = 100;
            BulletData bulletdata = new BulletData(Position, Direction, Scale, speed);
            contorler = new Entities.Controlors.DirectControler(bulletdata);      //选择直线控制器
            renderlevel = 0;

            //需要提前约定好
            MissTick = 61;
            MissRefresh = 60;
        }
        public override void Render()
        {
            float ag = Direction.getcurve();
            bulletbody.sprite.SetPosition(Position.X, Position.Y);
            Stage1State._renderer.DrawSprite(bulletbody.sprite, Position.X, Position.Y, ag);
        }
        public override void RenderByLevel()
        {
            Render();
        }

        public override void Hit(Entity loader)
        {
            working = false;            //停止工作
            disabled = true;            //允许销毁
        }
        public override void update(double ElapsedTime)
        {
            if (working)
                contorler.Update(ElapsedTime);
            if (Position.X < -200 || Position.X > 200 || Position.Y < -18 || Position.Y > 500)
            {
                speed = 0;
                working = false;
                disabled = true;
            }
        }
        double lenth = 0;
        bool missed = false;
        Vector2D missedPosition;
        public override bool Collision(Collider c)
        {
            if (working)
                if (c is RoundCollider)
                {
                    RoundCollider rd = (RoundCollider)c;
                    double missDistance = 15;
                    if (Datas.CurrentPlayer.CurrentCharactor.Name == "")
                    {
                        missDistance = 25;
                    }
                    double r = rd.GetRadius();
                    double x = rd.GetX();
                    double y = rd.GetY();
                    lenth = ((x - Position.X) * (x - Position.X) + (y - Position.Y) * (y - Position.Y));
                    if (lenth < (missDistance + radius) * (missDistance + radius))
                    {
                        //擦弹
                        missed = true;
                        missedPosition = new Vector2D(x - Position.X, y - Position.Y).GetNormalize();
                        if (lenth < (r + radius) * (r + radius))
                        {
                            return true;
                        }
                    }
                }
            return false;
        }
        //判定距离，玩家坐标，反馈位置矢量(生成道具所需要的数据)
        protected override bool MissJudge(double missDistance, double x, double y, ref Vector2D vct, ref Item itm)
        {
            if (missed)
            {
                vct.X = missedPosition.X; vct.Y = missedPosition.Y;
                itm = new MissPoint(texturemanager, x, y, missedPosition, 1200);
                missed = false;
                return true;
            }
            return false;
        }
    }

    namespace Stage1
    {
        public class Bullet1 : Bullet
        {
            float radius = 4;                         //判定半径
            Controler contorler;
            TextureManager texturemanager;
            //----------------死亡处理过程----------------//
            bool isdead = false;                   //当子弹已经死亡，仅剩下特效时进行标记
            Effect effect;
            void AddUpdate(double ElapsedTime)     //追加更新命令
            {
                if (isdead)
                {
                    effect.Update(ElapsedTime);
                    if (effect.disposed)
                    {
                        disabled = true;
                    }
                }
            }
            void AddRender()
            {
                if (isdead)
                {
                    effect.Render(Stage1State._renderer);
                }
            }
            //----------------死亡处理过程----------------//


            public Bullet1(TextureManager _t, Vector2D direction, Vector2D position, int color = 5,float _speed = 100)
            {
                texturemanager = _t;
                Direction.X = direction.X; Position.X = position.X;
                Direction.Y = direction.Y; Position.Y = position.Y;
                bulletbody = new BulletBodys.BigButterfly(_t, 3);                        //敌机子弹皮肤（miniRice）
                speed = _speed;
                BulletData bulletdata = new BulletData(Position, Direction, Scale, speed);
                contorler = new Entities.Controlors.DirectControler(bulletdata);      //选择直线控制器
                renderlevel = 0;

                //需要提前约定好
                MissTick = 61;
                MissRefresh = 60;
            }
            public override void Render()
            {
                if (working)
                {
                    float ag = Direction.getcurve();
                    bulletbody.sprite.SetPosition(Position.X, Position.Y);
                    Stage1State._renderer.DrawSprite(bulletbody.sprite, Position.X, Position.Y, ag);
                }
                //如果已经死亡，则生成死亡动画
                AddRender();
            }
            public override void RenderByLevel()
            {
                Render();
            }

            public override void Hit(Entity loader)
            {
                working = false;            //停止工作
                disabled = true;            //允许销毁
            }
            public override void update(double ElapsedTime)
            {
                AddUpdate(ElapsedTime);
                if (working)
                    contorler.Update(ElapsedTime);
                if (Position.X < -200 || Position.X > 200 || Position.Y < -18 || Position.Y > 500)
                {
                    speed = 0;
                    working = false;
                    disabled = true;
                }
            }
            double lenth = 0;
            bool missed = false;
            Vector2D missedPosition;
            public override bool Collision(Collider c)
            {
                if (working)
                    if (c is RoundCollider)
                    {
                        RoundCollider rd = (RoundCollider)c;
                        double missDistance = 15;
                        if (Datas.CurrentPlayer.CurrentCharactor.Name == "")
                        {
                            missDistance = 25;
                        }
                        double r = rd.GetRadius();
                        double x = rd.GetX();
                        double y = rd.GetY();
                        lenth = ((x - Position.X) * (x - Position.X) + (y - Position.Y) * (y - Position.Y));
                        if (lenth < (missDistance + radius) * (missDistance + radius))
                        {
                            //擦弹
                            missed = true;
                            missedPosition = new Vector2D(x - Position.X, y - Position.Y).GetNormalize();
                            if (lenth < (r + radius) * (r + radius))
                            {
                                return true;
                            }
                        }
                    }
                return false;
            }
            //判定距离，玩家坐标，反馈位置矢量(生成道具所需要的数据)
            protected override bool MissJudge(double missDistance, double x, double y, ref Vector2D vct, ref Item itm)
            {
                if (missed)
                {
                    vct.X = missedPosition.X; vct.Y = missedPosition.Y;
                    itm = new MissPoint(texturemanager, x, y, missedPosition, 1200);
                    missed = false;
                    return true;
                }
                return false;
            }


            //-----------------以下为命令修改------------------//
            //修改后的信息处理函数
            protected override void DoMessages(double elapsedTime)
            {
                if (!working)
                    return;
                foreach (CmpMessage msg in ComponentMessages)
                {
                    //0号攻击判定
                    if (msg.Tag == CmpMessage.CollisionMasseageNormal)
                    {
                        this.working = false;
                        //进入死亡模式
                        isdead = true;
                        CollisionMessages.CollisionMasseageNormal m =
                        msg as CollisionMessages.CollisionMasseageNormal;
                        double x, y, x1, y1;
                        x = Position.X; y = Position.Y;
                        x1 = m.Position.X; y1 = m.Position.Y;
                        Vector2D vct = new Vector2D();
                        vct.X = x - x1;
                        vct.Y = y - y1;
                        vct.Normalize();
                        effect = new Entities.WindBlueEffect(texturemanager, Position,
                        360-(float)vct.GetAngle(),40,40);
                    }
                }
                ComponentMessages.Clear();
            }
        }

    }

}
