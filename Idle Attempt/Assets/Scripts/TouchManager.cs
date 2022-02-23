using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//credit for dragging world
//https://www.youtube.com/watch?v=qbl38iPitVY
//https://zerobin.net/?38740e79581aeb37#b8JPDn2BYAwZqkbZlx+VgVe4pwE1g+06f+sKtOO+AdY=

//credit for zooming world
//https://www.codegrepper.com/code-examples/csharp/camera+pinch+zoom+unity

public class TouchManager : MonoBehaviour
{
    bool tap = false;
    bool drag = false;
    bool isZooming = false;

    public GameObject lastSelected;
    float timer = 0.0f;
    public float tapInterval = 0.2f;

    //cam drag
    Vector2 startPosition;
    public float camAltitude = 9.5f;

    //cam zoom
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;

    // Start is called before the first frame update
    void Start()
    {
        lastSelected = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //prime and check timers when finger touches screen
        timer += Time.deltaTime;

        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            timer = 0;
        }

        //check drags which will be used for confirmation later
        if((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            //end timer
            if (timer <= tapInterval)
            {
                drag = false;
                tap = false;
            }
            else
            {
                drag = true;
                tap = false;
            }
        }

        //check taps which will be used in regular detection later
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            //end timer
            if (timer <= tapInterval)
            {
                tap = true;
                drag = false;
            }
            else
            {
                tap = false;
                drag = true;
            }
        }

        //reset if no fingers are on the svreen
        if(Input.touchCount == 0)
        {
            tap = false;
            drag = false;
            isZooming = false;
        }

        //handle tapping
        if (tap == true)
        {
            //get what we tapped
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;

            //if we tapped an actual object
            if (Physics.Raycast(raycast, out raycastHit))
            {
                //check the tag of what we hit
                if (raycastHit.collider.tag == "Plot")
                {
                    //unselect
                        //plot
                    if (lastSelected.GetComponent<PlotScript>() != null)
                    {
                        lastSelected.GetComponent<PlotScript>().Unselect();
                    }

                    //select
                        //plot
                    if (raycastHit.collider.gameObject.GetComponent<PlotScript>() != null)
                    {
                        raycastHit.collider.gameObject.GetComponent<PlotScript>().Select();
                    }

                    //update last selected
                    lastSelected = raycastHit.collider.gameObject;
                }
                //if we tap a non-specified tag treat it like a background tap
                else if(!EventSystem.current.IsPointerOverGameObject())
                {
                    //unselect
                    ForceUnselect();
                }
            }
        }

        //handle dragging
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                //position of finger
                Vector2 NewPosition = GetWorldPosition();

                //distance between finger position and camera position
                Vector2 PositionDifference = NewPosition - startPosition;

                //confirm drag before movement
                if (drag == true)
                {
                    //move camera
                    Camera.main.transform.Translate(-PositionDifference);
                }
            }
            //update start position
            startPosition = GetWorldPosition();

            //fix height
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 9.5f, Camera.main.transform.position.z);
        }

        //handle zooming
        if (Input.touchCount == 2)
        {
            //get each touch for reference
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            //get previous positions of each touch
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            //get the distance between the fingers
            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            //get the new distance between the fingers
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            //find the difference between those
            float difference = currentMagnitude - prevMagnitude;

            //zoom in based on the difference
            zoom(difference * 0.01f);
        }
    }

    //drag functions
    Vector2 GetWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
    }

    //zoom functions
    void zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }

    //other
    public void ForceUnselect ()
    {
        //unselect
        //plot
        if (lastSelected.GetComponent<PlotScript>() != null)
        {
            lastSelected.GetComponent<PlotScript>().Unselect();
        }

        //select this
        lastSelected = this.gameObject;
    }
}
