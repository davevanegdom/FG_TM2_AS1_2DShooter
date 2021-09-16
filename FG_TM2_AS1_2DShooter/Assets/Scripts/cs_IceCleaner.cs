using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_IceCleaner : MonoBehaviour
{
    public List<Transform> corners = new List<Transform>();

    public int TargetCorner;
    public int FromCorner;
    public float Speed;
  
    public void Awake()
    {
        TargetCorner = Random.Range(0, corners.Count);
        transform.position = corners[TargetCorner].position;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, corners[TargetCorner].position) < 0.1f)
        {
            RandomizeCornerTarget();
        }
        transform.position = transform.position + (corners[TargetCorner].position - transform.position).normalized * Speed * Time.deltaTime;

        //Smooth rotation in new direction

    }

    private void RandomizeCornerTarget()
    {
        int OldTarget = TargetCorner;

        TargetCorner = Random.Range(0, corners.Count);
        while (TargetCorner == FromCorner || TargetCorner == OldTarget)
        {
            TargetCorner = Random.Range(0, corners.Count);
        }

        FromCorner = OldTarget;
    }
}
