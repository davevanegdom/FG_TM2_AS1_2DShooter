using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_PuckPulseEffect : MonoBehaviour
{
    public float targetTime;
    public float size;
    public bool isPlaying;
    private float time;
    private SpriteRenderer sr;
    private Color serializedColor;


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        serializedColor = sr.color;
        isPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            timer();
        }
        else
        {
            time = 0f;
        }
    }

    private void timer()
    {
        time += Time.deltaTime / targetTime;

        if (time <= targetTime)
        {
            Animate(time);
        }
        else
        {
            time = 0f;
        }
    }

    void Animate(float time)
    {
        //size
        transform.localScale = Vector2.Lerp(Vector2.zero, Vector2.one * (size / (1/transform.parent.localScale.x)), time);

        //alpha
        sr.color = Vector4.Lerp(serializedColor, new Color(serializedColor.r, serializedColor.g, serializedColor.b, 0), time);
    }
}
