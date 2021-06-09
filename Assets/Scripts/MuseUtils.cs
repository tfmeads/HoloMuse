using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseUtils : MonoBehaviour
{

    //Holds the y position of each fret on the fret texture expressed as a ratio of the total length
    private static float[] fretLocations = {
        .078f, 
        .147f,
        .213f, //Fret 3
        .276f,
        .333f,
        .391f,
        .441f, //Fret 7
        .49f,
        .535f,
        .581f,
        .619f,
        .658f,  //Fret 12
        .694f,
        .730f,
        .763f,
        .79f,
        .82f, //Fret 17
        .848f,
        .872f,
        .895f,   //Fret 20
        .921f,
        .943f,
        .962f,
        .983f  //Fret 24
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Convenience function for accessing fretLocations
    internal static float GetFretLocationRatio(int fret)
    {
        //Index 0 = Fret #1, first non open note
        int index = fret - 1;

        if (index > fretLocations.Length)
            return 0f;

        else return fretLocations[index];
    }
}
