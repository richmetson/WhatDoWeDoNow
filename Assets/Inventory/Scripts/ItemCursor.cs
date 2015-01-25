using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AgonyBartender.Inventory
{

    public class ItemCursor : MonoBehaviour, IDragHandler, ICanvasRaycastFilter, IEndDragHandler
    {
        public Answer ItemInfo;
        public Image Image;

        public static ItemCursor ActiveCursor;

        public void OnEnable()
        {
            ActiveCursor = this;
        }

        public void OnDisable()
        {
            ActiveCursor = null;
        }

        public void Initialize(Answer itemInfo, Transform parent)
        {
            ItemInfo = itemInfo;

            Image.sprite = ItemInfo.Sprite;

            transform.SetParent(parent, false);
            transform.localScale = Vector3.one;
            ((RectTransform)transform).sizeDelta = new Vector2(ItemInfo.Pattern.Width * Inventory.Default.CellSize.x, ItemInfo.Pattern.Height * Inventory.Default.CellSize.y);
        }

        public void SyncCursorPos(PointerEventData eventData)
        {
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform.root.GetComponent<RectTransform>(),
                eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                transform.position = globalMousePos;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            SyncCursorPos(eventData);
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            return false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Destroy(gameObject);
        }
    }

}