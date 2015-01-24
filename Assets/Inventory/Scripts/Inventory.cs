using System.Collections.Generic;
using AgonyBartender;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IDropHandler
{

    public InventoryItem ItemPrefab;

    private List<InventoryItem> _items;

    public void Awake()
    {
        _items = new List<InventoryItem>();
    }

    public void AddItemToInventory(Answer item)
    {
        var newItem = (InventoryItem)Instantiate(ItemPrefab);
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
}
