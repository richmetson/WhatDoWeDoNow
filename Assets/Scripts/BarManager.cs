
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

namespace AgonyBartender
{
    public class BarManager : MonoBehaviour
    {

        List<BarStool> BarStools;
        BarStool CurrentBarStool;
        int CurrentBarStoolIndex;

        public Transform LeftmostBarStool;
        public Transform RightmostBarStool;
        public Transform FirstMidBarStool;

        public Button LeftButton;
        public Button RightButton;

        public GameObject PatronPrefab;

        public StandardProblemList StandardProblems;

        public float StoolSpacing = 2500;

        public PatronStatusMonitor.PatronTippingEvent OnPatronTipping;

        public void SetBarLength(int numStools)
        {
            if (numStools < 3) throw new System.ArgumentOutOfRangeException("numStools", numStools, "Must always have at least 3 bar stools");

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

            BarStools = new List<BarStool>();
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).localPosition = new Vector3(i*StoolSpacing, 0, 0);
                BarStool Entry = transform.GetChild(i).GetComponent<BarStool>();
                BarStools.Add(Entry);
            }
        }

        public void DeletePatrons()
        {
            foreach (var defn in GetComponentsInChildren<PatronStatusMonitor>())
            {
                defn.LeaveBar(PatronStatusMonitor.LeaveReason.PubClosing);
            }
        }

        public void FillBarStool(Patron patron, Problem problem)
        {
            var candidateStools = BarStools.Where(s => !s.IsActive && !s.CurrentPatron).ToArray();
            
            if (candidateStools.Length == 0) return;

            var stool = candidateStools.Random();

            GameObject NewPatron = (GameObject)Instantiate(PatronPrefab);
            NewPatron.GetComponent<PatronDefinition>().Patron = patron;
            NewPatron.GetComponent<PatronDefinition>().ActiveProblem = problem;

            NewPatron.GetComponent<PatronStatusMonitor>().OnPatronTipping.AddListener(OnPatronTip);

            stool.CurrentPatron = NewPatron;
        }

        public bool EnableArriveSounds;

        public void OnPatronArrived(BarStool stool)
        {
            if (EnableArriveSounds)
            {
                stool.CurrentPatron.GetComponent<PatronMouth>().PlayArriveSound();
            }
        }

        void OnPatronTip(int Amount)
        {
            OnPatronTipping.Invoke(Amount);
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

        public void MoveToBarStool(int index)
        {
            MoveToBarStool(BarStools[index]);
        }

        public void MoveToBarStool(BarStool NewBarStool)
        {
            if(CurrentBarStool != null)
            {
                CurrentBarStool.SwitchFromBarStool();
            }

            if (NewBarStool == null)
            {
                CurrentBarStool = null;
                return;
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

        public int GetStoolOffsetFromCurrent(Transform barStool)
        {
            int index = BarStools.FindIndex(e => e.gameObject == barStool.gameObject);
            if(index < 0)
            {
                Debug.LogError("Could not find " + barStool.gameObject);
            }
            return index - CurrentBarStoolIndex;
        }
    }
}