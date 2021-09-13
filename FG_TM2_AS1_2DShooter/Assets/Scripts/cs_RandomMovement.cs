using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class cs_RandomMovement : MonoBehaviour
{

    public float timer;

    public float Timelimit;

    public float speed;

    public Vector3 target;
    

    void Start()
    {
        
    }

    
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= Timelimit)
        {
            newTarget();
            timer = 0;

        }
       
        transform.position += speed * (target - transform.position) * Time.deltaTime;
       // transform.Translate(Vector3.up * Time.deltaTime);
    }
    void newTarget ()
    {
        float myX = gameObject.transform.position.x;
        float myY = gameObject.transform.position.y;

        float xPos = myX + Random.Range(-5, 5);
        float yPos = myY + Random.Range(-5, 5);
        target = new Vector2(xPos,yPos);
    }

    public IEnumerator IceCleanerMovement()
    {
        float time = 0f;

        while (time < timer)
        {
            time += Time.deltaTime;
            yield return null;
        }


    }

}
