using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{

    public enum GameLevel
    {
        easy =0, 
        normal ,
        hard , 
        lunatic,
        extra
    }
    //记录当前的全局数据
    public class Datas
    {
        static Datas()
        {
            GameRandom = new GRandom();
        }

        //游戏的难度
        public  static GameLevel _GameLevel = GameLevel.normal; 
        //游戏的选择人物
        public static Dictionary<string, People> Charactors = new Dictionary<string, People>();     //能操纵的所有角色
        public static string Fore_People_Select = "ReiMu";
        public static string Back_People_Select = "YuKaRi";

        public static int Hp_Start = 3;       //初始血量 (玩家数据，从磁盘上读取)
        public static int Hp = 3;             //血量
        public static int booms = 3;          //初始的booms数量
        public static int currentbooms = 3;   //当前的booms数量
        public static int PowerPoint;         //当前的P点数

        public static bool ReFre = false ;     //是否处于重播模式
        public static GRandom GameRandom ;      //游戏的随机器（注意需要记录其种子）
        public static Random LooselyRandom = new Random();   //使用时机不严谨的随机数生成器（可随时调用，不会影响系统逻辑）     

        public static List<string> Commands;              //从磁盘上读取的命令队列(或者是在运行游戏的过程中所记录的命令队列)

        public static List<Enemy> CurrentEnemys;          //当前的敌人列表，记录地址
        public static List<Item> CurrentItemAdd;          //当前的道具（添加）列表

        public static Player CurrentPlayer;               //当前的游戏人物（玩家）

        public static SoundManagerEx SoundManager_Static; //共享的声音处理器


        public static void WirteData(string command)
        {
            Commands.Add(command);
        }

        public static void PrintData(string path = "rf.dat")
        {
            System.IO.StreamWriter write = new System.IO.StreamWriter(path,false);
            foreach(string cmd in Commands)
            {
                write.WriteLine(cmd);
            }
            write.Dispose();
        }

        public static void ReadData(string path = "rf.dat")
        {
            Commands = new List<string>();
            System.IO.StreamReader read = new System.IO.StreamReader(path);
            while (!read.EndOfStream)
            {
                Commands.Add(read.ReadLine());
            }
        }

    }

    public class GRandom
    {
        public Random random;
        int seed;
        public GRandom()
        {
            long s = DateTime.Now.Ticks;
            seed = (int)(s % 100000000);
            random = new Random(seed);
        }
        public GRandom(int seed)
        {
            this.seed = seed;
            random = new Random(seed);
        }

        public int Seed
        {
            get { return seed; }
            set 
            {
                seed = value;
                random = new Random(seed);
            }
        }
        public double NextDouble()
        {
            return random.NextDouble();
        }

    }



}


