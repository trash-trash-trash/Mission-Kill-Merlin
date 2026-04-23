
using System.Collections;
using UnityEngine;

public class StraightLineRenderer : MonoBehaviour
{
    public Transform pointA;  
    public Transform pointB;  
    public LayerMask hitLayers;

    public LineRenderer line;

    public bool aiming = false;
    
    void ShowHitLine(Vector3 start, Vector3 end, bool input)
    {
        if (input)
        {
            line.positionCount = 2;
            line.SetPosition(0, start);
            line.SetPosition(1, end);
            line.startWidth = 1f;
            line.endWidth = 1f;
            line.startColor = Color.red;
            line.endColor = Color.red;
            line.enabled = true;
        }
        else
        {
            line.enabled = false;
        }
    }

    public void Update()
    {
        if (aiming)
        {
            ShowHitLine(pointA.position, pointB.position,true);
        }
        else if (line.enabled)
            ShowHitLine(pointA.position, pointB.position, false);
    }

}
