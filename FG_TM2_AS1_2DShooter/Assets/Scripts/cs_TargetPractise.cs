using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_TargetPractise : MonoBehaviour
{
    public GameObject prefabTarget;

    [SerializeField] Transform topWall;
    [SerializeField] Transform bottomWall;
    [SerializeField] Transform leftWall;
    [SerializeField] Transform rightWall;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnTarget", 2f, 1.5f);
    }

    void SpawnTarget()
    {
        float x = Random.Range(leftWall.position.x + 0.25f, rightWall.position.x - 0.25f);
        float y = Random.Range(bottomWall.position.y + 0.25f, topWall.position.y - 0.25f);
        Vector2 spawnPos = new Vector2(x, y);
        GameObject target = Instantiate(prefabTarget, spawnPos, Quaternion.identity);
        target.transform.parent = transform;
    }
}
