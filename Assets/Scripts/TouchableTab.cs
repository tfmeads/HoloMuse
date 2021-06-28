using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchableTab : MonoBehaviour, IMixedRealityPointerHandler
{

    public string TabTitle;

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        Debug.Log(TabTitle + " tab clicked!!!");
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
