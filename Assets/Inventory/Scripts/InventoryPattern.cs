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
            get
            {
                if (x < 0 || x >= Width) return false;
                if (y < 0 || y >= Height) return false;
                return Pattern[y*Width + x];
            }
            set { Pattern[y*Width + x] = value; }
        }
    }

}