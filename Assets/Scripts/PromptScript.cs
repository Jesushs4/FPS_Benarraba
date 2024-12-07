using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptScript : MonoBehaviour
{
    public GameObject promptContent;
    public GameObject promptContentSecondary;
    private Camera playerCamera;


    //Start
    void Start()
    {
        playerCamera = Camera.main;
        SetActive(false);
    }


    //Update
    void Update()
    {
        transform.LookAt(playerCamera.transform.position);
        transform.Rotate(0, 180, 0);
    }


    //Set active or inactive
    public void SetActive(bool state)
    {
        promptContent.SetActive(state);

        if (promptContentSecondary != null) promptContentSecondary.SetActive(state);
    }
}
