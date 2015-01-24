using UnityEngine;

namespace AgonyBartender
{
	public class Patron : ScriptableObject
	{
		public string PatronName;
        public Sprite[] FaceSprites;
		public Problem[] PatronsProblems;

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
