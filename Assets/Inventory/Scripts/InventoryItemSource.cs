using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AgonyBartender.Inventory
{
    public class InventoryItemSource : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Answer ItemInfo;
        public Image ItemCursorPrefab;

        private Image _itemCursor;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _itemCursor = (Image)Instantiate(ItemCursorPrefab);
            _itemCursor.sprite = ItemInfo.Sprite;
            _itemCursor.transform.SetParent(transform.root);
            _itemCursor.transform.localScale = Vector3.one;
            _itemCursor.GetComponent<RectTransform>().sizeDelta = new Vector2(ItemInfo.Pattern.Width * Inventory.Default.CellSize.x, ItemInfo.Pattern.Height * Inventory.Default.CellSize.y);

            SyncCursorPos(eventData);
        }

        private void SyncCursorPos(PointerEventData eventData)
        {
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
            if(_itemCursor)
                Destroy(_itemCursor.gameObject);
        }

        public void OnDisable()
        {
            if (_itemCursor)
                Destroy(_itemCursor.gameObject);
        }
    }
}