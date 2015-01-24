using UnityEngine;
using System.Collections;

namespace AgonyBartender
{
    public class PatronMouth : MonoBehaviour {

        public Problem PatronsProblem; 

	    // Use this for initialization
	    void Start () {
            SpeechBubble ProblemSpeech = gameObject.AddComponent<SpeechBubble>();
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
