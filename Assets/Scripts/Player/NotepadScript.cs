using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class NotepadScript : MonoBehaviour
{
    //References
    public PlayerController playerController;
    public TimeController timeController;
    private ButtonManager buttonManagerScript;
    public MapScript mapScript;
    public GameObject notepadContent;
    public GameObject hintsListObject;
    public GameObject notePad;
    public GameObject hintTaskPrefab;
    public PlayerSaveData saveDataPreset;

    //Variables
    private bool isOpen = false;

    //Variables
    public bool notepadIsOpen = false;


    //Start
    private void Start()
    {
        buttonManagerScript = GameObject.FindObjectOfType<ButtonManager>();
        //Hide the notepad
        notepadContent.SetActive(false);
    }


    //Try to add a hint if it doesn't exist previously
    public void TryAddHint(string hintText)
    {
        //Save the hint
        saveDataPreset.SaveData(hintText);

        //Save the rest of the player things
        ButtonManager.buttonManager.OnBtSaving();
    }


    //Update the notepad
    private void UpdateNotepad()
    {
        //Destroy all the previous notes
        foreach (Transform child in hintsListObject.transform)
        {
            Destroy(child.gameObject);
        }

        //Add a gameobject for each hint
        foreach (string hint in saveDataPreset.seenHints)
        {
            //Instantiate inside the notepad content
            GameObject tempHintObject = Instantiate(hintTaskPrefab, hintsListObject.transform, false);
            tempHintObject.GetComponent<HintTextScript>().SetHintText(hint);
        }

    }


    //Open or close the notepad, depending on it's current state
    private void OpenCloseNotepad()
    {
        if (!buttonManagerScript.menu && !mapScript.mapIsOpen && !timeController.transition && !playerController.notepadLocked)
        {
            //The opposite of the current state
            notepadIsOpen = !notepadIsOpen;
            notepadContent.SetActive(notepadIsOpen);

            //Lock or unlock the cursor and stop / unstop the time
            if (notepadIsOpen)
            {
                notePad.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                UpdateNotepad();
                Time.timeScale = 0f;
                //ButtonManager.buttonManager.menu = true;
                InteractionManager.interactionManager.isOnConversation = true;
                
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
                //ButtonManager.buttonManager.menu = false;
                InteractionManager.interactionManager.isOnConversation = false;
                //buttonManagerScript.menu = false;
                notePad.SetActive(false);
            }
        }
        

    }


    //Event => Open / Close the notepad when the C key is pressed
    public void OnNotepadKey(InputAction.CallbackContext context)
    {
        //Is not necessary to read the input context, just open / close the notepad
        OpenCloseNotepad();
    }

}
