using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    GameObject fretboard;
    public GameObject noteBubblePrefab;
    float scaleLength = 0;
    const int TOTAL_FRETS = 24;

    public Modality keyModality;

    //Placeholder til modal UI is developed
    private bool modalityDisplayed = false;

    void Start()
    {
        fretboard = GameObject.Find("Fretboard");
        Vector3 size = fretboard.GetComponent<MeshFilter>().mesh.bounds.size;

        //Get length of fretboard- use x axis as default asset is on its side
        //Future versions will support user rotating fretboard to vertical or either horizontal position
        if(size != null)
            scaleLength = size.x * fretboard.transform.localScale.x;
        
    
        Transform bubbles = fretboard.transform.Find("OpenStringBubbles");
        MuseNote lastNote;

        foreach(Transform openStringBubble in bubbles){

            MuseNote parentNote = openStringBubble.GetComponent<MuseNote>();
            lastNote = parentNote;

            if(parentNote != null)
            {
                for(int i = 1; i <= TOTAL_FRETS; i++)
                {
                    GameObject childNoteGo = CreateNewNoteBubble(lastNote.getChromaticNoteAbove());

                    childNoteGo.transform.position = openStringBubble.transform.position;

                    childNoteGo.transform.SetParent(openStringBubble, true);

                    //Using scale length and current fret number, calculate distance fret bubble must be translated
                    float fretDistance = scaleLength * MuseUtils.GetFretLocationRatio(i);

                    childNoteGo.transform.Translate(new Vector3(0, -fretDistance));
                    childNoteGo.name = openStringBubble.name.Replace("Open", "Fret" + i);

                    lastNote = childNoteGo.GetComponent<MuseNote>();

                    //Add tag indicating pitch value so it can easily be found later
                    childNoteGo.tag = Enum.GetName(typeof(MuseNote.NoteValue), lastNote.GetNoteValue());
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
        if(keyModality != null && !modalityDisplayed)
        {
            DisplayModality(keyModality);
        }
    }

    private void DisplayModality(Modality modality)
    {
        HideAllNoteBubbles();

        GameObject[] rootNotes = GameObject.FindGameObjectsWithTag(Enum.GetName(typeof(MuseNote.NoteValue), modality.root));

        foreach (GameObject child in rootNotes)
        {
            Debug.Log("Found root note " + child.name);
            Renderer rend = child.GetComponent<Renderer>();

            rend.material.color = Color.red;
        }

        List<MuseNote.NoteValue> targetNotes = modality.GetNotesForModality();


        foreach (MuseNote.NoteValue note in targetNotes)
        {
            GameObject[] notes = GameObject.FindGameObjectsWithTag(Enum.GetName(typeof(MuseNote.NoteValue), note));

            foreach (GameObject child in notes)
            {
                Debug.Log("Found target note " + child.name);
                Renderer rend = child.GetComponent<Renderer>();

                rend.material.color = Color.blue;
            }
        }


        modalityDisplayed = true;
    }

    private void HideAllNoteBubbles()
    {
        MuseNote[] notes = GameObject.FindObjectsOfType<MuseNote>();

        Color transparentColor = new Color(0, 0, 0, 0);

        foreach (MuseNote note in notes)
        {
            Renderer rend = note.GetComponent<Renderer>();
            rend.material.SetColor("_Color", transparentColor);
        }
    }
}
