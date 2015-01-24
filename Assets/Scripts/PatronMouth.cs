using UnityEngine;
using System.Collections;

namespace AgonyBartender
{
    public class PatronMouth : MonoBehaviour {

        public Problem PatronsProblem; 

	    // Use this for initialization
	    void Start () {
	
	    }
	
	    // Update is called once per frame
	    void Update () {
	
	    }

        public void SetSpeechBox(GUIText TextBox)
        {
            TextBox.text = PatronsProblem.ProblemString;
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
