using UnityEngine;

namespace AgonyBartender
{
    public class RandomizeSprite : MonoBehaviour
    {
        public UnityEngine.UI.Image Target;

        public Sprite[] Sprites;
        public bool SetNativeSize;
        public float ScaleFactor = 1f;

        public void OnEnable()
        {
            Target.sprite = Sprites.Random();

            if (SetNativeSize)
            {
                Target.GetComponent<RectTransform>().sizeDelta = new Vector2(Target.sprite.rect.width,
                    Target.sprite.rect.height) * ScaleFactor;
            }
        }
    }

}