using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class AimInAStraightLine : MonoBehaviour
{
    public Transform aimOrigin;  
    public float aimDist = Single.MaxValue;
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
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
            line.startColor = Color.red;
            line.endColor = Color.red;
            line.enabled = true;
        }
        else
        {
            line.enabled = false;
        }
    }
    
    public void Aim()
    {
        aiming = true;
    }


    public void Holster()
    {
        aiming = false;
    }

    public void Update()
    {
        if (aiming)
        {
            ShowHitLine(aimOrigin.position, aimOrigin.forward * aimDist,true);
        }
        else if (line.enabled)
            ShowHitLine(aimOrigin.position, aimOrigin.forward * aimDist, false);
    }
}
