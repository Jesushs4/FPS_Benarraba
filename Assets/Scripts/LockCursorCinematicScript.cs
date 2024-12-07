using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LockCursorCinematicScript : MonoBehaviour
{
    //Start is called before the first frame update
    void Start()
    {
        //Unlock the cursor
        Cursor.lockState = CursorLockMode.None;
    }
}
