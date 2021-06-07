using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseNote : MonoBehaviour
{
    //String representation of the note in Scientific Pitch Notation
    public string note;

    public enum NoteValue { A = 0, Bb = 1, B = 2, C = 3, Db = 4, D = 5, Eb = 6, E = 7, F = 8, Gb = 9, G = 10, Ab = 11};

    private NoteValue pitch;
    private int octave;

    // Start is called before the first frame update
    void Start()
    {
        initNoteValue(note);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initNoteValue(string newNote)
    {
        if (newNote != null && newNote.Length < 2)
            return;

        try
        {
            pitch = GetNoteValueFromString(newNote.Substring(0, newNote.Length - 1));

            String octString = newNote.Substring(newNote.Length - 1, 1);
            octave = System.Int32.Parse(octString);
        }
        catch(Exception e)
        {
            Debug.LogError("MuseNote parsing failed, note = " + newNote + " , msg= " + e.Message);
        }

        note = newNote;
    }

    //Returns string representation
    public string getChromaticNoteAbove()
    {
        NoteValue newNoteVal = pitch + 1;

        //Ab is largest value so any larger pitches should roll back to 0
        if (newNoteVal > NoteValue.Ab)
            newNoteVal = NoteValue.A;

        //if new note is a C, we are in a new octave (scientific pitch notation)
        int newNoteOctave =  
            (newNoteVal == NoteValue.C) ? octave + 1 : octave;

        string result = Enum.GetName(typeof(NoteValue) ,newNoteVal) + newNoteOctave.ToString();
        Debug.Log("Chromatic note above " + note + " is " + result);

        return result;
    }

    public NoteValue GetNoteValue()
    {
        return GetNoteValueFromString(note);
    }

    public static NoteValue GetNoteValueFromString(string input)
    {
        //TODO handle sharps and automatically convert them to flats

        return (NoteValue)Enum.Parse(typeof(NoteValue), input);
    }

    public override string ToString()
    {
        return note;
    }
}
