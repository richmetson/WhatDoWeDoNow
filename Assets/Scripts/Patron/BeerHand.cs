using UnityEngine;
using System.Collections;
namespace AgonyBartender
{
    [RequireComponent(typeof(PatronDefinition))]
    public class BeerHand : MonoBehaviour
    {
        public Drink Beer;

        public RangedFloat GapBetweenGulps;
        public RangedFloat LengthOfGulp;

        bool IsDrinking;

        // Use this for initialization
        IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(GapBetweenGulps.PickRandom());
                IsDrinking = true;
                yield return new WaitForSeconds(LengthOfGulp.PickRandom());
                IsDrinking = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(IsDrinking)
            {
                if(!Beer)
                {
                    Debug.LogError("WHERE IS MY BEER!!!");
                    return;
                }

                Beer.Level = Beer.Level - 0.1f * Time.deltaTime;
            }
        }
    }
}
