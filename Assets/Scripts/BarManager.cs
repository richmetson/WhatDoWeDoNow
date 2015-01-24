using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace AgonyBartender
{
    
    public class BarStoolEntry
    {
        GameObject RootEntry;

        bool IsActive;
        bool HasPatron;
        Vector3 PatronPosition;

        public delegate void PatronLeaves(BarStoolEntry Entry);

        public event PatronLeaves OnPatronLeaves;
        
        public BarStoolEntry(GameObject Root)
        {
            RootEntry = Root;
            IsActive = false;
            PatronStatusMonitor Patron = Root.GetComponentInChildren<PatronStatusMonitor>();
            if(Patron != null)
            {
                HasPatron = true;
                Patron.OnPatronLeaves += BarStoolEntry_OnPatronLeaves;
                PatronPosition = Patron.transform.localPosition;
            }
            else
            {
                HasPatron = false;
                PatronPosition = new Vector3(626, -413);
            }
        }

        void BarStoolEntry_OnPatronLeaves()
        {
            if(OnPatronLeaves != null)
            {
                OnPatronLeaves(this);
            }
        }

        public void FillSeat(GameObject Patron)
        {
            BeerHand BeerHand = Patron.GetComponent<BeerHand>();
            BeerHand.Beer = RootEntry.GetComponentInChildren<Drink>();


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
            return RootEntry.transform.localPosition.x;
        }
    }

    public class BarManager : MonoBehaviour
    {

        List<BarStoolEntry> BarStools;
        BarStoolEntry CurrentBarStool;
        int CurrentBarStoolIndex;

        public RangedFloat EmptyStoolTime;

        public Transform LeftmostBarStool;
        public Transform RightmostBarStool;
        public Transform FirstMidBarStool;

        public Button LeftButton;
        public Button RightButton;

        public GameObject PatronPrefab;

        // Use this for initialization
        void Start()
        {
            CurrentBarStool = null;
            SetBarLength(7);
        }

        public float StoolSpacing = 2500;

        public void SetBarLength(int numStools)
        {
            if (numStools < 3) throw new ArgumentOutOfRangeException("numStools", numStools, "Must always have at least 3 bar stools");

            while (transform.childCount > numStools)
            {
                Destroy(transform.GetChild(2).gameObject);
            }

            while (transform.childCount < numStools)
            {
                var newStool = (Transform) Instantiate(FirstMidBarStool);
                newStool.SetParent(transform);
                newStool.localScale = Vector3.one;
                newStool.SetSiblingIndex(transform.childCount - 2);
            }

            BarStools = new List<BarStoolEntry>();
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).localPosition = new Vector3(i*StoolSpacing, 0, 0);
                BarStoolEntry Entry = new BarStoolEntry(transform.GetChild(i).gameObject)
                Entry.OnPatronLeaves +=Entry_OnPatronLeaves;
                BarStools.Add(Entry);
            }

            MoveToBarStool(BarStools[0]);
        }

        void Entry_OnPatronLeaves(BarStoolEntry Entry)
        {
            StartCoroutine(FillBarStool(Entry));
        }

        IEnumerator FillBarStool(BarStoolEntry Entry)
        {
            yield return new WaitForSeconds(EmptyStoolTime.PickRandom());

            GameObject NewPatron = (GameObject)Instantiate(PatronPrefab);

            Entry.FillSeat(NewPatron);
        }

        public bool CanMoveLeft()
        {
            return CurrentBarStoolIndex != 0;
        }

        public bool CanMoveRight()
        {
            return CurrentBarStoolIndex < BarStools.Count - 1;
        }

        public void MoveLeftBarStool()
        {
            if(!CanMoveLeft())
            {
                Debug.LogError("Moving left off the edge of the bar");
                return;
            }

            MoveToBarStool(BarStools[CurrentBarStoolIndex-1]);
        }

        public void MoveRightBarStool()
        {
            if (!CanMoveRight())
            {
                Debug.LogError("Moving right off the edge of the bar");
                return;
            }

            MoveToBarStool(BarStools[CurrentBarStoolIndex+1]);
        }

        public void MoveToBarStool(BarStoolEntry NewBarStool)
        {
            if(CurrentBarStool != null)
            {
                CurrentBarStool.SwitchFromBarStool();
            }

            NewBarStool.SwitchToBarStool();

            CurrentBarStool = NewBarStool;
            CurrentBarStoolIndex = BarStools.IndexOf(CurrentBarStool);
            LeftButton.gameObject.SetActive(CanMoveLeft());
            RightButton.gameObject.SetActive(CanMoveRight());

            Vector3 NewPosition = gameObject.transform.localPosition;
            NewPosition.x = -CurrentBarStool.GetCameraXPosition();
            gameObject.transform.localPosition = NewPosition;
        }
    }
}