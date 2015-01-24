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

        bool IsDrinking;

        // Use this for initialization
        IEnumerator Start()
        {
            Patron Patron = gameObject.GetComponent<PatronDefinition>().Patron;
            while (true)
            {
                yield return new WaitForSeconds(Patron.GapBetweenGulps.PickRandom());
                IsDrinking = true;

                var gulpClip = Gulps[Random.Range(0, Gulps.Length)];
                GulpSource.clip = gulpClip;
                GulpSource.Play();

                yield return new WaitForSeconds(gulpClip.length);
                IsDrinking = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(IsDrinking)
            {
                if(!Beer)
                {
                    Debug.LogError("WHERE IS MY BEER!!!");
                    return;
                }

                Patron Patron = gameObject.GetComponent<PatronDefinition>().Patron;
                float QuantityDrunk = Patron.GulpMagnitude.PickRandom() * Time.deltaTime;
                float OldBeerLevel = Beer.Level;
                Beer.Level = Beer.Level - QuantityDrunk;

                gameObject.GetComponent<Liver>().AdjustDrunkeness(Beer.DrinkStrength * (OldBeerLevel - Beer.Level));
            }
        }

        public void OnDrunkennessAdjusted(float ABV)
        {
            if(ABV >= 1.0f)
            {
                StopCoroutine("Start");
                IsDrinking = false;
            }
        }
    }
}
