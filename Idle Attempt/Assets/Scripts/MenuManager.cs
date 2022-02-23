using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    //sprites


    //buttons
    public GameObject confirmButton;
    public GameObject crossButton;

    GameObject mainCam;

    private void Start()
    {
        mainCam = GameObject.Find("Main Camera");
    }

    public void ChangeMenuOptions (string type)
    {
        if(type == "null")
        {
            confirmButton.SetActive(false);
            crossButton.SetActive(false);
        }
        else if(type == "plotEmpty")
        {
            confirmButton.SetActive(true);
            crossButton.SetActive(true);
        } else if (type == "plotBuilding")
        {
            confirmButton.SetActive(false);
            crossButton.SetActive(true);
        }
    }

    public void CrossButtonClick ()
    {
        mainCam.GetComponent<TouchManager>().ForceUnselect();
    }

    public void ConfirmButtonClick ()
    {
        Debug.Log("wow");
        if (mainCam.GetComponent<TouchManager>().lastSelected.GetComponent<PlotScript>() != null)
        {
            mainCam.GetComponent<TouchManager>().lastSelected.GetComponent<PlotScript>().BuildHouse("standard");
        }
    }
}
