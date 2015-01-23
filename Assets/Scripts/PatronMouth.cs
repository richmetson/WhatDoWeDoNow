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

        void SetSpeechBox(GUIText TextBox)
        {
            TextBox.text = PatronsProblem.ProblemString;
        }

    }
}
