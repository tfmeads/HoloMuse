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
        if (textMesh.text.Length > 300)
        {
            textMesh.text = message + "\n";
        }
        else
        {
            textMesh.text += message + "\n";
        }
    }
}