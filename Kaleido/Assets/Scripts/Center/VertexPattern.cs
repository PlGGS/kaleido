using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VertexPattern
{
    public enum Patterns
    {
        Circle = 1,
        Infinity = 2
    }

    public static List<Vector3> Circle(int amtVertices, float currRadius)
    {
        List<Vector3> vertices = new List<Vector3>();

        float angleStep = 360.0f / amtVertices;
        float angleStepRadians = angleStep * Mathf.Deg2Rad;

        for (int i = 0; i < amtVertices; i++)
        {
            Vector3 position = new Vector3(
                Mathf.Cos(i * angleStepRadians) * currRadius,
                0,
                Mathf.Sin(i * angleStepRadians) * currRadius
            );
            vertices.Add(position);
        }

        return vertices;
    }

    public static List<Vector3> Infinity(int amtVertices, float currRadius)
    {
        List<Vector3> vertices = new List<Vector3>();

        float angleStep = 2 * Mathf.PI / amtVertices;

        for (int i = 0; i < amtVertices; i++)
        {
            float t = i * angleStep;
            float x = (Mathf.Sin(t) * currRadius) / (1 + Mathf.Pow(Mathf.Cos(t), 2));
            float z = (Mathf.Sin(t) * Mathf.Cos(t) * currRadius * 1.5f) / (1 + Mathf.Pow(Mathf.Cos(t), 2));

            Vector3 position = new Vector3(x, 0, z);
            vertices.Add(position);
        }

        return vertices;
    }
}
