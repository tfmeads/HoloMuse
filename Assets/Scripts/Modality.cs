using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MuseNote.Interval;
using static Modality.ChordQuality;
using static Modality.ScaleType;

public class Modality : MonoBehaviour
{
    public enum ChordQuality { Major = 0, Minor = 1, Dominant = 2, Diminished = 3, Augmented = 4 };
    public enum ScaleType { RootOnly = 0, Triad = 1, Seventh = 2, Pentatonic = 3, Diatonic = 4 };

    private readonly MuseNote.Interval[] INTERVAL_DEF_MAJ = { M2, M3, P4, P5, M6, M7 };
    private readonly MuseNote.Interval[] INTERVAL_DEF_MIN = { M2, m3, P4, P5, m6, m7 };
    private readonly MuseNote.Interval[] INTERVAL_DEF_DOM = { M2, M2, P4, P5, M6, m7 };
    private readonly MuseNote.Interval[] INTERVAL_DEF_DIM = { m2, m3, P4, aug4, m6, M6 };
    private readonly MuseNote.Interval[] INTERVAL_DEF_AUG = { m3, M3, P5, m6, none, M7 };

    //Human friendly representations for interval def indexes 
    private enum IntervalClass { Second = 0, Third = 1, Fourth = 2, Fifth = 3, Sixth = 4, Seventh = 5 }

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

    //Returns NoteValues representing valid tones in this modality, excluding root.
    internal List<MuseNote.NoteValue> GetNotesForModality()
    {
        List<MuseNote.NoteValue> results = new List<MuseNote.NoteValue>();
        MuseNote.Interval [] intervalDef = GetIntervalDef();

        if (scaleType == RootOnly)
            return results;

        if (scaleType == Diatonic)
        {
            //Add every interval possible
            foreach (IntervalClass ic in Enum.GetValues(typeof(IntervalClass)))
            {
                results.Add(GetIntervalNoteValue(ic, intervalDef));
            }
        }

        if (scaleType == Pentatonic)
        {
            if (chordQuality == Major)
            {
                results.Add(GetIntervalNoteValue(IntervalClass.Second, intervalDef));
                results.Add(GetIntervalNoteValue(IntervalClass.Third, intervalDef));
                results.Add(GetIntervalNoteValue(IntervalClass.Fifth, intervalDef));
                results.Add(GetIntervalNoteValue(IntervalClass.Sixth, intervalDef));
            }

            if (chordQuality == Minor)
            {
                results.Add(GetIntervalNoteValue(IntervalClass.Third, intervalDef));
                results.Add(GetIntervalNoteValue(IntervalClass.Fourth, intervalDef));
                results.Add(GetIntervalNoteValue(IntervalClass.Fifth, intervalDef));
                results.Add(GetIntervalNoteValue(IntervalClass.Seventh, intervalDef));
            }

            //TODO figure out how to handle dim/aug pentatonics
        }

        if (scaleType == Seventh)
        {
            results.Add(GetIntervalNoteValue(IntervalClass.Third, intervalDef));
            results.Add(GetIntervalNoteValue(IntervalClass.Fifth, intervalDef));
            results.Add(GetIntervalNoteValue(IntervalClass.Seventh, intervalDef));
        }

        if (scaleType == Triad)
        {
            results.Add(GetIntervalNoteValue(IntervalClass.Third, intervalDef));
            results.Add(GetIntervalNoteValue(IntervalClass.Fifth, intervalDef));
        }

        return results;
    }

    private MuseNote.NoteValue GetIntervalNoteValue(IntervalClass intervalClass, MuseNote.Interval[] intervalDef)
    {
        MuseNote.Interval interval = intervalDef[(int) intervalClass];

        MuseNote.NoteValue result = MuseNote.CalculateInterval(root, interval);

        return result;
     }

    private MuseNote.Interval[] GetIntervalDef()
    {
        switch (chordQuality)
        {
            case Major:
                return INTERVAL_DEF_MAJ;
            case Minor:
                return INTERVAL_DEF_MIN;
            case Dominant:
                return INTERVAL_DEF_DOM;
            case Augmented:
                return INTERVAL_DEF_AUG;
            case Diminished:
                return INTERVAL_DEF_DIM;

        }

        return null;
    }

    public override bool Equals(object other)
    {
        Modality otherModality = (Modality) other;
        Boolean result;

        if (!root.Equals(otherModality.root))
            result =  false;

        if (!chordQuality.Equals(otherModality.chordQuality))
            result =  false;

        if (!scaleType.Equals(otherModality.scaleType))
            result =  false;

        result =  true;

        return result;
    }

    public override int GetHashCode()
    {
        return root.GetHashCode() + (int)chordQuality + (int)scaleType;
    }

}
