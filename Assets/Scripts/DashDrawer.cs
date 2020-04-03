using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe unique qui dessine la ligne de dash
public static class DashDrawer
{

    public static void DrawLine(LineRenderer line, Vector3 startPoint, Vector3 endPoint, Color color, float width)
    {
        line.positionCount = 2;
        line.SetPosition(0, startPoint);
        line.SetPosition(1, endPoint);
        line.startColor = color;
        line.startWidth = width;
    }

    public static void ClearLine(LineRenderer line)
    {
        line.positionCount = 0;
    }

    public static IEnumerator FadeLine(LineRenderer line, float duration)
    {
        float lerpValue = 0f;
        for (int i = 0; i < line.positionCount - 1; i++)
        {
            Vector3 startPos = line.GetPosition(i);
            while (lerpValue <= 1f)
            {
                line.SetPosition(i, Vector3.Lerp(startPos, line.GetPosition(i + 1), lerpValue));
                lerpValue += Time.deltaTime / duration;
                yield return null;
            }
            lerpValue = 0;
            ClearLine(line);
        }
    }
}
