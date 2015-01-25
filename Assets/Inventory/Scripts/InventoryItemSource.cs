using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AgonyBartender.Inventory
{
    public class InventoryItemSource : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        public Answer ItemInfo;
        public ItemCursor ItemCursorPrefab;

        public ItemCursor ItemCursor { get; private set; }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!ItemInfo) return;

            ItemCursor = (ItemCursor)Instantiate(ItemCursorPrefab);
            ItemCursor.Initialize(ItemInfo, transform.root);
            ItemCursor.SyncCursorPos(eventData);

            eventData.pointerDrag = ItemCursor.gameObject;
        }

        // Doesn't do anything, but necessary in order to receive OnBeginDrag
        public void OnDrag(PointerEventData eventData)
        {
            
        }
    }
}