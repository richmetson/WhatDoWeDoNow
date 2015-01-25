using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AgonyBartender.Inventory
{

    public class Inventory : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDragItemSource
    {

        public static Inventory Default { get; private set; }

        public InventoryItem ItemPrefab;

        public Vector2 CellSize;

        private List<InventoryItem> _items;

        public InventoryItemSource ItemSource;

        private Answer _currentDraggingItem;
        Answer IDragItemSource.ItemInfo { get { return _currentDraggingItem; } }

        public InventoryPattern InventoryShape;

        public void Awake()
        {
            _items = new List<InventoryItem>();
            Default = this;
        }

        public void AddItemToInventory(Answer item, int row, int column)
        {
            var newItem = (InventoryItem) Instantiate(ItemPrefab);
            newItem.transform.SetParent(transform);
            newItem.transform.SetAsLastSibling();
            newItem.ItemInfo = item;
            newItem.Row = row;
            newItem.Column = column;

            var rt = newItem.GetComponent<RectTransform>();
            rt.localScale = Vector3.one;
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, column * CellSize.x, item.Pattern.Width * CellSize.x);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, row * CellSize.y, item.Pattern.Height * CellSize.y);

            _items.Add(newItem);
        }

        public void OnDrop(PointerEventData eventData)
        {
            var draggedObj = eventData.pointerDrag;
            if (!draggedObj) return;

            var source = (IDragItemSource)draggedObj.GetComponent(typeof (IDragItemSource));
            if ((source == null) || !source.ItemInfo) return;

            // Figure out where it was dropped...
            var cursor = source.ItemCursor.GetComponent<RectTransform>();
            var localRect = RectTransformUtility.CalculateRelativeRectTransformBounds(transform, cursor);

            int column = Mathf.RoundToInt(localRect.min.x/CellSize.x);
            int row = -Mathf.RoundToInt(localRect.max.y/CellSize.y);

            if (!CanPlaceItemAt(source.ItemInfo.Pattern, row, column))
                return;

            AddItemToInventory(source.ItemInfo, row, column);
        }

        public bool CanPlaceItemAt(InventoryPattern pattern, int row, int column)
        {
            for (int y = 0; y < pattern.Height; ++y)
            {
                for (int x = 0; x < pattern.Width; ++x)
                {
                    if (!InventoryShape[column + x, row + y])
                    {
                        return false;
                    }

                    foreach (var item in _items)
                    {
                        if (item.DoesCoverCell(row + y, column + x)) return false;
                    }
                }
            }

            return true;
        }

        public Answer GetSelectedItem()
        {
            return _currentDraggingItem;
        }

        public Image ItemCursorPrefab;

        private Image _itemCursor;
        Image IDragItemSource.ItemCursor { get { return _itemCursor; } }

        public bool IsDraggingItem { get; set; }

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
            _itemCursor.transform.localScale = Vector3.one;
            _itemCursor.GetComponent<RectTransform>().sizeDelta = new Vector2(item.ItemInfo.Pattern.Width * CellSize.x, item.ItemInfo.Pattern.Height * CellSize.y);

            _currentDraggingItem = item.ItemInfo;

            _items.Remove(item);
            Destroy(item.gameObject);

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

        public Color32 GetHighlightAt(int x, int y)
        {
            return new Color32(255, 255, 255, 255);
        }
    }

}