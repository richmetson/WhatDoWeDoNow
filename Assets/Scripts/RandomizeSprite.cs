using UnityEngine;

namespace AgonyBartender
{
    public class RandomizeSprite : MonoBehaviour
    {
        public UnityEngine.UI.Image Target;

        public Sprite[] Sprites;

        public void OnEnable()
        {
            Target.sprite = Sprites[Random.Range(0, Sprites.Length)];
        }
    }

}