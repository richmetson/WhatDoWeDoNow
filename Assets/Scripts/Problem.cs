﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AgonyBartender
{
    public enum ProblemSolutionFacialExpression
    {
        NeutralResponse = 0, 
        HappyResponse,
        AngryResponse,
        ConfusedResponse,

        FacialExpressionCount

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

