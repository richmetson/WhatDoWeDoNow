using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AgonyBartender.Inventory
{

    public class Inventory : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        public static Inventory Default { get; private set; }

        public InventoryItem ItemPrefab;

        public Vector2 CellSize;

        private List<InventoryItem> _items;

        private InventoryItem CurrentDraggingItem;

        public InventoryPattern InventoryShape;

        public void Awake()
        {
            _items = new List<InventoryItem>();
            Default = this;

            GetComponent<RectTransform>().sizeDelta = new Vector2(CellSize.x*InventoryShape.Width,
                CellSize.y*InventoryShape.Height);
        }

        public void AddItemToInventory(Answer item)
        {
            var newItem = (InventoryItem) Instantiate(ItemPrefab);
            newItem.transform.SetParent(transform);
            newItem.transform.SetAsLastSibling();
            newItem.ItemInfo = item;
            GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
            _items.Add(newItem);
        }

        public void OnDrop(PointerEventData eventData)
        {
            var draggedObj = eventData.pointerDrag;
            if (!draggedObj) return;

            var source = draggedObj.GetComponent<InventoryItemSource>();
            if (!source || !source.ItemInfo) return;

            AddItemToInventory(source.ItemInfo);
        }

        public Answer GetSelectedItem()
        {
            if (CurrentDraggingItem == null)
            {
                return null;
            }
            else
            {
                return CurrentDraggingItem.ItemInfo;
            }
        }

        public Image ItemCursorPrefab;

        private Image _itemCursor;

        public InventoryItem GetItemUnderCursor(PointerEventData eventData)
        {
            foreach (var item in _items)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(item.GetComponent<RectTransform>(),
                    eventData.position, eventData.enterEventCamera))
                    return item;
            }
            return null;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Figure out which item we're dragging
            var item = GetItemUnderCursor(eventData);
            if (!item) return;

            _itemCursor = (Image)Instantiate(ItemCursorPrefab);
            _itemCursor.sprite = item.ItemInfo.Sprite;
            _itemCursor.transform.SetParent(transform.root);

            CurrentDraggingItem = item;

            SyncCursorPos(eventData);
        }

        private void SyncCursorPos(PointerEventData eventData)
        {
            if (!_itemCursor) return;

            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform.root.GetComponent<RectTransform>(),
                eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                _itemCursor.transform.position = globalMousePos;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            SyncCursorPos(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_itemCursor)
                Destroy(_itemCursor.gameObject);
        }

        public void OnDisable()
        {
            if (_itemCursor)
                Destroy(_itemCursor.gameObject);
        }
    }

}