using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace AgonyBartender
{
    [RequireComponent(typeof(PatronDefinition))]
    [RequireComponent(typeof(Image))]
    public class PatronFaceController : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            OnFaceChanged(ProblemSolutionFacialExpression.NeutralResponse);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnFaceChanged(ProblemSolutionFacialExpression NewExpression)
        {
            if(NewExpression == ProblemSolutionFacialExpression.FacialExpressionCount)
            {
                Debug.LogError("Invalid facial expression");
            }

            PatronDefinition Patron = gameObject.GetComponent<PatronDefinition>();
            Sprite NewFace = Patron.Patron.FaceSprites[(int)NewExpression];

            Image ImageComponent = gameObject.GetComponent<Image>();
            ImageComponent.sprite = NewFace;
        }
    }

}
