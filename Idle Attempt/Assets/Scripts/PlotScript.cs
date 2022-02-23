using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotScript : MonoBehaviour
{
    public Material regularMat;
    public Material selectedMat;
    GameObject es;
    string buildingOccupied = "none";

    //building spawning
    public GameObject standardBuilding;
    Vector3 locationSpawn = new Vector3(0f, 0.5f, 0f);
    Vector3 localScaleSpawn = new Vector3(0.5f, 4f, 0.5f);

    // Start is called before the first frame update
    void Start()
    {
        es = GameObject.Find("EventSystem");
    }

    public void Select ()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = selectedMat;
        if(buildingOccupied == "none")
        {
            es.GetComponent<MenuManager>().ChangeMenuOptions("plotEmpty");
        } /*else if ()
        {

        }*/
        
    }

    public void Unselect ()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = regularMat;
        es.GetComponent<MenuManager>().ChangeMenuOptions("null");
    }

    public void BuildHouse (string type)
    {
        if (buildingOccupied == "none")
        {
            if (type == "standard")
            {
                GameObject childBuilding = Instantiate(standardBuilding);
                childBuilding.transform.parent = this.transform;
                childBuilding.transform.localPosition = locationSpawn;
                childBuilding.transform.localScale = localScaleSpawn;
                buildingOccupied = "standard";
                es.GetComponent<MenuManager>().ChangeMenuOptions("plotBuilding");
            }
        }
    } 
}
