using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using TMPro;
using UnityEngine;

public class ChordBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KeyCenterSelected(object[] args)
    {
        MuseNote.NoteValue root = (MuseNote.NoteValue)(args[MuseUtils.ARG_ROOT_INDEX]);
        Modality.ChordQuality quality = (Modality.ChordQuality)(args[MuseUtils.ARG_QUALITY_INDEX]);

        Modality keyModality = GetComponent<KeySelectionHandler>().modalityOwner.GetComponent<Modality>();

        //Populate chord buttons

        Transform chordButtons = transform.Find("ChordButtons");

        if(chordButtons != null)
        {
            int i = 0;

            foreach(Transform chord in chordButtons.transform)
            {
                string chordName = GetChordNameBuiltOnScaleDegree(keyModality, i++);
                chord.GetComponentInChildren<TextMeshPro>().SetText(chordName);
            }
        }
    }

    private string GetChordNameBuiltOnScaleDegree(Modality keyModality, int rootScaleDegree)
    {
        //TODO generating this list every time may cost too much performance
        List<MuseNote.NoteValue> notes = keyModality.GetNotesForModality();

        MuseNote.NoteValue rootNote = notes[rootScaleDegree];
        string result = rootNote.ToString();

        Debug.Log("building chord based on " + rootNote);

        if(keyModality.scaleType >= Modality.ScaleType.Triad)
        {
            MuseNote.NoteValue third = notes[(rootScaleDegree + 2) % notes.Count];
            Debug.Log("third = " + third);
            MuseNote.Interval thirdInterval = MuseNote.CalculateIntervalFromNoteValues(rootNote, third);
            result += (thirdInterval == MuseNote.Interval.m3 ? "m" : "");
        }

        

        return result;
    }
}
