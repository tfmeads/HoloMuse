using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostNote : MonoBehaviour
{
    //Source and destination of lerp
    GameObject source, target;

    float startTime, finishTime, totalTime;

    Renderer renderer;
    Color startColor, endColor;

    Boolean isActive = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            float lerpFactor = (Time.time - startTime) / totalTime;
            transform.position = Vector3.Lerp(source.transform.position, target.transform.position, lerpFactor);
            renderer.material.SetColor("_Color", Color.Lerp(startColor, endColor, lerpFactor));
        }
    }

    internal void StartLerp(GameObject noteBubble, GameObject targetBubble, float endTime, Color color)
    {
        Debug.Log("StartLerp");
        source = noteBubble;
        target = targetBubble;
        startTime = Time.time;
        finishTime = endTime;
        endColor = color;

        totalTime = finishTime - startTime;

        startColor = new Color(0, 0, 0, 0);

        renderer = GetComponent<Renderer>();

        isActive = true;
    }
}
