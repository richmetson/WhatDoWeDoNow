using UnityEngine;
using System.Collections;

namespace AgonyBartender
{
    interface IAlchoholAffectedSystem
    {
        void OnDrunkennessAdjusted(float ABV);
    }

    [RequireComponent(typeof(PatronDefinition))]
    public class Liver : MonoBehaviour {

        public float StartingABV;

        float CurrentABV;

	    // Use this for initialization
	    void Start () {
            CurrentABV = 0.0f;
            AdjustDrunkeness(StartingABV);
	    }
	
	    // Update is called once per frame
	    void Update () {
	
	    }

        public void AdjustDrunkeness(float DeltaDrunkeness)
        {
            CurrentABV += DeltaDrunkeness;

            gameObject.SendMessage("OnDrunkennessAdjusted", CurrentABV, SendMessageOptions.DontRequireReceiver);
        }
    }
}
