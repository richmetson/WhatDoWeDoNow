using UnityEngine;
using System.Collections;

namespace AgonyBartender
{
    [RequireComponent(typeof(BeerHand))]
    [RequireComponent(typeof(Liver))]
    [RequireComponent(typeof(PatronDefinition))]
    public class PatronStatusMonitor : MonoBehaviour
    {
        BeerHand BeerHand;
        Liver Liver;

        Coroutine EmptyBeerCountdown;

        // Use this for initialization
        void Start()
        {
            BeerHand = gameObject.GetComponent<BeerHand>();
            Liver = gameObject.GetComponent<Liver>();
            EmptyBeerCountdown = null;
        }

        // Update is called once per frame
        void Update()
        {
            if(Liver.GetCurrentABV() >= 1.0f)
            {
                LeaveBar();
            }

            if(EmptyBeerCountdown == null && BeerHand.Beer.IsEmpty)
            {
                print("Thinking of leaving");
                EmptyBeerCountdown = StartCoroutine(EmptyBeerMonitor());
            }
        }

        IEnumerator EmptyBeerMonitor()
        {
            float TimeEmpty = 0.0f;

            while(TimeEmpty <= 5.0f)
            {
                if(BeerHand.Beer.IsEmpty)
                {
                    TimeEmpty += Time.deltaTime;
                    yield return null;
                }
                else
                {
                    // The beer has been topped up, so we stop considering leaving
                    EmptyBeerCountdown = null;
                    yield break;
                }
            }

            LeaveBar();
            yield break;
        }

        public void LeaveBar()
        {
            print("I'm off!");
            // todo
        }
    }
}