using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySelectionHandler : MonoBehaviour, IMixedRealityPointerHandler
{
    
    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        Vector3 clickPos = eventData.Pointer.Position;
        Vector3 distPos = transform.position - clickPos;
        Vector3 normPos = distPos.normalized;
        
        
        Debug.Log("Distance: " +
            distPos.x + "(" + normPos.x + ") ," +
            distPos.y + "(" + normPos.y + ") ," +
            distPos.z + "(" + normPos.z + ")"
            );


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
