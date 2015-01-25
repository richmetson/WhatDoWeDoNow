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
            Satisfied,
            Angry,
            PubClosing,
        }

        [Serializable]
        public class PatronLeavingEvent : UnityEvent<PatronStatusMonitor, LeaveReason> { }

        public PatronLeavingEvent OnPatronLeaving;

        [System.Serializable]
        public class PatronTippingEvent : UnityEvent<int> { }

        public PatronTippingEvent OnPatronTipping;

        BeerHand BeerHand;
        Liver Liver;

        bool HasStartedLeavingBar;

        Coroutine EmptyBeerCountdown;

        public string[] AngryExits = new string[]{
                        "Do you want to take this outside?",
                        "Go hug a landmine",
                        "Whatever, jerk",
                        "Enjoy your deadend job"
                    };

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

                case LeaveReason.Angry:
                    
                    gameObject.GetComponent<PatronMouth>().Say(AngryExits.Random());
                    break;
                default:
                    break;
            }
            if (reason != LeaveReason.PubClosing)
            {
                yield return new WaitForSeconds(3.0f);
            }

            OnPatronLeaving.Invoke(this, reason);
            int Tip = GetComponent<PatronMouth>().ComputeTip();
            int Multiplier = GetComponent<PatronDefinition>().Patron.Genorosity;
            float BeerDrunk = GetComponent<BeerHand>().GetAmountBeerDrunkInPints();
            float BeerMultiplier = Mathf.Max(1.0f, BeerDrunk); // never reduce the tip size
            OnPatronTipping.Invoke(Mathf.RoundToInt(BeerMultiplier * Tip * Multiplier) * 100);

            GameObject.Destroy(gameObject);         
        }
    }
}