using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseNote : MonoBehaviour
{
    [SerializeField]
    public string note;

    enum NoteValue { A = 0, Bb = 1, B = 2, C = 3, Db = 4, D = 5, Eb = 6, E = 7, F = 8, Gb = 9, G = 10, Ab = 11};

    private int value;
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

        
        String pitch = note.Substring(0, note.Length - 1);
        String oct = note.Substring(note.Length - 1, 1);

        Debug.Log(pitch + " / " + oct);

        return true;
    }
}
