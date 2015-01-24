using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AgonyBartender.Inventory
{
    public class InventoryItem : MonoBehaviour
    {
        public int Row;
        public int Column;
        public Image Icon;
        private Answer _itemInfo;

        public Answer ItemInfo
        {
            get { return _itemInfo; }
            set
            {
                _itemInfo = value;
                Icon.sprite = _itemInfo.Sprite;
            }
        }

        public bool DoesCoverCell(int row, int column)
        {
            return _itemInfo.Pattern[(column - Column), (row - Row)];
        }
    }
}