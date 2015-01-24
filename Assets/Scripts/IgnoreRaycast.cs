using UnityEngine;

namespace AgonyBartender
{
    public class IgnoreRaycast : MonoBehaviour, ICanvasRaycastFilter
    {
        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            return false;
        }
    }
}