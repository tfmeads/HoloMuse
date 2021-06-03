using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject bubbles = GameObject.Find("OpenStringBubbles");

        foreach(Transform child in bubbles.transform){
            Debug.Log(child.name);

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
