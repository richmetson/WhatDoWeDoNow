using System;
using UnityEngine;
using System.Collections;

namespace AgonyBartender.Inventory
{

    [Serializable]
    public class InventoryPattern
    {
        public int Width;
        public int Height;
        public bool[] Pattern;

        public bool this[int x, int y]
        {
            get { return Pattern[y*Width + x]; }
            set { Pattern[y*Width + x] = value; }
        }
    }

}