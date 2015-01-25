﻿using System.Linq;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

namespace AgonyBartender
{

    public class GameSession : MonoBehaviour
    {
        public BarManager BarManager;
        public DifficultyLevel Difficulty;
        public Clock Clock;

        public Patron[] PatronArchetypes;

        public void Start()
        {
            BeginNewShift();
        }

        public int ShiftNumber { get; private set; }

        public float BeerDispensedThisShift { get; private set; }
        public float TotalBeerDispensed { get; private set; }

        public int PatronsServedThisShift { get; private set; }
        public int TotalPatronsServed { get; private set; }

        public int PatronsPoisonedThisShift { get; private set; }
        public int TotalPatronsPoisoned { get; private set; }

        public int TipsMade { get; private set; }
        public int TotalTipsMade { get; private set; }

        public void OnBeerDispensed(float amount)
        {
            BeerDispensedThisShift += amount;
            TotalBeerDispensed += amount;
        }

        public void OnPatronArrived()
        {
            PatronsServedThisShift++;
            TotalPatronsServed++;
        }

        public void OnPatronLiverFailed()
        {
            PatronsPoisonedThisShift++;
            TotalPatronsPoisoned++;
        }

        public void OnPatronLeftTip(int amount)
        {
            TipsMade += amount;
            TotalTipsMade += amount;
        }

        public ScoreDisplayController ScorePage;
        public CanvasGroup GameGroup;

        public void OnShiftEnded()
        {
            CancelInvoke();
            Clock.enabled = false;
            BarManager.DeletePatrons();

            GameGroup.interactable = false;

            ++ShiftNumber;
            ScorePage.gameObject.SetActive(true);
            ScorePage.DisplayResults(this);
        }

        public void BeginNewShift()
        {
            BeerDispensedThisShift = 0;
            PatronsServedThisShift = 0;
            PatronsPoisonedThisShift = 0;
            TipsMade = 0;

            BarManager.MoveToBarStool(null);
            BarManager.DeletePatrons();
            int barLength = Mathf.RoundToInt(Difficulty.BarLength.Evaluate(ShiftNumber));
            BarManager.SetBarLength(barLength);

            int initialPatrons = (int)(barLength * Mathf.Clamp01(Difficulty.InitialFullness.Evaluate(ShiftNumber)));
            // Ensure that we have at least one patron waiting at the beginning of the game, because it's boring to start with an empty bar
            if (ShiftNumber == 0) initialPatrons = Mathf.Max(initialPatrons, 1);

            BarManager.EnableArriveSounds = false;
            for (int i = 0; i < initialPatrons; ++i)
                SpawnPatron();
            BarManager.EnableArriveSounds = true;

            BarManager.MoveToBarStool(Mathf.FloorToInt(barLength / 2f));

            ScheduleNewPatron();

            Clock.GameDuration = Difficulty.ShiftLength.Evaluate(ShiftNumber);
            Clock.ResetClock();
            Clock.enabled = true;

            GameGroup.interactable = true;
        }

        public float FadeTime = 0.5f;

        public void DoFadeAndBeginNextWave()
        {
            StartCoroutine(FadeOutAndBeginNextWave());
        }

        public IEnumerator FadeOutAndBeginNextWave()
        {
            float startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < startTime + FadeTime)
            {
                GameGroup.alpha = 1f - Mathf.Clamp01((Time.realtimeSinceStartup - startTime)/FadeTime);
                yield return null;
            }
            GameGroup.alpha = 0;

            ++ShiftNumber;
            BeginNewShift();

            startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < startTime + FadeTime)
            {
                GameGroup.alpha = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / FadeTime);
                yield return null;
            }
            GameGroup.alpha = 1f;
        }

        private void ScheduleNewPatron()
        {
            float minTimeToNewPatron = Difficulty.MinTimeForNewPatron.Evaluate(ShiftNumber);
            float maxTimeToNewPatron = Difficulty.MaxTimeForNewPatron.Evaluate(ShiftNumber);
            Invoke("SpawnScheduledPatron", Random.Range(minTimeToNewPatron, maxTimeToNewPatron));
        }

        private void SpawnScheduledPatron()
        {
            SpawnPatron();
            ScheduleNewPatron();
        }

        public StandardProblemList StandardProblems;

        public void SpawnPatron()
        {
            var minDifficulty = Difficulty.MinPatronDifficulty.Evaluate(ShiftNumber);
            var maxDifficulty = Difficulty.MaxPatronDifficulty.Evaluate(ShiftNumber);

            var patron = PatronArchetypes
                        .Where(p => p.DifficultyRating >= minDifficulty && p.DifficultyRating <= maxDifficulty)
                        .Random();

            var problem = StandardProblems.GlobalProblems.Concat(patron.PatronsProblems).Random();

            BarManager.FillBarStool(patron, problem);
        }
        
    }

}