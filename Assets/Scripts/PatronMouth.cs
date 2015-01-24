using UnityEngine;
using System.Collections;

namespace AgonyBartender
{
    public class PatronMouth : MonoBehaviour {

        public Patron Patron;
        Problem ActiveProblem;

        public SpeechBubble ProblemSpeech;

        public RangedFloat TimeBeforeSpeaking;
        public RangedFloat TimeToSpeak;

	    // Use this for initialization
        public IEnumerator Start()
        {
            ProblemSpeech.gameObject.SetActive(false);
            yield return new WaitForSeconds(TimeBeforeSpeaking.PickRandom());

            ProblemSpeech.gameObject.SetActive(true);
            SpeakProblem();

            yield return new WaitForSeconds(TimeToSpeak.PickRandom());

            ProblemSpeech.gameObject.SetActive(false);

        }

        void SpeakProblem()
        {
            if (ProblemSpeech == null)
            {
                print("Could not find speech bubble");
                return;
            }

            if(Patron == null)
            {
                print("No patron selected");
                return;
            }

            ActiveProblem = Patron.SelectProblem();
            ProblemSpeech.gameObject.SetActive(true);
            ProblemSpeech.SetText(ActiveProblem.ProblemString);
        }
	
	    // Update is called once per frame
	    void Update () {
	
	    }

        public void ReceieveResponse(Answer Solution)
        {
            int Index = ActiveProblem.ProblemSolutions.FindIndex(x => x.Answer == Solution);
            
            int Score;
            ProblemSolutionFacialExpression Expression;
            if(Index >= 0)
            {
                Score = ActiveProblem.ProblemSolutions[Index].Score;
                Expression = ActiveProblem.ProblemSolutions[Index].FacialOutcome;
            }
            else
            {
                // Assign a default value since this is essentially a random answer
                Expression = ProblemSolutionFacialExpression.ConfusedResponse;
                Score = 0;
            }
        }
    }

}
