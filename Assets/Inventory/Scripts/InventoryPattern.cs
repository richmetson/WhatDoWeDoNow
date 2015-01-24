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
    }

}