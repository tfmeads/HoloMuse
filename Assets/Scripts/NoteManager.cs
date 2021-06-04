using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //TODO change to find co-sibling in Fretboard prefab for efficiency's sake
        ///and in case we want to have multiple notemanagers/fretboards in the future
        GameObject bubbles = GameObject.Find("OpenStringBubbles");

        foreach(Transform child in bubbles.transform){
            Debug.Log(child.name);

            MuseNote note = child.GetComponent<MuseNote>();

            if(note != null)
            {
                Debug.Log(note.GetNoteValue());
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
