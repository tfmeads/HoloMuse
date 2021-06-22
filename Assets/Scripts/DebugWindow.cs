using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugWindow : MonoBehaviour
{
    public TextMeshPro textMesh;
    // Start is called before the first frame update
    void Start()
    {
     
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
        textMesh.text += message + "\n";

        //Set page to last page available to auto-scroll
        int numPages = textMesh.textInfo.pageCount;
        textMesh.pageToDisplay = numPages;
    }
}