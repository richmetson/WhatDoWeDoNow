using AgonyBartender.Inventory;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InventoryGroup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ICanvasRaycastFilter
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

        bool shouldShow = IsPointerInside || Inventory.Default.IsDraggingItem && transform.parent.GetComponent<CanvasGroup>().interactable;
        if (shouldShow != _isShowing)
        {
            float offset = 1f - Mathf.Clamp01(currentAnimation.normalizedTime);

            Animator.Play(shouldShow ? "ShowInventory" : "HideInventory", 0, offset);
            _isShowing = shouldShow;
        }
    }

    public Vector2 RaycastCenterPoint;
    public Vector2 RaycastCenterScale;

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        var rt = GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, sp, eventCamera, out localPoint);

        Vector2 localDelta = localPoint - RaycastCenterPoint;
        localDelta.x /= RaycastCenterScale.x;
        localDelta.y /= RaycastCenterScale.y;

        return localDelta.sqrMagnitude < 1f;
    }

    public void OnDrawGizmos()
    {
        var mat = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.TransformPoint(RaycastCenterPoint), Quaternion.identity, new Vector3(RaycastCenterScale.x, RaycastCenterScale.y, 1f)) * mat;
        Gizmos.DrawSphere(Vector3.zero, 1f);
        Gizmos.matrix = mat;
    }
}
