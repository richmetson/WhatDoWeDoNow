using AgonyBartender.Inventory;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InventoryGroup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Animator Animator;

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsPointerInside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsPointerInside = false;
    }

    public bool IsPointerInside { get; private set; }

    private bool _isShowing;

    public void Update()
    {
        var currentAnimation = Animator.GetCurrentAnimatorStateInfo(0);

        bool shouldShow = IsPointerInside || Inventory.Default.IsDraggingItem;
        if (shouldShow != _isShowing)
        {
            float offset = 1f - Mathf.Clamp01(currentAnimation.normalizedTime);

            Animator.Play(shouldShow ? "ShowInventory" : "HideInventory", 0, offset);
            _isShowing = shouldShow;
        }
    }
}
