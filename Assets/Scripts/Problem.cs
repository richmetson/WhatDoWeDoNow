using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AgonyBartender
{
    public enum ProblemSolutionFacialExpression
    {
        HappyResponse,
        NeutralResponse, 
        AngryResponse,
        ConfusedResponse
    }

    [System.Serializable]
    public struct ProblemSolution
    {
        public Answer Answer;
        public int Score;
        public ProblemSolutionFacialExpression FacialOutcome;
    }

    public class Problem : ScriptableObject
    {
        public string ProblemString;

        public List<ProblemSolution> ProblemSolutions;
    }
}

