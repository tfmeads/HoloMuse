using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modality : MonoBehaviour
{
    public enum ChordQuality { Major = 0, Minor = 1, Dominant = 2, Diminished = 3, Augmented = 4 };
    public enum ScaleType { RootOnly = 0, Triad = 1, Seventh = 2, Pentatonic = 3, Diatonic = 4};


    public MuseNote.NoteValue root;
    public ChordQuality chordQuality;
    public ScaleType scaleType;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
