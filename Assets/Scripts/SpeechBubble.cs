using UnityEngine;
using UnityEngine.UI;

namespace AgonyBartender
{
    public class SpeechBubble : MonoBehaviour
    {
        [SerializeField] private Text Text;

        public void SetText(string text)
        {
            Text.text = text;
        }
    }
}