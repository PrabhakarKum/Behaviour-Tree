using System;
using System.Collections.Generic;

public static class Utils
{
    public static readonly Random random = new System.Random();
    public static void Shuffle<T>(this IList<T> list)
    {
        var n = list.Count;
        while(n > 1)
        {
            n--;
            var k = random.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}
