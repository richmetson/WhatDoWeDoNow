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

        protected override void OnFillVBO(System.Collections.Generic.List<UIVertex> vbo)
        {
            RectTransform myRT = GetComponent<RectTransform>();
            var myHeight = myRT.rect.y;

            UIVertex[] vtx = new UIVertex[4];
			vtx[0].uv0 = new Vector2(CellSprite.textureRect.xMin / CellSprite.texture.width, CellSprite.textureRect.yMin / CellSprite.texture.height);
			vtx[1].uv0 = new Vector2(CellSprite.textureRect.xMax / CellSprite.texture.width, CellSprite.textureRect.yMin / CellSprite.texture.height);
			vtx[2].uv0 = new Vector2(CellSprite.textureRect.xMax / CellSprite.texture.width, CellSprite.textureRect.yMax / CellSprite.texture.height);
			vtx[3].uv0 = new Vector2(CellSprite.textureRect.xMin / CellSprite.texture.width, CellSprite.textureRect.yMax / CellSprite.texture.height);

            for (int y = 0; y < _inventory.InventoryShape.Height; ++y)
            {
                for (int x = 0; x < _inventory.InventoryShape.Width; ++x)
                {
                    if (!_inventory.InventoryShape[x, y]) continue;

                    vtx[0].position = new Vector2(x*_inventory.CellSize.x, - y*_inventory.CellSize.y);
                    vtx[1].position = new Vector2((x + 1) * _inventory.CellSize.x, - y * _inventory.CellSize.y);
                    vtx[2].position = new Vector2((x + 1) * _inventory.CellSize.x, - (y + 1) * _inventory.CellSize.y);
                    vtx[3].position = new Vector2(x * _inventory.CellSize.x, - (y + 1) * _inventory.CellSize.y);

                    vtx[0].color = vtx[1].color = vtx[2].color = vtx[3].color = _inventory.GetHighlightAt(x, y);

                    vbo.AddRange(vtx);
                }
            }
        }
    }
}