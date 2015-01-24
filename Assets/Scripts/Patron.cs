﻿using UnityEngine;

namespace AgonyBartender
{
	public class Patron : ScriptableObject
	{
		public string PatronName;
		public Sprite Sprite;
		public Problem[] PatronsProblems;

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
