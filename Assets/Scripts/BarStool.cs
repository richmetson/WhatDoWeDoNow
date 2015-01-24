using UnityEngine;
using System.Collections;
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
        bool HasPatron;
        public Vector3 PatronPosition;

        public delegate void PatronLeaves(BarStool Entry);

        public event PatronLeaves OnPatronLeaves;

        void BarStoolEntry_OnPatronLeaves()
        {
            if (OnPatronLeaves != null)
            {
                OnPatronLeaves(this);
            }
        }

        public void FillSeat(GameObject Patron)
        {
            BeerHand BeerHand = Patron.GetComponent<BeerHand>();
            BeerHand.Beer = Drink;

            PatronMouth Mouth = Patron.GetComponent<PatronMouth>();
            Mouth.ProblemSpeech = PatronSpeech;

            Patron.transform.SetParent(transform);
            Patron.transform.localScale = Vector3.one;
            Patron.transform.localPosition = PatronPosition;
            Patron.transform.SetSiblingIndex(3);

            Patron.GetComponent<PatronStatusMonitor>().OnPatronLeaves += BarStoolEntry_OnPatronLeaves;

            HasPatron = true;
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

        // Use this for initialization
        void Start()
        {
            PatronStatusMonitor Patron = GetComponentInChildren<PatronStatusMonitor>();
            if (Patron != null)
            {
                HasPatron = true;
                Patron.OnPatronLeaves += BarStoolEntry_OnPatronLeaves;
                PatronPosition = Patron.transform.localPosition;
            }
            else
            {
                HasPatron = false;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}