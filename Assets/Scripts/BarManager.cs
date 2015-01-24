using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AgonyBartender
{
    
    public class BarStoolEntry
    {
        GameObject RootEntry;

        bool IsActive;
        
        public BarStoolEntry(GameObject Root)
        {
            RootEntry = Root;
            IsActive = false;
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

        // Use this for initialization
        void Start()
        {
            BarStools = new List<BarStoolEntry>();
            foreach(Transform child in transform)
            {
                BarStools.Add(new BarStoolEntry(child.gameObject));
            }
            CurrentBarStool = null;
            CurrentBarStoolIndex = 0;
            MoveToBarStool(BarStools[CurrentBarStoolIndex]);
        }

        // Update is called once per frame
        void Update()
        {

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

            --CurrentBarStoolIndex;
            MoveToBarStool(BarStools[CurrentBarStoolIndex]);
        }

        public void MoveRightBarStool()
        {
            if (!CanMoveRight())
            {
                Debug.LogError("Moving right off the edge of the bar");
                return;
            }

            ++CurrentBarStoolIndex;
            MoveToBarStool(BarStools[CurrentBarStoolIndex]);
        }

        public void MoveToBarStool(BarStoolEntry NewBarStool)
        {
            if(CurrentBarStool != null)
            {
                CurrentBarStool.SwitchFromBarStool();
            }

            NewBarStool.SwitchToBarStool();

            CurrentBarStool = NewBarStool;

            Vector3 NewPosition = gameObject.transform.localPosition;
            print("Putting bar at: " + (-CurrentBarStool.GetCameraXPosition()));
            NewPosition.x = -CurrentBarStool.GetCameraXPosition();
            gameObject.transform.localPosition = NewPosition;
        }
    }
}