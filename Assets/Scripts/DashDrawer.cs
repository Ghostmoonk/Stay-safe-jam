using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe unique qui dessine la ligne de dash
public static class DashDrawer
{

    public static void DrawLine(LineRenderer line, Vector3 startPoint, Vector3 endPoint, Color color, float width)
    {
        line.positionCount = 2;
        line.SetPosition(0, new Vector3(startPoint.x, startPoint.y, -0.1f));
        line.SetPosition(1, new Vector3(endPoint.x, endPoint.y, -0.1f));
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

    public static void DrawTajectoryLine(LineRenderer line, Color color, Vector3 startPos, int numberOfPoints, Vector2 initialVelocity)
    {
        //yt = v0y*t - 1/2 * g * t²
        line.positionCount = numberOfPoints;
        line.startColor = color;
        line.endColor = color;
        line.startWidth = 0.1f;
        for (int i = 0; i < numberOfPoints; i++)
        {
            Vector3 pos = CalculatePosInTime(startPos, initialVelocity, i / (float)numberOfPoints);
            line.SetPosition(i, new Vector3(pos.x, pos.y, -0.1f));
        }
    }

    public static Vector3 CalculatePosInTime(Vector2 startPos, Vector2 vo, float time)
    {
        Vector3 initialVelocity = vo;
        initialVelocity.y = 0f;

        Vector3 result = startPos + vo * time;

        float sY = (-0.5f * Mathf.Abs(Physics2D.gravity.y) * (time * time)) + (vo.y * time) + startPos.y;

        result.y = sY;

        return result;
    }
}
