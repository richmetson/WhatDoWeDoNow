using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace AgonyBartender.Inventory
{
    public class InventoryGrid : Graphic
    {
        public Sprite CellSprite;

        public override Texture mainTexture
        {
            get { return CellSprite.texture; }
        }

        protected override void Reset()
        {
            _inventory = GetComponent<Inventory>();
            base.Reset();
        }

        [SerializeField] private Inventory _inventory;

        public Color BaseGridColor;

        protected override void OnFillVBO(System.Collections.Generic.List<UIVertex> vbo)
        {
            RectTransform myRT = GetComponent<RectTransform>();

            Color32 white = BaseGridColor;
            Color32 highlight = white;
            if (_pendingObjectInfo)
            {
                highlight = _inventory.CanPlaceItemAt(_pendingObjectInfo.Pattern, _pendingY, _pendingX)
                    ? new Color32(0x00, 0xff, 0x00, 0xff)
                    : new Color32(0xff, 0x00, 0x00, 0xff);
            }

            UIVertex[] vtx = new UIVertex[4];
			vtx[0].uv0 = new Vector2(CellSprite.textureRect.xMin / CellSprite.texture.width, CellSprite.textureRect.yMin / CellSprite.texture.height);
			vtx[1].uv0 = new Vector2(CellSprite.textureRect.xMax / CellSprite.texture.width, CellSprite.textureRect.yMin / CellSprite.texture.height);
			vtx[2].uv0 = new Vector2(CellSprite.textureRect.xMax / CellSprite.texture.width, CellSprite.textureRect.yMax / CellSprite.texture.height);
			vtx[3].uv0 = new Vector2(CellSprite.textureRect.xMin / CellSprite.texture.width, CellSprite.textureRect.yMax / CellSprite.texture.height);
            vtx[0].color = vtx[1].color = vtx[2].color = vtx[3].color = white;

            for (int y = 0; y < _inventory.InventoryShape.Height; ++y)
            {
                for (int x = 0; x < _inventory.InventoryShape.Width; ++x)
                {
                    if (!_inventory.InventoryShape[x, y]) continue;

                    vtx[0].position = new Vector2(x*_inventory.CellSize.x, - y*_inventory.CellSize.y);
                    vtx[1].position = new Vector2((x + 1) * _inventory.CellSize.x, - y * _inventory.CellSize.y);
                    vtx[2].position = new Vector2((x + 1) * _inventory.CellSize.x, - (y + 1) * _inventory.CellSize.y);
                    vtx[3].position = new Vector2(x * _inventory.CellSize.x, - (y + 1) * _inventory.CellSize.y);

                    var applyHighlightHere = _pendingObjectInfo && _pendingObjectInfo.Pattern[x - _pendingX, y - _pendingY];
                    vtx[0].color = vtx[1].color = vtx[2].color = vtx[3].color = applyHighlightHere ? highlight : white;

                    vbo.AddRange(vtx);
                }
            }
        }

        private Answer _pendingObjectInfo;
        private int _pendingX;
        private int _pendingY;

        public void ClearPendingObject()
        {
            if (!_pendingObjectInfo) return;
            _pendingObjectInfo = null;
            SetVerticesDirty();
        }

        public void SetPendingObject(Answer itemInfo, int x, int y)
        {
            if (_pendingObjectInfo == itemInfo && _pendingX == x && _pendingY == y) return;

            _pendingObjectInfo = itemInfo;
            _pendingX = x;
            _pendingY = y;

            SetVerticesDirty();
        }
    }
}