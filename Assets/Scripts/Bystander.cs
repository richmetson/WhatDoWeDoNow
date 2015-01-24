using System.Collections;
using AgonyBartender.Inventory;
using UnityEngine;

namespace AgonyBartender
{
    public class Bystander : MonoBehaviour
    {
        public OverheardConversation[] ConversationSnippets;
        public InventoryItemSource ItemSource;
        public SpeechBubble SpeechBubble;

        public RangedFloat TimeBeforeSpeaking;
        public RangedFloat TimeToSpeak;

        public IEnumerator Start()
        {
            SpeechBubble.gameObject.SetActive(false);

            while (true)
            {
                yield return new WaitForSeconds(TimeBeforeSpeaking.PickRandom());

                OverheardConversation snippet = ConversationSnippets.Random();

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