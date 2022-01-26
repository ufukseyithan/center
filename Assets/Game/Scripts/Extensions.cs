using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions {
    public static bool IsEqualTo(this Color c1, Color c2, int digits = 2) => Math.Round(c1.r, digits) == Math.Round(c2.r, digits) && Math.Round(c1.g, digits) == Math.Round(c2.g, digits) && Math.Round(c1.b, digits) == Math.Round(c2.b, digits) && Math.Round(c1.a, digits) == Math.Round(c2.a, digits);
    
    public static T RandomElement<T>(this IEnumerable<T> enumerable) {
        int index = UnityEngine.Random.Range(0, enumerable.Count());
        return enumerable.ElementAt(index);
    }
}