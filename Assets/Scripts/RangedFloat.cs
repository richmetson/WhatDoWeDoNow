using System;
using UnityEngine;
using System.Collections;

[Serializable]
public struct RangedFloat
{
    public float Min;
    public float Max;

    public float PickRandom()
    {
        return UnityEngine.Random.Range(Min, Max);
    }
}