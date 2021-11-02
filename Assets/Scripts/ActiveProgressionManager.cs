using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveProgressionManager : MonoBehaviour
{
    public GameObject chordButtonPrefab;

    private GridObjectCollection chordGrid;

    Boolean isStarted = false;
    
    public GameObject startStopBtn;


    //Think of as top number in time signature. Currently hardcoded to 4/4
    private int beatsPerMeasure = 4;

    //Represents what beat we're currently in. incremented by metronome and reset to 1 every measure
    private int currentBeat = 1;

    //Beats per minute of metronome
    public int activeBPM = 60;
    private readonly int BPM_MINIMUM = 33;
    private readonly int BPM_MAXIMUM = 333;

    private AudioSource metronome;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void HandleStartStopButton()
    {

        if (startStopBtn == null)
        {
            Debug.Log("StartStopButton not found");
            return;
        }

        metronome = GetComponent<AudioSource>();

        if (!isStarted)
        {
            //Start metronome routine
            Debug.Log("Starting metronome");

            try
            {
                InvokeRepeating(nameof(MetronomeTick), 0, GetQuarterNoteIntervalForBPM(activeBPM));
            }
            catch(Exception e)
            {
                Debug.LogError(e.Message);
                return;
            }
        }
        else
        {
            //Stop metronome routine
            Debug.Log("Stopping metronome");
            CancelInvoke(nameof(MetronomeTick));
        }

        isStarted = !isStarted;
        startStopBtn.GetComponentInChildren<TextMeshPro>().SetText(isStarted ? "Stop" : "Start");
    }

    internal void MetronomeTick()
    {
        if (currentBeat > beatsPerMeasure)
            currentBeat = 1;

        if(currentBeat == 1)
        {
            metronome.pitch = 2;
            metronome.volume = 1;
            metronome.Play();
        }
        else
        {
            metronome.pitch = 1;
            metronome.volume = .8f;
            metronome.Play();
        }


        currentBeat++;
    }

    //returns float representing number of seconds = 1 quarter note tick at specified BPM
    internal float GetQuarterNoteIntervalForBPM(int bpm)
    {
        if(bpm < BPM_MINIMUM || bpm > BPM_MAXIMUM)
        {
            Debug.LogError(bpm + " is out of allowable BPM range.");
            return -1f;
        }
        else
        {
            return 60f / (float) bpm;
        }
    }

    internal void AddChord(Modality targetModality)
    {
        if (chordGrid == null)
            chordGrid = transform.Find("ProgressionGrid").GetComponent<GridObjectCollection>();

        Debug.Log("Add Chord " + targetModality.ToString());
        GameObject btnGo = Instantiate(chordButtonPrefab) as GameObject;
        btnGo.transform.position = chordGrid.transform.position;
        btnGo.transform.rotation = chordGrid.transform.rotation;
        btnGo.transform.SetParent(chordGrid.transform);

        Debug.Log("Button " + btnGo.name + " created");
        Modality btnModality = btnGo.GetComponent<Modality>();

        btnModality.root = targetModality.root;
        btnModality.chordQuality = targetModality.chordQuality;
        btnModality.scaleType = targetModality.scaleType;

        btnGo.GetComponentInChildren<TextMeshPro>().SetText(btnModality.ToString());

        chordGrid.UpdateCollection();
    }
}
