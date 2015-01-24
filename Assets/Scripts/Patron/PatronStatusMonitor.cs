using UnityEngine;
using System.Collections;

namespace AgonyBartender
{
    [RequireComponent(typeof(BeerHand))]
    [RequireComponent(typeof(Liver))]
    [RequireComponent(typeof(PatronDefinition))]
    [RequireComponent(typeof(PatronMouth))]
    public class PatronStatusMonitor : MonoBehaviour
    {
        public enum LeaveReason
        {
            PassedOut,
            Bored,
            Satisfied
        }

        public delegate void PatronLeaves();

        public event PatronLeaves OnPatronLeaves;

        BeerHand BeerHand;
        Liver Liver;

        bool HasStartedLeavingBar;

        Coroutine EmptyBeerCountdown;

        // Use this for initialization
        void Start()
        {
            BeerHand = gameObject.GetComponent<BeerHand>();
            Liver = gameObject.GetComponent<Liver>();
            EmptyBeerCountdown = null;
            HasStartedLeavingBar = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(Liver.GetCurrentABV() >= 1.0f)
            {
                LeaveBar(LeaveReason.PassedOut);
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

            LeaveBar(LeaveReason.Bored);
            yield break;
        }

        public void LeaveBar(LeaveReason Reason)
        {
            if (!HasStartedLeavingBar)
            {
                print("I'm off!");
                HasStartedLeavingBar = true;
                StartCoroutine(LeaveSequence(Reason));
            }
        }

        IEnumerator LeaveSequence(LeaveReason Reason)
        {
            switch (Reason)
            {
                case LeaveReason.PassedOut:
                    gameObject.GetComponent<PatronMouth>().Say("Zzzz...");
                    break;
                case LeaveReason.Bored:
                    gameObject.GetComponent<PatronMouth>().Say("Err, thanks for nothing");
                    break;
                case LeaveReason.Satisfied:
                    gameObject.GetComponent<PatronMouth>().Say("Cheers, have a good one");
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(3.0f);

            if(OnPatronLeaves != null)
            {
                OnPatronLeaves();
            }

            GameObject.Destroy(gameObject);         
        }
    }
}