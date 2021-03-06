﻿using UnityEngine;

namespace AgonyBartender
{
    public class DifficultyLevel : ScriptableObject
    {
        public AnimationCurve BarLength;
        public AnimationCurve InitialFullness;
        public AnimationCurve MinPatronDifficulty;
        public AnimationCurve MaxPatronDifficulty;
        public AnimationCurve MinTimeForNewPatron;
        public AnimationCurve MaxTimeForNewPatron;
        public AnimationCurve ShiftLength;
        public AnimationCurve PricePerPint;
        public AnimationCurve GenorosityMultiplier;
        public AnimationCurve CommonProblemsSetSize;
        public AnimationCurve NumRedHerrings;

        public AnimationCurve ScoreGrade;
    }
}