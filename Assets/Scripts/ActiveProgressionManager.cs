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

        if (startStopBtn != null)
        {
            isStarted = !isStarted;
            startStopBtn.GetComponentInChildren<TextMeshPro>().SetText(isStarted ? "Stop" : "Start");
        }
        else
        {
            Debug.Log("StartStopButton not found");
            return;
        }

        if (!isStarted)
        {
            //Start metronome routine
            Debug.Log("Starting metronome");
        }
        else
        {
            //Stop metronome routine
            Debug.Log("Stopping metronome");
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
