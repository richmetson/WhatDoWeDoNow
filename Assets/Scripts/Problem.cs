using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AgonyBartender
{
    public enum ProblemSolutionFacialExpression
    {
        NeutralResponse = 0, 
        HappyResponse,
        AngryResponse,
        ConfusedResponse
    }

    [System.Serializable]
    public struct ProblemSolution
    {
        public Answer Answer;
        public int Score;
        public ProblemSolutionFacialExpression FacialOutcome;
        public string SpecificResponse;
    }

    public class Problem : ScriptableObject
    {
        public string ProblemString;

        public List<ProblemSolution> ProblemSolutions;
    }
}

