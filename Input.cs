using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{
    public class Pac_boolean
    {
        public bool value = false;
        public Pac_boolean(bool v)
        {
            value = v;
        }
    }
    public class Input
    {

        static Dictionary<string, Pac_boolean> Keys_Down = new Dictionary<string,Pac_boolean>();
        static Dictionary<string, Pac_boolean> Keys_Up = new Dictionary<string,Pac_boolean>();
        static Input()
        {
            Keys_Down = new Dictionary<string, Pac_boolean>();
            Keys_Up = new Dictionary<string, Pac_boolean>();
            for (int i = 0; i < 26;i++ )
            {
                char c = (char)('A' + i);
                Keys_Down.Add(c.ToString(), new Pac_boolean(false));
                Keys_Up.Add(c.ToString(), new Pac_boolean(false));
            }
            #region Down
            Keys_Down.Add("Left", new Pac_boolean(false));
            Keys_Down.Add("Right", new Pac_boolean(false));
            Keys_Down.Add("Up", new Pac_boolean(false));
            Keys_Down.Add("Down", new Pac_boolean(false));
            Keys_Down.Add("Escape", new Pac_boolean(false));
            Keys_Down.Add("Space", new Pac_boolean(false));
            Keys_Down.Add("Leftshift", new Pac_boolean(false));
            Keys_Down.Add("Rightshift", new Pac_boolean(false));
            Keys_Down.Add("Shift", new Pac_boolean(false));

            #endregion

            #region Up
            Keys_Up.Add("Left", new Pac_boolean(false));
            Keys_Up.Add("Right", new Pac_boolean(false));
            Keys_Up.Add("Up", new Pac_boolean(false));
            Keys_Up.Add("Down", new Pac_boolean(false));
            Keys_Up.Add("Escape", new Pac_boolean(false));
            Keys_Up.Add("Space", new Pac_boolean(false));
            Keys_Up.Add("Leftshift", new Pac_boolean(false));
            Keys_Up.Add("Rightshift", new Pac_boolean(false));
            Keys_Up.Add("Shift", new Pac_boolean(false));

            #endregion
        }
        public static void SetKeyDown(string ID, bool ispress)
        {
            if (Keys_Down.ContainsKey(ID))
                Keys_Down[ID].value = ispress;
        }
        public static void SetKeyUp(string ID, bool ispress)
        {
            if (Keys_Up.ContainsKey(ID))
                Keys_Up[ID].value = ispress;
        }
        public static void InitKeys()
        {
            foreach (Pac_boolean b in Keys_Down.Values)
                b.value = false;
            foreach (Pac_boolean b in Keys_Up.Values)
                b.value = false;
        }

        public static bool getKeyDown(string ID)
        {
            if (Keys_Down.ContainsKey(ID))
            {
                if (Keys_Down[ID].value == true)
                {
                    Keys_Down[ID].value = false;
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        public static bool getKeyUp(string Id)
        {
            if (Keys_Up.ContainsKey(Id))
            {
                if (Keys_Up[Id].value == true)
                {
                    Keys_Up[Id].value = false;
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }


    }
}
