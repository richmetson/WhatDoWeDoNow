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

        public string[] AngryExits = new string[]{
                        "Do you want to take this outside?",
                        "Go hug a landmine",
                        "Whatever, jerk",
                        "Enjoy your deadend job"
                    };

        public string[] SatisfiedExits = new string[]{
            "Cheers, have a good one",
            "That actually sounds like a good idea"
        };

        public string[] ConfusedSayings = new string[]{
            "Err... what?",
            "Are you even listening to me?",
            "I'm not sure that would help, in fact, that doesn't even make gramatical sense"
        };
	}
}
