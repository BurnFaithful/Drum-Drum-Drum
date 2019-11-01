using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static int Decode36(string digits36)
    {
        int result = 0;

        if (digits36[1] >= 'A')
            result += digits36[1] - 'A' + 10;
        else
            result += digits36[1] - '0';

        if (digits36[0] >= 'A')
            result += (digits36[0] - 'A' + 10) * 36;
        else
            result += (digits36[0] - '0') * 36;

        return result;
    }

    public static float FAbs(float value)
    {
        return value > 0 ? value : -value;
    }

    public static float FDistance(float a, float b)
    {
        return FAbs(a - b);
    }

    public static void CalculateDigit(ref Queue<int> destQueue, int decimalValue)
    {
        while (decimalValue > 0)
        {
            destQueue.Enqueue(decimalValue % 10);
            decimalValue = decimalValue / 10;
        }
    }

    public static void RollingNumber()
    {

    }
}
