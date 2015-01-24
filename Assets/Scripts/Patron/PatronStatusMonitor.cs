using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

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

        [Serializable]
        public class PatronLeavingEvent : UnityEvent<PatronStatusMonitor, LeaveReason> { }

        public PatronLeavingEvent OnPatronLeaving;

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

            Liver.OnLiverFailed.AddListener(OnLiverFailed);
        }

        private void OnLiverFailed()
        {
            LeaveBar(LeaveReason.PassedOut);
        }

        // Update is called once per frame
        void Update()
        {
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

        IEnumerator LeaveSequence(LeaveReason reason)
        {
            switch (reason)
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

            OnPatronLeaving.Invoke(this, reason);

            GameObject.Destroy(gameObject);         
        }
    }
}