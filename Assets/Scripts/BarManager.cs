
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace AgonyBartender
{
    public class BarManager : MonoBehaviour
    {

        List<BarStool> BarStools;
        BarStool CurrentBarStool;
        int CurrentBarStoolIndex;

        public RangedFloat EmptyStoolTime;

        public Transform LeftmostBarStool;
        public Transform RightmostBarStool;
        public Transform FirstMidBarStool;

        public Button LeftButton;
        public Button RightButton;

        public GameObject PatronPrefab;

        public StandardProblemList StandardProblems;
        public Patron[] PatronArchetypes;

        // Use this for initialization
        void Start()
        {
            CurrentBarStool = null;
            SetBarLength(7);

            FillBarStool();
            FillBarStool();
            FillBarStool();

            ScheduleNewPatron();

            MoveToBarStool(BarStools[0]);
        }

        public float StoolSpacing = 2500;

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

        Patron ChoosePatron()
        {
            if (PatronArchetypes.Length == 0)
            {
                Debug.LogError("No patrons found, have you considered advertising?");
                return null;
            }

            return PatronArchetypes[Random.Range(0, PatronArchetypes.Length)];
        }

        Problem ChooseProblem(Patron Patron)
        {
            var problems = Patron.PatronsProblems.Concat(StandardProblems.GlobalProblems).ToArray();
            return problems[Random.Range(0, problems.Length)]; 
        }

        public void FillBarStool()
        {
            var candidateStools = BarStools.Where(s => !s.IsActive && !s.CurrentPatron).ToArray();
            var stool = candidateStools[Random.Range(0, candidateStools.Length)];

            GameObject NewPatron = (GameObject)Instantiate(PatronPrefab);
            Patron ChosenPatron = ChoosePatron();
            NewPatron.GetComponent<PatronDefinition>().Patron = ChoosePatron();
            NewPatron.GetComponent<PatronDefinition>().SetProblem(ChooseProblem(ChosenPatron));

            stool.CurrentPatron = NewPatron;

            ScheduleNewPatron();
        }

        public void ScheduleNewPatron()
        {
            Invoke("FillBarStool", EmptyStoolTime.PickRandom());
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

        public void MoveToBarStool(BarStool NewBarStool)
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