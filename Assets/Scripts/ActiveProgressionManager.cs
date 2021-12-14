using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveProgressionManager : MonoBehaviour
{
    public GameObject chordButtonPrefab;
    public GameObject startStopBtn;
    private GridObjectCollection chordGrid;

    public MaterialLibrary matLib;

    //Is progression active, i.e is metronome running
    bool isActive = false;

    private bool startupComplete;

    //Think of as top number in time signature. Currently hardcoded to 4/4
    private int beatsPerMeasure = 4;

    //Represents what beat we're currently in. incremented by metronome and reset to 1 upon new measure
    private int currentBeat = 1;

    //Which beat to start interpolating to next modality
    private readonly int START_INTERPOLATE_BEAT = 3;

    //Beats per minute of metronome
    public int activeBPM = 60;
    private readonly int BPM_MINIMUM = 33;
    private readonly int BPM_MAXIMUM = 333;
    
    private AudioSource metronome;
    
    private NoteManager noteManager;

    //Chord currently selected during active progression
    private GameObject activeChordButton;
    private int activeChordIndex;

    private GameObject[] chordButtonList;
    private GameObject lastSelectedButton;
    private int lastSelectedButtonIndex;
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!startupComplete)
        {
            GameObject fretboard = GameObject.Find("Fretboard");
            noteManager = fretboard.GetComponent<NoteManager>();
        }
    }

    public void HandleStartStopButton()
    {

        if (startStopBtn == null)
        {
            Debug.Log("StartStopButton not found");
            return;
        }

        metronome = GetComponent<AudioSource>();

        if (!isActive)
        {
            //Start metronome routine
            Debug.Log("Starting metronome");


            BuildProgressionChordList();
           

            if(lastSelectedButton != null)
            {
                MuseUtils.SetButtonSelected(lastSelectedButton, false, matLib);
            }

            try
            {
                currentBeat = 1;

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

            //Re-select last saved user-selected button
            if (lastSelectedButton != null)
            {
                MuseUtils.SetButtonSelected(lastSelectedButton, true, matLib);
            }

            //Clear active chord status
            if(activeChordButton != null)
            {
                if(activeChordButton != lastSelectedButton)
                    MuseUtils.SetButtonSelected(activeChordButton, false, matLib);

                activeChordButton = null;
            }
        }

        isActive = !isActive;
        startStopBtn.GetComponentInChildren<TextMeshPro>().SetText(isActive ? "Stop" : "Start");
    }

    //Build array of ChordButton GameObjects so they can be easily accessed while progression is running
    private void BuildProgressionChordList()
    {
        IReadOnlyList<ObjectCollectionNode> nodes = chordGrid.NodeListReadOnly;

        chordButtonList = new GameObject[nodes.Count];

        int count = 0;

        foreach (ObjectCollectionNode node in nodes)
        {
            GameObject btn = node.Transform.gameObject;

            if (lastSelectedButton != null && lastSelectedButton == btn)
            {
                lastSelectedButtonIndex = count;
                Debug.Log("Index of selected button is " + lastSelectedButtonIndex);
            }

            chordButtonList[count++] = btn;
        }
    }

    internal void MetronomeTick()
    {
        if (currentBeat > beatsPerMeasure)
            currentBeat = 1;

        PlayTickAudio();

        //New measure, go to next chord
        if (currentBeat == 1)
        {
            activeChordIndex = GetNextChordIndex();
            SetActiveChordButton(chordButtonList[activeChordIndex]);
        }

        if(currentBeat == START_INTERPOLATE_BEAT)
        {
            Modality nextModality = chordButtonList[GetNextChordIndex()].GetComponent<Modality>();
            noteManager.InterpolateToModality(nextModality);
        }

        currentBeat++;
    }

    private int GetNextChordIndex()
    {
        //If there is no active chord, default to last selected. 
        //If last selected is null, will default to 0 (start of chord list)
        if (activeChordButton == null)
            return lastSelectedButtonIndex;

        int result = activeChordIndex + 1;

        if (result >= chordButtonList.Length)
            result = 0;

        return result;
    }

    private void PlayTickAudio()
    {
        Debug.Log("PlayTick beat= " + currentBeat + " / " + beatsPerMeasure);
        if (currentBeat == 1)
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

        btnGo.GetComponent<Interactable>().OnClick.AddListener( delegate { SelectChordButton(btnGo); });

        chordGrid.UpdateCollection();
    }

    //For when users manually select chord buttons
    internal void SelectChordButton(GameObject btnGo)
    {
        if (isActive)
            Debug.Log("Can't select Chord Buttons while progression is active.");
        else
        {
            if (lastSelectedButton == btnGo)
            {
                MuseUtils.SetButtonSelected(btnGo, false, matLib);
                lastSelectedButton = null;
            }
            else
            {
                if (lastSelectedButton != null)
                    MuseUtils.SetButtonSelected(lastSelectedButton, false, matLib);

                MuseUtils.SetButtonSelected(btnGo, true, matLib);

                Modality modality = btnGo.GetComponent<Modality>();

                if (modality != null)
                    noteManager.SetModality(modality);
                else
                    Debug.LogError("No modality found for " + btnGo);

                lastSelectedButton = btnGo;
            }
        }
    }

    //Select chord button while moving through active progression
    private void SetActiveChordButton(GameObject btnGo)
    {
        if(activeChordButton != null)
            MuseUtils.SetButtonSelected(activeChordButton, false, matLib);
        
        activeChordButton = btnGo;

        MuseUtils.SetButtonSelected(activeChordButton, true, matLib);

        Modality modality = activeChordButton.GetComponent<Modality>();

        noteManager.SetModality(modality);
    }

}

