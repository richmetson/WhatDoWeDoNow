using AgonyBartender.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

namespace AgonyBartender
{
    [RequireComponent(typeof(PatronDefinition))]
    public class PatronMouth : MonoBehaviour, IDropHandler
    {

        PatronDefinition PatronDefinition;

        public SpeechBubble ProblemSpeech;

        public RangedFloat TimeBeforeSpeaking;
        public RangedFloat TimeToSpeak;

        int BestAnswer;
        int WorstAnswer;

	    // Use this for initialization
        public IEnumerator Start()
        {
            ProblemSpeech.gameObject.SetActive(false);

            PatronDefinition = gameObject.GetComponent<PatronDefinition>();

            yield return new WaitForSeconds(TimeBeforeSpeaking.PickRandom());

            ProblemSpeech.gameObject.SetActive(true);
            SpeakProblem();
        }

        public void Say(string Text)
        {
            if (ProblemSpeech == null)
            {
                Debug.LogError("Could not find speech bubble");
                return;
            }

            ProblemSpeech.gameObject.SetActive(true);
            ProblemSpeech.SetText(Text);
        }

        void SpeakProblem()
        {
            Problem ActiveProblem = PatronDefinition.ActiveProblem;
            Say(ActiveProblem.ProblemString);
        }

        void OnDestroy()
        {
            ProblemSpeech.gameObject.SetActive(false);
        }

        public void ReceieveResponse(Answer Solution)
        {

            if (Solution)
            {
                Problem ActiveProblem = PatronDefinition.ActiveProblem;

                int Index = ActiveProblem.ProblemSolutions.FindIndex(x => x.Answer == Solution);

                int Score;
                ProblemSolutionFacialExpression Expression;
                string SpecialReply = string.Empty;
                if (Index >= 0)
                {
                    Score = ActiveProblem.ProblemSolutions[Index].Score;
                    Expression = ActiveProblem.ProblemSolutions[Index].FacialOutcome;
                    SpecialReply = ActiveProblem.ProblemSolutions[Index].SpecificResponse;
                }
                else
                {
                    // Assign a default value since this is essentially a random answer
                    Expression = ProblemSolutionFacialExpression.ConfusedResponse;
                    Score = 0;
                }

                GetComponent<PatronFaceController>().OnFaceChanged(Expression);

                ProblemSpeech.gameObject.SetActive(false);
                if (Score > 0)
                {
                    BestAnswer = Mathf.Max(BestAnswer, Score);
                }
                else
                {
                    WorstAnswer = Score;
                }

                if (!string.IsNullOrEmpty(SpecialReply))
                {
                    Say(SpecialReply);
                }
                else if(Score > 1)
                {
                    Say(GetComponent<PatronDefinition>().Patron.SatisfiedExits.Random());
                }
                else if (Score < 0)
                {
                    Say(GetComponent<PatronDefinition>().Patron.AngryExits.Random());
                }

                ConsiderLeaving();
            }

            StartCoroutine(RemindProblem());
        }

        IEnumerator RemindProblem()
        {
            yield return new WaitForSeconds(5.0f);
            Say(PatronDefinition.ActiveProblem.ProblemString);
        }

        private void ConsiderLeaving()
        {
            if(BestAnswer >= 2)
            {
                GetComponent<PatronStatusMonitor>().LeaveBar(PatronStatusMonitor.LeaveReason.Satisfied);
            }
            else if(WorstAnswer < 0)
            {
                BestAnswer = 0;
                GetComponent<PatronStatusMonitor>().LeaveBar(PatronStatusMonitor.LeaveReason.Angry);
            }
        }

        public int ComputeTip()
        {
            return BestAnswer;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag)
            {
                var source = eventData.pointerDrag.GetComponent<ItemCursor>();
                if (source != null)
                {
                    ReceieveResponse(source.ItemInfo);
                }
            }
        }

        public AudioClip[] ArriveSounds;

        public void PlayArriveSound()
        {
            var source = GetComponent<AudioSource>();
            source.clip = ArriveSounds.Random();
            source.Play();
        }
    }

}
