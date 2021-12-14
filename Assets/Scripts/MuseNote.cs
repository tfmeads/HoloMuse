using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseNote : MonoBehaviour
{
    //String representation of the note in Scientific Pitch Notation
    public string note;

    private static readonly int TOTAL_NOTES = 12;
    public enum NoteValue { C = 0, Db = 1, D = 2, Eb = 3, E = 4, F = 5, Gb = 6, G = 7, Ab = 8, A = 9, Bb = 10, B = 11};
    public enum Interval {none = 0, m2 = 1, M2 = 2, m3 = 3, M3 = 4, P4 = 5, aug4 = 6, P5 = 7, m6 = 8, M6 = 9, m7 = 10, M7 = 11};

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

    //Returns string representation in scientific pitch notation
    public string getChromaticNoteAbove()
    {
        NoteValue newNoteVal = pitch + 1;

        //B is largest value so any larger pitches should roll back to C / 0
        if ((int) newNoteVal > (int) NoteValue.B)
            newNoteVal = NoteValue.C;

        //if new note is a C, we are in a new octave (scientific pitch notation)
        int newNoteOctave =  
            (newNoteVal == NoteValue.C) ? octave + 1 : octave;

        string result = Enum.GetName(typeof(NoteValue) ,newNoteVal) + newNoteOctave.ToString();

        return result;
    }

    public NoteValue GetNoteValue()
    {
        return pitch;
    }

    internal static NoteValue CalculateNoteValueFromInterval(NoteValue root, Interval interval)
    {
        int newPitch = ((int)root + (int)interval) % TOTAL_NOTES;

        NoteValue result = (NoteValue) newPitch;

        //Debug.Log(interval + " of " + root + " is " + result);

        return result;
    }

    public static Interval CalculateIntervalFromNoteValues(NoteValue root, NoteValue other)
    {
        int halfStepDifference = CalculateAscendingHalfStepDifference(root, other);

        Interval result = (Interval) halfStepDifference;

        Debug.Log(result);

        return result;
    }


    //Calculates half-step difference of two notes ascending-only (useful for determining interval)
    public static int CalculateAscendingHalfStepDifference(NoteValue note1, NoteValue note2)
    {
        int halfStepDifference = 0;

        if (note2 >= note1)
        {
            halfStepDifference = (int)note2 - (int)note1;
        }
        else
        {
            halfStepDifference = ((int)note2 + TOTAL_NOTES) - (int)note1;
        }

        return halfStepDifference;
    }

    //Calculates half-step difference of two notes descending-only 
    public static int CalculateDescendingHalfStepDifference(NoteValue note1, NoteValue note2)
    {
        int halfStepDifference = 0;

        if (note1 >= note2)
        {
            halfStepDifference = (int)note1 - (int)note2;
        }
        else
        {
            halfStepDifference = ((int)note1 + TOTAL_NOTES) - (int) note1;
        }

        return halfStepDifference;
    }

    //Calculates minimum half-step difference between two notes
    public static int CalculateAbsoluteHalfStepDifference(NoteValue note1, NoteValue note2)
    {
        int accDiff = CalculateAscendingHalfStepDifference(note1, note2);
        int decDiff = CalculateDescendingHalfStepDifference(note1, note2);

        int halfStepDifference = accDiff < decDiff ? accDiff : decDiff; 

        Debug.Log(note1 + " -> " + note2 + " : " + halfStepDifference);

        return halfStepDifference;
    }

    public static NoteValue GetNoteValueFromString(string input)
    {
        //TODO handle sharps and automatically convert them to flats

        return (NoteValue) Enum.Parse(typeof(NoteValue), input);
    }

    public override string ToString()
    {
        return note;
    }
}
