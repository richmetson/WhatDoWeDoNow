using UnityEngine;
using System.Collections;

namespace AgonyBartender
{

    public class GameSession : MonoBehaviour
    {
        public BarManager BarManager;
        public AnimationCurve BarLength;
        public AnimationCurve InitialFullness;

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
            int barLength = Mathf.RoundToInt(BarLength.Evaluate(ShiftNumber));
            BarManager.SetBarLength(barLength);

            int initialPatrons = (int)(barLength*Mathf.Clamp01(InitialFullness.Evaluate(ShiftNumber)));
            for(int i = 0; i < initialPatrons; ++i)
                BarManager.FillBarStool();

            BarManager.MoveToBarStool(Mathf.FloorToInt(barLength / 2f));



            ++ShiftNumber;
        }
        
    }

}