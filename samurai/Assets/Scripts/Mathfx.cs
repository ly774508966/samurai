using UnityEngine;
using System;

public class Mathfx
{
    public static float Hermite(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
    }

    public static Vector3 Hermite(Vector3 start, Vector3 end, float value)
    {
        return new Vector3(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value), Hermite(start.z, end.z, value));
    }


    public static float Sinerp(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
    }

    public static Vector3 Sinerp(Vector3 start, Vector3 end, float value)
    {
        return new Vector3(Sinerp(start.x, end.x, value), Sinerp(start.y, end.y, value), Sinerp(start.z, end.z, value));
    }

    public static float Coserp(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));
    }
    public static Vector3 Coserp(Vector3 start, Vector3 end, float value)
    {
        return new Vector3(Coserp(start.x, end.x, value), Coserp(start.y, end.y, value), Coserp(start.z, end.z, value));
    }

    public static float Berp(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);
        value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
        return start + (end - start) * value;
    }

    public static float SmoothStep(float x, float min, float max)
    {
        x = Mathf.Clamp(x, min, max);
        float v1 = (x - min) / (max - min);
        float v2 = (x - min) / (max - min);
        return -2 * v1 * v1 * v1 + 3 * v2 * v2;
    }

    public static float Lerp(float start, float end, float value)
    {
        return ((1.0f - value) * start) + (value * end);
    }

    public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
        Vector3 lineDirection = Vector3.Normalize(lineEnd - lineStart);
        float closestPoint = Vector3.Dot((lineStart - point), lineDirection) / Vector3.Dot(lineDirection, lineDirection);
        return lineStart + (closestPoint * lineDirection);
    }

    public static float DistanceFromPointToVector(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
        Vector3 online = NearestPoint(lineStart, lineEnd, point);

        return (point - online).magnitude;
    }

    public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
        Vector3 fullDirection = lineEnd - lineStart;
        Vector3 lineDirection = Vector3.Normalize(fullDirection);
        float closestPoint = Vector3.Dot((point - lineStart), lineDirection) / Vector3.Dot(lineDirection, lineDirection);
        return lineStart + (Mathf.Clamp(closestPoint, 0.0f, Vector3.Magnitude(fullDirection)) * lineDirection);
    }
    public static float Bounce(float x)
    {
        return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));
    }

    // test for value that is near specified float (due to floating point inprecision)
    // all thanks to Opless for this!
    public static bool Approx(float val, float about, float range)
    {
        return ((Mathf.Abs(val - about) < range));
    }

    // test if a Vector3 is close to another Vector3 (due to floating point inprecision)
    // compares the square of the distance to the square of the range as this 
    // avoids calculating a square root which is much slower than squaring the range
    public static bool Approx(Vector3 val, Vector3 about, float range)
    {
        return ((val - about).sqrMagnitude < range * range);
    }

    /*
      * CLerp - Circular Lerp - is like lerp but handles the wraparound from 0 to 360.
      * This is useful when interpolating eulerAngles and the object
      * crosses the 0/360 boundary.  The standard Lerp function causes the object
      * to rotate in the wrong direction and looks stupid. Clerp fixes that.
      */
    public static float Clerp(float start, float end, float value)
    {
        float min = 0.0f;
        float max = 360.0f;
        float half = Mathf.Abs((max - min) / 2.0f);//half the distance between min and max
        float retval = 0.0f;
        float diff = 0.0f;

        if ((end - start) < -half)
        {
            diff = ((max - start) + end) * value;
            retval = start + diff;
        }
        else if ((end - start) > half)
        {
            diff = -((max - end) + start) * value;
            retval = start + diff;
        }
        else retval = start + (end - start) * value;

        // Debug.Log("Start: "  + start + "   End: " + end + "  Value: " + value + "  Half: " + half + "  Diff: " + diff + "  Retval: " + retval);
        return retval;
    }

    public static Vector3 BezierSpline(Vector3[] pts, float pm)
    {
        float rawParam = pm * pts.Length;
        int currLeg = Mathf.Clamp(Mathf.FloorToInt(rawParam), 0, pts.Length - 1);
        float t = rawParam - currLeg;

        Vector3 st;
        Vector3 en;
        Vector3 ctrl;

        if (currLeg == 0)
        {
            st = pts[0];
            en = (pts[1] + pts[0]) * 0.5f;
            return Vector3.Lerp(st, en, t);
        }
        else if (currLeg == (pts.Length - 1))
        {
            int pBound = pts.Length - 1;
            st = (pts[pBound - 1] + pts[pBound]) * 0.5f;
            en = pts[pBound];
            return Vector3.Lerp(st, en, t);
        }
        else
        {
            st = (pts[currLeg - 1] + pts[currLeg]) * 0.5f;
            en = (pts[currLeg + 1] + pts[currLeg]) * 0.5f;
            ctrl = pts[currLeg];
            return BezInterp(st, en, ctrl, t);
        }
    }

    public static Vector3 SmoothCurveDirection(Vector3[] pts, float pm)
    {
        float rawParam = pm * pts.Length;
        int currLeg = Mathf.Clamp(Mathf.FloorToInt(rawParam), 0, pts.Length - 1);
        float t = rawParam - currLeg;

        Vector3 st;
        Vector3 en;
        Vector3 ctrl;

        if (currLeg == 0)
        {
            st = pts[0];
            en = (pts[1] + pts[0]) * 0.5f;
            return en - st;
        }
        else if (currLeg == (pts.Length - 1))
        {
            int pBound = pts.Length - 1;
            st = (pts[pBound - 1] + pts[pBound]) * 0.5f;
            en = pts[pBound];
            return en - st;
        }
        else
        {
            st = (pts[currLeg - 1] + pts[currLeg]) * 0.5f;
            en = (pts[currLeg + 1] + pts[currLeg]) * 0.5f;
            ctrl = pts[currLeg];
            return BezDirection(st, en, ctrl, t);
        }
    }

    static Vector3 BezInterp(Vector3 st, Vector3 en, Vector3 ctrl, float t)
    {
        float d = 1.0f - t;
        return d * d * st + 2.0f * d * t * ctrl + t * t * en;
    }


    static Vector3 BezDirection(Vector3 st, Vector3 en, Vector3 ctrl, float t)
    {
        return (2.0f * st - 4.0f * ctrl + 2.0f * en) * t + 2.0f * ctrl - 2.0f * st;
    }


}