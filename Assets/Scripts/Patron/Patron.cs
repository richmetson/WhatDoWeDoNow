using UnityEngine;

namespace AgonyBartender
{
	public class Patron : ScriptableObject
	{
		public string PatronName;
	    public float DifficultyRating;
        public Sprite[] FaceSprites;
		public Problem[] PatronsProblems;

        public RangedFloat GapBetweenGulps;
        public RangedFloat GulpMagnitude;

        public float AlcoholIntolerance = 0.5f;

        public Problem SelectProblem()
        {
            if (PatronsProblems.Length == 0)
            {
                return null;
            }
            else
            {
                int RandomProblemIndex = Random.Range(0, PatronsProblems.Length);
                return PatronsProblems[RandomProblemIndex];
            }
        }

	}
}
