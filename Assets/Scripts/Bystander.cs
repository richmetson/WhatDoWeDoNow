using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AgonyBartender
{

    public class Bystander : MonoBehaviour
    {
        public SpeechBubble SpeechBubble;
        public InventoryItemSource ItemSource;
        public OverheardConversation[] ConversationSnippets;

        public RangedFloat TimeBeforeSpeaking;
        public RangedFloat TimeToSpeak;
        
        public IEnumerator Start()
        {
            SpeechBubble.gameObject.SetActive(false);

            while (true)
            {
                yield return new WaitForSeconds(TimeBeforeSpeaking.PickRandom());

                var snippet = ConversationSnippets[Random.Range(0, ConversationSnippets.Length)];

                SpeechBubble.gameObject.SetActive(true);
                SpeechBubble.SetText(snippet.Text);
                ItemSource.ItemInfo = snippet.AnswerDelivered;

                yield return new WaitForSeconds(TimeToSpeak.PickRandom());

                SpeechBubble.gameObject.SetActive(false);
                ItemSource.ItemInfo = null;
            }
        }
    }

}