using UnityEngine;
using UnityEngine.EventSystems;

namespace AgonyBartender.Inventory
{
    public class InventoryItemSource : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        public Answer ItemInfo;

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
        }
    }
}