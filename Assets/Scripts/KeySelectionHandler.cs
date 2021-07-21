﻿using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeySelectionHandler : MonoBehaviour
{

    //GameObject that should recieve modality updates
    public GameObject modalityOwner;

    Boolean startupKeySelected = false;
    private readonly string STARTUP_KEY_CENTER = "Em";

    // Start is called before the first frame update
    void Start()
    {
        Transform keyButtons = transform.Find("KeyButtons").transform;
        if (keyButtons != null)
        {
            Color transparentColor = new Color(0, 0, 0, 0);

            foreach (Transform keyBtn in keyButtons)
            {
                Renderer[] rends = keyBtn.GetComponentsInChildren<Renderer>();

                foreach(Renderer rend in rends)
                    rend.material.SetColor("_Color", transparentColor);
            }
        }
        else
            Debug.Log("KeySelectionSlate not found");

    }

    // Update is called once per frame
    void Update()
    {
        if (!startupKeySelected)
        {
            SelectKeyCenter(STARTUP_KEY_CENTER);
            startupKeySelected = true;
        }
    }

    public void SelectKeyCenter(string rootNoteString)
    {
        Debug.Log("Selecting key center " + rootNoteString);

        Transform title = transform.Find("SelectedKey");

        if (title != null)
        {
            TextMeshPro titleTv = title.gameObject.GetComponent<TextMeshPro>();

            titleTv.SetText(rootNoteString);
        }
        else
            Debug.Log("Could not find title for KeySelectionSlate");

        MuseNote.NoteValue root;
        Modality.ChordQuality quality;
        String keyName = rootNoteString;

        if (rootNoteString.EndsWith("m"))
        {
            quality = Modality.ChordQuality.Minor;
            keyName = rootNoteString.Substring(0, rootNoteString.Length - 1);
        }
        else
            quality = Modality.ChordQuality.Major;

        root = MuseNote.GetNoteValueFromString(keyName);

        object[] tempStorage = new object[2];
        tempStorage[MuseUtils.ARG_ROOT_INDEX] = root;
        tempStorage[MuseUtils.ARG_QUALITY_INDEX] = quality;

        Debug.Log("root = " + root.ToString());
        Debug.Log("quality = " + quality.ToString());

        //Send events to fretboard and to child components (Chord Builder)
        modalityOwner.SendMessage("KeyCenterSelected", tempStorage);
        SendMessage("KeyCenterSelected", tempStorage);
    }


}
