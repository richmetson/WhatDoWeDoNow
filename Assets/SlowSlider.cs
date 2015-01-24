using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public struct Slide
{
    public Vector3 StartPosition;
    public Vector3 EndPosition;
    public float ZoomTime;

    public AnimationCurve Curve;
}

[RequireComponent(typeof(UnityEngine.UI.Text))]
public class SlowSlider : MonoBehaviour {

    public List<Slide> Slides;

    float TimeElapsed;
	// Use this for initialization
	void Start () {
        TimeElapsed = 0.0f;
        StartCoroutine(UpdatePosition());
	}
	

    IEnumerator UpdatePosition()
    {
        foreach(Slide s in Slides)
        {
            while(TimeElapsed <= s.ZoomTime)
            {
                transform.localPosition = Vector3.Lerp(s.StartPosition, s.EndPosition, s.Curve.Evaluate(TimeElapsed / s.ZoomTime));
                TimeElapsed += Time.deltaTime;
                yield return null;
            }

            TimeElapsed = Time.deltaTime;
        }
    }
}
