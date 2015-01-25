using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AgonyBartender.Inventory
{
    public interface IDragItemSource
    {
        Answer ItemInfo { get; }
        Image ItemCursor { get; }
    }

    public class InventoryItemSource : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDragItemSource
    {
        public Answer ItemInfo;
        public Image ItemCursorPrefab;

        Answer IDragItemSource.ItemInfo { get { return ItemInfo; }}
        Image IDragItemSource.ItemCursor { get { return ItemCursor; }}

        public Image ItemCursor { get; private set; }

        public void OnBeginDrag(PointerEventData eventData)
        {
            ItemCursor = (Image)Instantiate(ItemCursorPrefab);
            ItemCursor.sprite = ItemInfo.Sprite;
            ItemCursor.transform.SetParent(transform.root);
            ItemCursor.transform.localScale = Vector3.one;
            ItemCursor.GetComponent<RectTransform>().sizeDelta = new Vector2(ItemInfo.Pattern.Width * Inventory.Default.CellSize.x, ItemInfo.Pattern.Height * Inventory.Default.CellSize.y);

            Inventory.Default.IsDraggingItem = true;

            SyncCursorPos(eventData);
        }

        private void SyncCursorPos(PointerEventData eventData)
        {
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform.root.GetComponent<RectTransform>(),
                eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                ItemCursor.transform.position = globalMousePos;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            SyncCursorPos(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(ItemCursor)
                Destroy(ItemCursor.gameObject);

            Inventory.Default.IsDraggingItem = false;
        }

        public void OnDisable()
        {
            if (ItemCursor)
                Destroy(ItemCursor.gameObject);

            Inventory.Default.IsDraggingItem = false;
        }
    }
}