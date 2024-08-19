using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPath
{
    public enum Paths
    {
        Diagonal = 2,
        ZigZag = 3, //pass a param for how many walls zig left and zag right
        OpposePlayer = 4, //pass a param for the player
        NineOClockFromPlayer = 5,
        ThreeOClockFromPlayer = 6,
        OpposeEnemy = 7, //pass a param for the enemy
        NineOClockFromEnemy = 8,
        ThreeOClockFromEnemy = 9
    }

    public static List<Vector3> Diagonal(int amtWallsToZigZag, float currRadius)
    {
        List<Vector3> vertices = new List<Vector3>();

        //float angleStep = 360.0f / amtVertices;
        //float angleStepRadians = angleStep * Mathf.Deg2Rad;

        //for (int i = 0; i < amtVertices; i++)
        //{
        //    Vector3 position = new Vector3(
        //        Mathf.Cos(i * angleStepRadians) * currRadius,
        //        0,
        //        Mathf.Sin(i * angleStepRadians) * currRadius
        //    );
        //    vertices.Add(position);
        //}

        return vertices;
    }
}
