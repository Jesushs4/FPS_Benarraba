using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WayPointScript : MonoBehaviour
{

    //References
    public GameObject associatedWaypoint;
    private MapScript mapScript;


    //Start
    private void Start()
    {
        //Find the map script, there will be only one
        mapScript = GameObject.FindObjectOfType<MapScript>();
    }


    //Event => Select and show a waypoint in the world
    public void OnClickSelectWaypoint()
    {
        mapScript.ShowWaypoint(associatedWaypoint);
    }
}
