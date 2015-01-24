using UnityEngine;
using System.Collections;

namespace AgonyBartender
{
    public class PatronMouth : MonoBehaviour {

        public Problem PatronsProblem;
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
            }

            print("Hello, World: " + PatronsProblem.ProblemString);
            ProblemSpeech.gameObject.SetActive(true);
            ProblemSpeech.SetText(PatronsProblem.ProblemString);
        }
	
	    // Update is called once per frame
	    void Update () {
	
	    }

        public void ReceieveResponse(Answer Solution)
        {
            int Index = PatronsProblem.ProblemSolutions.FindIndex(x => x.Answer == Solution);
            
            int Score;
            if(Index >= 0)
            {
                Score = PatronsProblem.ProblemSolutions[Index].Score;
            }
            else
            {
                // Assign a default value since this is essentially a random answer
                Score = 0;
            }
        }
    }

}
