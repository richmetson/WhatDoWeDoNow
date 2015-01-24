using System.Collections;
using UnityEngine;

namespace AgonyBartender
{
    public class FruitMachine : MonoBehaviour
    {
        public UnityEngine.UI.Image Image;
        public Sprite[] Sprites;
        public float FPS = 20f;

        public IEnumerator Start()
        {
            int index = 0;
            while (true)
            {
                Image.sprite = Sprites[index++ % Sprites.Length];
                yield return new WaitForSeconds(1f/FPS);
            }
        }

    }

}