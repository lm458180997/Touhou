using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{

    public class MessageManager
    {
        public List<CmpMessage> Buffers;
        public List<CmpMessage> Messages;
        public MessageManager()
        {
            Messages = new List<CmpMessage>();
            Buffers = new List<CmpMessage>();
        }
        /// <summary>
        /// 获得信息
        /// </summary>
        /// <param name="msg"></param>
        public void AcceptMessage(CmpMessage msg)
        {
            Messages.Add(msg);
        }
        /// <summary>
        /// 清空信息
        /// </summary>
        public void ClearMessage()
        {
            Messages.Clear();
        }
        /// <summary>
        /// 是否信息队列为空
        /// </summary>
        /// <returns>是否信息队列为空</returns>
        public bool IsEmpty()
        {
            return Messages.Count == 0;
        }
        /// <summary>
        /// 根据标签筛选信息
        /// </summary>
        /// <param name="Tag">Tag标签</param>
        /// <returns>返回筛选出的信息</returns>
        public CmpMessage[] SelectMessages(int Tag , bool deleteDatas = true)
        {
            foreach (CmpMessage msg in Messages)
            {
                Buffers.Add(msg);
            }
            int count = Buffers.Count;
            CmpMessage[] msgs = new CmpMessage[count];
            for (int i = 0; i < count; i++)
            {
                msgs[i] = Buffers[i];
                //如果需要删除读取过的信息，则从缓冲区删除[当信息种类很多的时候能有效提高效率]
                if (deleteDatas)
                {
                    Messages.Remove(Buffers[i]);
                }
            }
            Buffers.Clear();
            return msgs;
        }


    }

    public class CmpMessage         //组件之间传递的信息[基]
    {
        public const int NONE = 0, CollisionMasseageNormal =1;
        public string Msg;          //报告信息
        public int Tag = NONE;      //报告信息标签
    }

    namespace CollisionMessages
    {
        ///一般的碰撞信息Message ， 携带颜色数据
        public class CollisionMasseageNormal: CmpMessage
        {
            //传递的输出方的坐标信息
            public Vector2D Position = new Vector2D(0,0);
            //颜色数据，由0-10来代表不同的颜色
            int color = 0;
            //方向偏转数据：int 
            int Angle = 0;
            /// <summary>
            /// 一般的碰撞信息Message ， 携带颜色数据
            /// </summary>
            public CollisionMasseageNormal()
            {
                Tag = CollisionMasseageNormal;
                Msg = "CollisionMasseageNormal";
            }
            /// <summary>
            /// 设置颜色
            /// </summary>
            /// <param name="c">由0-9分别代表不同的颜色</param>
            public void SetColor(int c)
            {
                if (c < 10)
                    color = c;
            }
            /// <summary>
            /// 获得当前的颜色
            /// </summary>
            /// <returns>颜色数据</returns>
            public int GetColor()
            {
                return color;
            }
            /// <summary>
            /// 获得角度偏转值
            /// </summary>
            /// <returns>角度偏转值</returns>
            public int GetAngle()
            {
                return Angle;
            }

        }

    }
}
