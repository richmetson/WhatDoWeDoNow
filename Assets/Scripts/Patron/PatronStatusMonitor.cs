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
                EmptyBeerCountdown = StartCoroutine(EmptyBeerMonitor());
            }

            else if (EmptyBeerCountdown != null && !BeerHand.Beer.IsEmpty)
            {
                StopCoroutine(EmptyBeerCountdown);
                EmptyBeerCountdown = null;
            }
        }

        IEnumerator EmptyBeerMonitor()
        {
            var defn = GetComponent<PatronDefinition>();
            yield return new WaitForSeconds(5.0f);

            if (defn.Patron.DrinkWarning1.Length > 0)
            {
                audio.clip = defn.Patron.DrinkWarning1.Random();
                audio.Play();
                yield return new WaitForSeconds(audio.clip.length);
            }

            yield return new WaitForSeconds(4.0f);

            if (defn.Patron.DrinkWarning2.Length > 0)
            {
                audio.clip = defn.Patron.DrinkWarning2.Random();
                audio.Play();
                yield return new WaitForSeconds(audio.clip.length);
            }

            yield return new WaitForSeconds(3.0f);

            if (defn.Patron.DrinkWarning3.Length > 0)
            {
                audio.clip = defn.Patron.DrinkWarning3.Random();
                audio.Play();
                yield return new WaitForSeconds(audio.clip.length);
            }

            yield return new WaitForSeconds(2.0f);

            LeaveBar(LeaveReason.Bored);
        }

        public void LeaveBar(LeaveReason Reason)
        {
            if (!HasStartedLeavingBar)
            {
                print("I'm off!");
                HasStartedLeavingBar = true;
                BeerHand.Beer.gameObject.SetActive(false);
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
                    gameObject.GetComponent<PatronMouth>().Say("Terrible service...");
                    break;
                case LeaveReason.Satisfied:
                    gameObject.GetComponent<PatronMouth>().Say("Cheers!");
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