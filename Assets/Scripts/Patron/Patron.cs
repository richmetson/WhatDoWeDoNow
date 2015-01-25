using UnityEngine;

namespace AgonyBartender
{
	public class Patron : ScriptableObject
	{
		public string PatronName;
	    public float DifficultyRating;
        public Sprite[] FaceSprites;
		public Problem[] PatronsProblems;

	    public AudioClip[] Sighs;

	    public AudioClip[] DrinkWarning1;
	    public AudioClip[] DrinkWarning2;
	    public AudioClip[] DrinkWarning3;

        public RangedFloat GapBetweenGulps;
        public RangedFloat GulpMagnitude;

        public float AlcoholIntolerance = 0.5f;

        public int Genorosity;
	    public float SizeMultiplier = 2;
	}
}
