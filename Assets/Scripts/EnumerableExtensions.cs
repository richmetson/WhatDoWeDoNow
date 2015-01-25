using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtensions
{
    public static T Random<T>(this IEnumerable<T> sequence)
    {
        var arr = sequence.ToArray();
        if (arr.Length == 0) throw new InvalidOperationException("Sequence is empty");
        return arr[UnityEngine.Random.Range(0, arr.Length)];
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> sequence)
    {
        var arr = sequence.ToList();
        while (arr.Count > 0)
        {
            int idx = UnityEngine.Random.Range(0, arr.Count);
            yield return arr[idx];
            arr.RemoveAt(idx);
        }
    }
}