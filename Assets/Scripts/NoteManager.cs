using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    GameObject fretboard;
    float scaleLength = 0;
    const int TOTAL_FRETS = 24;

    public GameObject noteBubblePrefab;

    void Start()
    {
        fretboard = GameObject.Find("Fretboard");

        Vector3 size = fretboard.GetComponent<MeshFilter>().mesh.bounds.size;
        scaleLength = 0;

        //Get length of fretboard- use x axis as default asset is on its side
        //Future versions will support user rotating fretboard to vertical or either horizontal position
        if(size != null)
        {
            scaleLength = size.x * fretboard.transform.localScale.x;
            Debug.Log("scaleLength = " + scaleLength.ToString());
        }
        
    
        Transform bubbles = fretboard.transform.Find("OpenStringBubbles");
        MuseNote lastNote;

        foreach(Transform openStringBubble in bubbles){
            Debug.Log("Generating child bubbles for " + openStringBubble.name);

            MuseNote parentNote = openStringBubble.GetComponent<MuseNote>();
            lastNote = parentNote;

            if(parentNote != null)
            {
                for(int i = 1; i <= TOTAL_FRETS; i++)
                {
                    GameObject childNoteGo = CreateNewNoteBubble(lastNote.getChromaticNoteAbove());

                    childNoteGo.transform.position = openStringBubble.transform.position;

                    //Using scale length and current fret number, calculate distance fret bubble must be translated
                    float fretDistance = scaleLength * MuseUtils.GetFretLocationRatio(i);
                    Debug.Log("Fret " + i + " = " + fretDistance);

                    childNoteGo.transform.Translate(new Vector3(0, -fretDistance));
                    childNoteGo.name = openStringBubble.name.Replace("Open", "Fret" + i);

                    lastNote = childNoteGo.GetComponent<MuseNote>();
                }

            }
        }

    }

    private GameObject CreateNewNoteBubble(string noteValue)
    {
        GameObject noteGo = Instantiate(noteBubblePrefab) as GameObject;
        MuseNote note = noteGo.GetComponent<MuseNote>();

        note.initNoteValue(noteValue);

        return noteGo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
