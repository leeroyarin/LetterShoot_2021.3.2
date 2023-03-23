using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class LaserShooter : MonoBehaviour
{

    LineRenderer lineRenderer;
    TrailRenderer trailRenderer;

    Transform[] Transform;
    private void Awake()
    {
        
    }

    private void Start()
    {
       
    }

    IEnumerator GetTrialToAttaclGameOjects()
    {
        yield return new WaitForSeconds(1);
    }
}
