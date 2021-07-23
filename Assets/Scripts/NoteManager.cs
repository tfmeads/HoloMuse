﻿using Microsoft.MixedReality.Toolkit;
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

    public Modality displayModality;

    void Start()
    {
        //TODO change to transform.parent if NoteManger is always component of Fretboard
        fretboard = transform.gameObject;
        displayModality = fretboard.GetComponent<Modality>();

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
 
    }

    public void KeyCenterSelected(object[] args)
    {
        MuseNote.NoteValue root = (MuseNote.NoteValue) (args[MuseUtils.ARG_ROOT_INDEX]);
        Modality.ChordQuality quality = (Modality.ChordQuality) (args[MuseUtils.ARG_QUALITY_INDEX]);

        displayModality.root = root;
        displayModality.chordQuality = quality;
        displayModality.scaleType = Modality.ScaleType.Diatonic;

        DisplayModality(displayModality);
    }

    internal void DisplayModality(Modality modality)
    {

        HideAllNoteBubbles();

        GameObject[] rootNotes = GameObject.FindGameObjectsWithTag(Enum.GetName(typeof(MuseNote.NoteValue), modality.root));

        foreach (GameObject child in rootNotes)
        {
            Renderer rend = child.GetComponent<Renderer>();
            rend.material.color = Color.red;
        }

        List<MuseNote.NoteValue> targetNotes = modality.GetNotesForModality(false);

        foreach (MuseNote.NoteValue note in targetNotes)
        {
            GameObject[] notes = GameObject.FindGameObjectsWithTag(Enum.GetName(typeof(MuseNote.NoteValue), note));

            foreach (GameObject child in notes)
            {
                Renderer rend = child.GetComponent<Renderer>();
                rend.material.color = Color.blue;
            }
        }
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
