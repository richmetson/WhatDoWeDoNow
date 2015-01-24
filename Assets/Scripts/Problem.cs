using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AgonyBartender
{
    [System.Serializable]
    public struct ProblemSolution
    {
        public Answer Answer;
        public int Score;
    }

    public class Problem : ScriptableObject
    {
        public string ProblemString;

        public List<ProblemSolution> ProblemSolutions;
    }
}

