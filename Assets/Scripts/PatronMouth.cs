﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

namespace AgonyBartender
{
    [RequireComponent(typeof(PatronDefinition))]
    public class PatronMouth : MonoBehaviour, IDropHandler
    {

        PatronDefinition PatronDefinition;

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

            PatronDefinition = gameObject.GetComponent<PatronDefinition>();

            Problem ActiveProblem = PatronDefinition.GetActiveProblem();
            ProblemSpeech.gameObject.SetActive(true);
            ProblemSpeech.SetText(ActiveProblem.ProblemString);
        }
	
	    // Update is called once per frame
	    void Update () {
	
	    }

        public void ReceieveResponse(Answer Solution)
        {
            Problem ActiveProblem = PatronDefinition.GetActiveProblem();

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

            GetComponent<PatronFaceController>().OnFaceChanged(Expression);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag)
            {
                // This doesn't feel like a good way of doing this, bht the GO being dragged seems to get destroyed
                // before we can read out a component from it
                Inventory.Inventory Inventory = eventData.pointerDrag.GetComponent<Inventory.Inventory>();
                if (Inventory != null)
                {
                    Answer SelectedAnswer = Inventory.GetSelectedItem();
                    ReceieveResponse(SelectedAnswer);
                }
            }
        }
    }

}
