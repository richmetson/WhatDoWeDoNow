using System.Linq;
using UnityEngine;
using System.Collections;

namespace AgonyBartender
{

    public class GameSession : MonoBehaviour
    {
        public BarManager BarManager;
        public DifficultyLevel Difficulty;

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
            for (int i = 0; i < initialPatrons; ++i)
                SpawnPatron();

            BarManager.MoveToBarStool(Mathf.FloorToInt(barLength / 2f));

            ScheduleNewPatron();
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

        public void SpawnPatron()
        {
            var minDifficulty = Difficulty.MinPatronDifficulty.Evaluate(ShiftNumber);
            var maxDifficulty = Difficulty.MaxPatronDifficulty.Evaluate(ShiftNumber);

            var patrons =
                PatronArchetypes.Where(p => p.DifficultyRating >= minDifficulty && p.DifficultyRating <= maxDifficulty)
                    .ToArray();
            var patron = patrons[Random.Range(0, patrons.Length)];

            BarManager.FillBarStool(patron);
        }
        
    }

}