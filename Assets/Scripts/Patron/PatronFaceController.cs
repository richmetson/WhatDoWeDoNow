using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace AgonyBartender
{
    [RequireComponent(typeof(PatronDefinition))]
    [RequireComponent(typeof(Image))]
    public class PatronFaceController : MonoBehaviour, IAlchoholAffectedSystem
    {

        float FacialRotation;
        public float WobblePeriod;

        public float MaxRotation = 25.0f;

        // Use this for initialization
        void Start()
        {
            OnFaceChanged(ProblemSolutionFacialExpression.NeutralResponse);
            StartCoroutine(WobbleHead());
        }

        // Update is called once per frame
        void Update()
        {
        }

        IEnumerator WobbleHead()
        {
            Image ImageComponent = gameObject.GetComponent<Image>();
            while (true)
            {
                print(FacialRotation);
                for (float Rotation = 0.0f; Rotation <= FacialRotation; Rotation += (FacialRotation / WobblePeriod) * Time.deltaTime)
                {
                    ImageComponent.transform.rotation = Quaternion.Euler(0, 0, Rotation);
                    yield return null;
                }

                for (float Rotation = FacialRotation; Rotation >= -FacialRotation; Rotation -= (FacialRotation / WobblePeriod) * Time.deltaTime)
                {
                    ImageComponent.transform.rotation = Quaternion.Euler(0, 0, Rotation);
                    yield return null;
                }

                for (float Rotation = -FacialRotation; Rotation <= 0.0f; Rotation += (FacialRotation / WobblePeriod) * Time.deltaTime)
                {
                    ImageComponent.transform.rotation = Quaternion.Euler(0, 0, Rotation);
                    yield return null;
                }
            }
        }

        public void OnFaceChanged(ProblemSolutionFacialExpression NewExpression)
        {
            PatronDefinition Patron = gameObject.GetComponent<PatronDefinition>();
            Sprite NewFace = Patron.Patron.FaceSprites[(int)NewExpression];

            Image ImageComponent = gameObject.GetComponent<Image>();
            ImageComponent.sprite = NewFace;
        }

        public void OnDrunkennessAdjusted(float ABV)
        {
            FacialRotation = ABV * MaxRotation;
        }
    }

}
