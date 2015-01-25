using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AgonyBartender.Inventory
{
    public interface IDragItemSource
    {
        Answer ItemInfo { get; }
        ItemCursor ItemCursor { get; }
    }

    public class InventoryItemSource : MonoBehaviour, IDragHandler, IBeginDragHandler, IDragItemSource
    {
        public Answer ItemInfo;
        public ItemCursor ItemCursorPrefab;

        Answer IDragItemSource.ItemInfo { get { return ItemInfo; }}
        ItemCursor IDragItemSource.ItemCursor { get { return ItemCursor; }}

        public ItemCursor ItemCursor { get; private set; }

        public void OnBeginDrag(PointerEventData eventData)
        {
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