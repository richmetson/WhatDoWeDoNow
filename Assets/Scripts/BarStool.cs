using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace AgonyBartender
{
    public class BarStool : MonoBehaviour
    {
        public SpeechBubble BackgroundSpeech;
        public SpeechBubble PatronSpeech;
        public Drink Drink;

        public bool IsActive
        {
            get;
            private set;
        }

        public void Awake()
        {
            PatronSpeech.gameObject.SetActive(false);
            Drink.gameObject.SetActive(false);
        }
        
        public Vector3 PatronPosition;

        private GameObject _currentPatron;
        public GameObject CurrentPatron
        {
            get { return _currentPatron; }
            set
            {
                _currentPatron = value;
                if (_currentPatron == null) return;

                BeerHand BeerHand = _currentPatron.GetComponent<BeerHand>();
                BeerHand.Beer = Drink;
                Drink.gameObject.SetActive(true);
                Drink.Level = 1.0f;
                // TODO: Pay player for this beer

                PatronMouth Mouth = _currentPatron.GetComponent<PatronMouth>();
                Mouth.ProblemSpeech = PatronSpeech;

                _currentPatron.transform.SetParent(transform);
                _currentPatron.transform.localScale = Vector3.one;
                _currentPatron.transform.localPosition = PatronPosition;
                _currentPatron.transform.SetSiblingIndex(3);

                _currentPatron.GetComponent<PatronStatusMonitor>().OnPatronLeaving.AddListener(BarStoolEntry_OnPatronLeft);

                OnPatronArrived.Invoke(this);
            }
        }

        [Serializable]
        public class BarStoolEvent : UnityEvent<BarStool> { }

        public BarStoolEvent OnPatronArrived;
        public BarStoolEvent OnPatronPoisoned;
        public BarStoolEvent OnPatronUnsatisfied;
        public BarStoolEvent OnPatronLeft;

        void BarStoolEntry_OnPatronLeft(PatronStatusMonitor patron, PatronStatusMonitor.LeaveReason reason)
        {
            if (reason == PatronStatusMonitor.LeaveReason.PassedOut)
                OnPatronPoisoned.Invoke(this);

            if (reason == PatronStatusMonitor.LeaveReason.Bored)
                OnPatronUnsatisfied.Invoke(this);

            if (reason == PatronStatusMonitor.LeaveReason.Angry)
                OnPatronUnsatisfied.Invoke(this);

            OnPatronLeft.Invoke(this);
        }

        public void SwitchToBarStool()
        {
            IsActive = true;
        }

        public void SwitchFromBarStool()
        {
            IsActive = false;
        }

        public float GetCameraXPosition()
        {
            return transform.localPosition.x;
        }
    }

}