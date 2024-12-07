using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MapScript : MonoBehaviour
{
    //References
    public TimeController timeController;
    public NotepadScript notepadScript;
    public GameObject mapContent;
    private GameObject[] waypointsList;
    private ButtonManager buttonManagerScript;

    //Variables
    public bool mapIsOpen = false;


    //Start
    private void Start()
    {

        //Get the button manager's script
        buttonManagerScript = GameObject.FindObjectOfType<ButtonManager>();

        //Hide the map
        mapContent.SetActive(false);

        //Hide all the waypoints by default
        waypointsList = GameObject.FindGameObjectsWithTag("Waypoint");
        HideAllWaypoints();
    }


    //Open or close the map, depending on it's current state
    private void OpenCloseMap()
    {

        if (!buttonManagerScript.menu && !notepadScript.notepadIsOpen && !timeController.transition)
        {
            mapIsOpen = !mapIsOpen;
            mapContent.SetActive(mapIsOpen);
            
            if (mapIsOpen)
            {
                InteractionManager.interactionManager.isOnConversation = true;
                
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
                //buttonManagerScript.menu = true;
            }
            else
            {
                InteractionManager.interactionManager.isOnConversation = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
                //buttonManagerScript.menu = false;
            }
        }
        else
        {
            Debug.Log("No puedes hacer esto aqui");
        }
        


    }


    //Event => Open / Close the map when the V key is pressed
    public void OnMapKey(InputAction.CallbackContext context)
    {
        //Is not necessary to read the input context, just open / close the map
        OpenCloseMap();
    }


    //Hide all the waypoints
    private void HideAllWaypoints()
    {
        foreach (GameObject waypoint in waypointsList) waypoint.SetActive(false);
    }


    //Show or hide a waypoint
    public void ShowWaypoint(GameObject waypoint)
    {
        HideAllWaypoints();

        //Hide all the waypoints except the one selected
        foreach (GameObject worldPoint in waypointsList)
        {
            if (worldPoint == waypoint) waypoint.SetActive(true);
        }
    }

}
