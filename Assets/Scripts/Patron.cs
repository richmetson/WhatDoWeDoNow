using UnityEngine;

namespace AgonyBartender
{
	public class Patron : ScriptableObject
	{
		public string PatronName;
        public Sprite[] FaceSprites;
		public Problem[] PatronsProblems;

        public void OnValidate()
        {
            if(FaceSprites == null || FaceSprites.Length != (int)(ProblemSolutionFacialExpression.FacialExpressionCount))
            {
                Debug.LogError("Need " + (int)(ProblemSolutionFacialExpression.FacialExpressionCount) + " sprites for each facial animation");
            }
        }

        public Problem SelectProblem()
        {
            if (PatronsProblems.Length == 0)
            {
                return null;
            }
            else
            {
                int RandomProblemIndex = Random.Range(0, PatronsProblems.Length - 1);
                return PatronsProblems[RandomProblemIndex];
            }
        }

	}
}
