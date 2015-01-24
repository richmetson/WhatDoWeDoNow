using AgonyBartender;
using UnityEngine;
using System.Collections;

public class InventoryItem : MonoBehaviour
{
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

    public UnityEngine.UI.Image Icon;
    public UnityEngine.UI.Text Name;
}
