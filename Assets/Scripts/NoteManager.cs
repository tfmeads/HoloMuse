using Microsoft.MixedReality.Toolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MuseNote;

public class NoteManager : MonoBehaviour
{

    GameObject fretboard;
    public GameObject noteBubblePrefab, ghostBubblePrefab;
    float scaleLength = 0;
    public const int TOTAL_FRETS = 24;
    public Modality displayModality;

    void Start()
    {
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
                    childNoteGo.tag = Enum.GetName(typeof(NoteValue), lastNote.GetNoteValue());
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

    public void SetModality(Modality newModality)
    {
        displayModality.root = newModality.root;
        displayModality.chordQuality = newModality.chordQuality;
        displayModality.scaleType = newModality.scaleType;

        DisplayModality(displayModality);
    }

    private void DisplayModality(Modality modality)
    {

        HideAllNoteBubbles();

        GameObject[] rootNotes = GameObject.FindGameObjectsWithTag(Enum.GetName(typeof(NoteValue), modality.root));

        foreach (GameObject child in rootNotes)
        {
            Renderer rend = child.GetComponent<Renderer>();
            rend.material.color = Color.red;
        }

        List<NoteValue> targetNotes = modality.GetNotesForModality(false);

        foreach (NoteValue note in targetNotes)
        {
            GameObject[] notes = GameObject.FindGameObjectsWithTag(Enum.GetName(typeof(NoteValue), note));

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

    
    internal void InterpolateToModality(Modality modality, float lerpTime)
    {
        List<NoteValue> currentNotes = displayModality.GetNotesForModality(true);
        List<NoteValue> newNotes = modality.GetNotesForModality(true);

        List<NoteValue> interpolateNotes = new List<NoteValue>(); 

        foreach(NoteValue note in currentNotes)
        {
            //Holds smallest difference in half-steps found between current notes and new notes
            int smallestDiff = 99999;

            foreach(NoteValue otherNote in newNotes)
            {
                int halfStepDiff = CalculateAbsoluteHalfStepDifference(note, otherNote);
                
                if(halfStepDiff < smallestDiff)
                {
                    smallestDiff = halfStepDiff;
                    interpolateNotes.Clear();
                }

                if (halfStepDiff == smallestDiff)
                    interpolateNotes.Add(otherNote);
            }

            InterpolateNote(note, interpolateNotes, lerpTime, smallestDiff);
        }
    }

    //Create translucent bubble that moves from source note to target notes
    private void InterpolateNote(NoteValue note, List<NoteValue> interpolateNotes, float lerpTime, int halfStepDifference)
    {

        GameObject[] rootNotes = GameObject.FindGameObjectsWithTag(Enum.GetName(typeof(NoteValue), note));

        foreach (GameObject noteBubble in rootNotes)
        {
            foreach(NoteValue otherNote in interpolateNotes)
            {
                Debug.Log("Interpolating " + note + " to " + otherNote);
                GameObject ghostNote = Instantiate(ghostBubblePrefab) as GameObject;

                ghostNote.transform.position = noteBubble.transform.position;

                ghostNote.transform.SetParent(noteBubble.transform, true);

                GameObject nearestTargetBubble = GetNearestTargetBubble(noteBubble, otherNote, halfStepDifference);

                if (nearestTargetBubble != null)
                    ghostNote.GetComponent<GhostNote>().StartLerp(noteBubble, nearestTargetBubble, lerpTime, Color.green);
                else
                    Debug.Log("No suitable target bubble found");
            }
        }
    }

    //Find target Note bubble that is closest to given  note
    private GameObject GetNearestTargetBubble(GameObject noteBubble, NoteValue otherNote, int halfStepDifference)
    {
        Debug.Log("GetNearestTargetBubble(" + otherNote + ")");

        Transform parent = noteBubble.transform.parent.transform;
        int index = noteBubble.transform.GetSiblingIndex();

        //First check if parent (Open Note bubble) is valid
        if(parent.gameObject.GetComponent<MuseNote>().GetNoteValue() == otherNote)
        {
            return parent.gameObject;
        }

        //Check if either sibling offset by given half step difference are valid. Note bubbles are added in ascending order which makes this possible
        if(parent.childCount > (index + halfStepDifference))
        {
            GameObject otherBubble = parent.GetChild(index + halfStepDifference).gameObject;

            if (otherBubble != null && otherBubble.GetComponent<MuseNote>().GetNoteValue() == otherNote)
                return otherBubble;
        }

        if((index - halfStepDifference) >= 0)
        {
            GameObject otherBubble = parent.GetChild(index - halfStepDifference).gameObject;

            if (otherBubble != null && otherBubble.GetComponent<MuseNote>().GetNoteValue() == otherNote)
                return otherBubble;
        }

        return null;
    }
}
