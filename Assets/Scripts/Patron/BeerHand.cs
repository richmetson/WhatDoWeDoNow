using UnityEngine;
using System.Collections;
namespace AgonyBartender
{
    [RequireComponent(typeof(PatronDefinition))]
    [RequireComponent(typeof(Liver))]
    public class BeerHand : MonoBehaviour, IAlchoholAffectedSystem
    {
        public Drink Beer;
        public AudioSource GulpSource;
        public AudioClip[] Gulps;

        // Use this for initialization
        IEnumerator Start()
        {
            Patron Patron = gameObject.GetComponent<PatronDefinition>().Patron;
            while (true)
            {
                yield return new WaitForSeconds(Patron.GapBetweenGulps.PickRandom());

                if (Beer.IsBeingFilled) continue;

                Beer.IsBeingDrunk = true;

                var gulpClip = Gulps.Random();
                GulpSource.clip = gulpClip;
                GulpSource.Play();

                float gulpSize = Patron.GulpMagnitude.PickRandom();
                float startTime = Time.time;
                while (Time.time < startTime + gulpClip.length)
                {
                    Beer.Level -= gulpSize*(Time.deltaTime/gulpClip.length);
                    yield return null;
                }
                GetComponent<Liver>().AdjustDrunkeness(Beer.DrinkStrength * gulpSize);

                Beer.IsBeingDrunk = false;
            }
        }

        public void OnDrunkennessAdjusted(float ABV)
        {
            if(ABV >= 1.0f)
            {
                StopCoroutine("Start");
            }
        }
    }
}
