using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AgonyBartender.Inventory
{

    public class Inventory : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler
    {

        public static Inventory Default { get; private set; }

        public InventoryItem ItemPrefab;

        public Vector2 CellSize;

        private List<InventoryItem> _items;

        public InventoryItemSource ItemSource;

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

            var source = draggedObj.GetComponent<ItemCursor>();
            if (!source) return;

            // Figure out where it was dropped...
            var cursor = source.GetComponent<RectTransform>();
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

        public ItemCursor ItemCursorPrefab;

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

            var itemCursor = (ItemCursor)Instantiate(ItemCursorPrefab);
            itemCursor.Initialize(item.ItemInfo, transform.root);
            itemCursor.SyncCursorPos(eventData);
            eventData.pointerDrag = itemCursor.gameObject;

            _items.Remove(item);
            Destroy(item.gameObject);
        }

        public void OnDrag(PointerEventData eventData)
        {

        }

        public void Clear()
        {
            foreach (var item in _items)
                Destroy(item.gameObject);

            _items.Clear();
        }

        public InventoryGrid Grid;

        public Answer PendingObject { get; private set; }
        public int PendingColumn { get; private set; }
        public int PendingRow { get; private set; }

        public void Update()
        {
            if (!ItemCursor.ActiveCursor)
            {
                Grid.ClearPendingObject();
                return;
            }

            var cursor = ItemCursor.ActiveCursor.GetComponent<RectTransform>();
            var localRect = RectTransformUtility.CalculateRelativeRectTransformBounds(transform, cursor);

            int column = Mathf.RoundToInt(localRect.min.x / CellSize.x);
            int row = -Mathf.RoundToInt(localRect.max.y / CellSize.y);

            Grid.SetPendingObject(ItemCursor.ActiveCursor.ItemInfo, column, row);
        }
    }

}