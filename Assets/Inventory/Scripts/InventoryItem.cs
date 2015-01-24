using UnityEngine;
using UnityEngine.UI;

namespace AgonyBartender.Inventory
{
    public class InventoryItem : MonoBehaviour
    {
        public Image Icon;
        public Text Name;
        private Answer _itemInfo;

        public Answer ItemInfo
        {
            get { return _itemInfo; }
            set
            {
                _itemInfo = value;
                Icon.sprite = _itemInfo.Sprite;
                Name.text = _itemInfo.DisplayName;
            }
        }
    }
}