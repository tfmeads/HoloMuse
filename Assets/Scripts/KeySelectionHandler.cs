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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!startupKeySelected)
        {
            SelectKeyCenter("Em");
            startupKeySelected = true;
        }
    }

    public void SelectKeyCenter(string rootNoteString)
    {
        Debug.Log("Selecting key center " + rootNoteString);

        modalityOwner.SendMessage("SelectKeyCenter", rootNoteString);
    }


}
