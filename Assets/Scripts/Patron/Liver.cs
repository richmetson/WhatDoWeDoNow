using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;

namespace AgonyBartender
{
    interface IAlchoholAffectedSystem
    {
        void OnDrunkennessAdjusted(float ABV);
    }

    [RequireComponent(typeof(PatronDefinition))]
    public class Liver : MonoBehaviour
    {

        private IAlchoholAffectedSystem[] _affectedSystems;

        public float StartingABV;

        [SerializeField] private float _currentAbv;

	    // Use this for initialization
	    void Start ()
	    {
            _affectedSystems = GetComponents(typeof(IAlchoholAffectedSystem)).Cast<IAlchoholAffectedSystem>().ToArray();
            _currentAbv = 0.0f;
            AdjustDrunkeness(StartingABV);
	    }

        public UnityEvent OnLiverFailed;

        public void AdjustDrunkeness(float deltaDrunkeness)
        {
            _currentAbv += deltaDrunkeness * gameObject.GetComponent<PatronDefinition>().Patron.AlcoholIntolerance;

            if (_currentAbv >= 1f)
                OnLiverFailed.Invoke();

            foreach (var system in _affectedSystems)
                system.OnDrunkennessAdjusted(_currentAbv);
        }
    }
}
