using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{

    private readonly string DEFAULT_MAIN_MENU_TAB = "Chord Builder";

    // Start is called before the first frame update
    void Start()
    {
        Transform buttons = transform.Find("MainMenuTabBar").transform;
        if (buttons != null)
        {
            Color transparentColor = new Color(0, 0, 0, 0);

            foreach (Transform keyBtn in buttons)
            {
                Renderer[] rends = keyBtn.GetComponentsInChildren<Renderer>();

                foreach (Renderer rend in rends)
                    rend.material.SetColor("_Color", transparentColor);
            }

            SelectMainMenuTab(DEFAULT_MAIN_MENU_TAB);
        }
        else
            Debug.Log("MainMenuTabBar not found");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectMainMenuTab(string tabTitle)
    {
        Debug.Log("Selecting " + tabTitle + " tab");

        GameObject tabBarGo = GameObject.Find("MainMenuTabBar");

        if (tabBarGo == null) { 
            Debug.Log("Could not find tab bar game object");
            return;
        }

        foreach(Transform child in tabBarGo.transform)
        {
            TouchableTab tab = child.GetComponent<TouchableTab>();
            TextMeshPro tmp = child.GetComponent<TextMeshPro>();

            Boolean isTab = tab.name.Equals(tabTitle);

            tab.content.SetActive(isTab);
            tmp.color = isTab ? Color.green : Color.white;
        }
    }
}
