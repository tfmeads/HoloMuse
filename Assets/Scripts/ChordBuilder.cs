using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using TMPro;
using UnityEngine;
using static MuseNote;

public class ChordBuilder : MonoBehaviour
{

    public Material selectedMat, unselectedMat;

    private GameObject lastSelectedButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateModality(Modality newModality)
    {
        Modality modality = GetComponent<Modality>();

        modality.root = newModality.root;
        modality.chordQuality = newModality.chordQuality;
        modality.scaleType = newModality.scaleType;

        List<MuseNote.NoteValue> notes = modality.GetNotesForModality(true);

        //Populate chord buttons
        Transform chordButtons = transform.Find("ChordButtons");

        if(chordButtons != null)
        {
            int i = 0;

            foreach(Transform chordButton in chordButtons.transform)
            {
                string chordName = GetChordNameBuiltOnScaleDegree(notes, modality.scaleType, i++);
                chordButton.GetComponentInChildren<TextMeshPro>().SetText(chordName);

                //Update modality info based on chord
                Modality chordModality = chordButton.GetComponent<Modality>();

                if (chordName.EndsWith("dim"))
                {
                    chordModality.chordQuality = Modality.ChordQuality.Diminished;
                    chordModality.root = (NoteValue)Enum.Parse(typeof(NoteValue), chordName.Substring(0, chordName.Length - 3));
                }
                else if (chordName.EndsWith("m"))
                {
                    chordModality.chordQuality = Modality.ChordQuality.Minor;
                    chordModality.root = (NoteValue) Enum.Parse(typeof(NoteValue), chordName.Substring(0, chordName.Length - 1));
                }
                else {
                    chordModality.chordQuality = Modality.ChordQuality.Major;
                    chordModality.root = (NoteValue)Enum.Parse(typeof(NoteValue), chordName);
                }

                chordModality.scaleType = Modality.ScaleType.Triad;
            }
        }
    }

    public void SelectChord(GameObject button)
    {
        Modality targetModality = button.GetComponent<Modality>();

        GetComponent<KeySelectionHandler>().noteManager.UpdateModality(targetModality);

        SetButtonSelected(lastSelectedButton, false);
        SetButtonSelected(button, true);

        lastSelectedButton = button;
    }

    private void SetButtonSelected(GameObject button, Boolean selected)
    {
        if (button != null)
        {
            GameObject backPlate = button.transform.Find("BackPlate/Quad").gameObject;

            if (backPlate != null)
            {
                backPlate.GetComponent<MeshRenderer>().material = selected ? selectedMat : unselectedMat;
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
