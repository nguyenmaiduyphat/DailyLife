using System.Collections;
using UnityEngine;

/// <summary>
/// This class store function to animate UI in screen when interact 
/// (hit button or input key)
/// </summary>
public class UIAnimation
{
    public static IEnumerator ZoomOut(RectTransform target, float time)
    {
        target.localScale = Vector3.zero;
        Vector3 start = Vector3.zero;
        Vector3 end = Vector3.one;
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / time);
            target.localScale = Vector3.Lerp(start, end, t);
            yield return null;
        }

        target.localScale = end;
    }


    public static IEnumerator ZoomIn(RectTransform target, float time)
    {
        target.localScale = Vector3.one;
        Vector3 start = Vector3.one;
        Vector3 end = Vector3.zero;
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / time);
            target.localScale = Vector3.Lerp(start, end, t);
            yield return null;
        }

        target.localScale = end;
    }
}
