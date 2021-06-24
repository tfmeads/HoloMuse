using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections;
using System.Collections.Generic;
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

        modalityOwner.SendMessage("SelectKeyCenter", rootNoteString);
    }


}
