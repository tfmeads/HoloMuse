using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugWindow : MonoBehaviour
{
    //Holds actual log messages to be displayed to user
    public TextMeshPro contentText;

    public TextMeshPro titleText;

    public GameObject pageUpButton, pageDownButton;

    // Start is called before the first frame update
    void Start()
    {

        pageUpButton.GetComponent<Interactable>().OnClick.AddListener(delegate { HandlePageUp(); });
        pageDownButton.GetComponent<Interactable>().OnClick.AddListener(delegate { HandlePageDown(); });

        
    }

    private void Update()
    {
        int totalPages = contentText.textInfo.pageCount;
        int currPage = contentText.pageToDisplay;

        titleText.SetText("Debug (" + currPage + 1 + "/" + totalPages + ")");
    }

    private void HandlePageDown()
    {
        int currentPage = contentText.pageToDisplay;

        if (currentPage < contentText.textInfo.pageCount)
            contentText.pageToDisplay++;
    }

    private void HandlePageUp()
    {
        int currentPage = contentText.pageToDisplay;

        if (currentPage > 0)
            contentText.pageToDisplay--;
    }

    void OnEnable()
    {
        Application.logMessageReceived += LogMessage;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= LogMessage;
    }

    public void LogMessage(string message, string stackTrace, LogType type)
    {
        contentText.text += message + "\n";

        //Set page to last page available to auto-scroll
        int numPages = contentText.textInfo.pageCount;
        contentText.pageToDisplay = numPages;


    }
}