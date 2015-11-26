using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundry        // class describe how far player and enemies can move on the map
{
    public float left, right, bottom, up;

    public Boundry(float left, float right, float bottom, float up)
    {
        this.left = left;
        this.right = right;
        this.bottom = bottom;
        this.up = up;
    }
}
