using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseNote : MonoBehaviour
{
    [SerializeField]
    public string note;

    public enum NoteValue { A = 0, Bb = 1, B = 2, C = 3, Db = 4, D = 5, Eb = 6, E = 7, F = 8, Gb = 9, G = 10, Ab = 11};

    private string pitch;
    private int octave;

    // Start is called before the first frame update
    void Start()
    {
        if (isValidNoteString(note))
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool isValidNoteString(string note)
    {
        if (note != null && note.Length < 2)
            return false;


        try
        {
            pitch = note.Substring(0, note.Length - 1);

            String octString = note.Substring(note.Length - 1, 1);
            octave = System.Int32.Parse(octString);
        }
        catch(Exception e)
        {
            Debug.Log("MuseNote parsing failed, " + e.Message);
            return false;
        }

        return true;
    }

    public NoteValue GetNoteValue()
    {
        return (NoteValue) Enum.Parse(typeof(NoteValue), pitch);
    }

    public override string ToString()
    {
        return note;
    }
}
