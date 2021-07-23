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
        Modality modality = GetComponent<Modality>();

        List<MuseNote.NoteValue> notes = modality.GetNotesForModality(true);

        String noteStr = "";

        foreach(MuseNote.NoteValue n in notes)
            noteStr += n.ToString() + ",";

        Debug.Log(root.ToString() + " " + quality.ToString() + ": " + noteStr);

        //Populate chord buttons
        Transform chordButtons = transform.Find("ChordButtons");

        if(chordButtons != null)
        {
            int i = 0;

            foreach(Transform chord in chordButtons.transform)
            {
                string chordName = GetChordNameBuiltOnScaleDegree(notes, modality.scaleType, i++);
                chord.GetComponentInChildren<TextMeshPro>().SetText(chordName);
            }
        }
    }

    private string GetChordNameBuiltOnScaleDegree(List<MuseNote.NoteValue> notes, Modality.ScaleType type, int rootScaleDegree)
    {
       
        MuseNote.NoteValue rootNote = notes[rootScaleDegree];
        string result = rootNote.ToString();

        if(type >= Modality.ScaleType.Triad)
        {
            MuseNote.NoteValue third = notes[(rootScaleDegree + 2) % notes.Count];
            MuseNote.Interval thirdInterval = MuseNote.CalculateIntervalFromNoteValues(rootNote, third);

            MuseNote.NoteValue fifth = notes[(rootScaleDegree + 4) % notes.Count];
            MuseNote.Interval fifthInterval = MuseNote.CalculateIntervalFromNoteValues(rootNote, fifth);

            if(thirdInterval == MuseNote.Interval.m3)
                result += (fifthInterval == MuseNote.Interval.aug4) ? "dim" : "m";
        }

        

        return result;
    }
}
