using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerController : MonoBehaviour
{

    public GameObject canvas;
    InteractionManager interactionManager;

    public bool notepadLocked = false;
    public bool conversation = false;
    public bool canClickNPC = true;
    public bool testTrueOrFalse;

    public GameObject target;
    public GameObject padlock;
    public GameObject teclaNotepad;


    public GameObject[] hiddenBuildings;
    public GameObject[] hiddenProps;
    private int countBuildings = 100;
    private int countProp = 100;


    private void Start()
    {
        interactionManager = GetComponent<InteractionManager>();

        SearchHidden("Building", hiddenBuildings);
        SearchHidden("Props", hiddenProps);

        for (int i = 1; i < 5; i++)
        {
            Invoke(nameof(ShowBuilding), i);
            Invoke(nameof(ShowProp), i);
        }
    }

    private void SearchHidden(string target, GameObject[] targetObject)
    {
        GameObject[] allBuildings = GameObject.FindObjectsOfType<GameObject>(true).Where(x => x.CompareTag(target)).ToArray();
        targetObject = allBuildings;

        for (int i = 0; i < allBuildings.Length; i++)
        {
            if (Vector3.Distance(targetObject[i].transform.position, transform.position) < 100)
            {
                for (int p = 0; p < targetObject[i].transform.childCount; p++)
                {
                    //Debug.Log(targetObject[i].transform.GetChild(p).name);
                    if (targetObject[i].transform.GetChild(p).GetComponentInChildren<MeshRenderer>())
                        targetObject[i].transform.GetChild(p).GetComponentInChildren<MeshRenderer>().enabled = true;
                }
                if (targetObject[i].GetComponent<MeshRenderer>())
                    targetObject[i].GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }




    private void ShowBuilding()
    {
        countBuildings +=30;

        for (int i = 0; i < hiddenBuildings.Length; i++)
        {
            if (!hiddenBuildings[i].activeInHierarchy &&
            Vector3.Distance(hiddenBuildings[i].transform.position, transform.position) < countBuildings)
            {
                for (int p = 0; p < hiddenBuildings[i].transform.childCount; p++)
                {
                    Debug.Log(hiddenBuildings[i].transform.GetChild(p).name);
                    hiddenBuildings[i].transform.GetChild(p).GetComponentInChildren<MeshRenderer>().enabled = true;
                }
                if (hiddenBuildings[i].GetComponent<MeshRenderer>())
                    hiddenBuildings[i].GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    private void ShowProp()
    {
        countProp += 30;

        for (int i = 0; i < hiddenProps.Length; i++)
        {
            if (!hiddenProps[i].activeInHierarchy &&
            Vector3.Distance(hiddenProps[i].transform.position, transform.position) < countBuildings)
            {
                for (int p = 0; p < hiddenProps[i].transform.childCount; p++)
                {
                    Debug.Log(hiddenProps[i].transform.GetChild(p).name);
                    hiddenProps[i].transform.GetChild(p).GetComponentInChildren<MeshRenderer>().enabled = true;
                }
                if (hiddenProps[i].GetComponent<MeshRenderer>())
                    hiddenProps[i].GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    private IEnumerator ShowBuildings(GameObject[] building)
    {


        for (int p = 6; p < 100; p+=10)
        {
            for (int i = 0; i < building.Length; i++)
            {
                if (Vector3.Distance(building[i].transform.position, transform.position) < p * 10)
                {
                    building[i].gameObject.SetActive(true);
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }


    private void Update()
    {
        //ViewBuilding();
        //If not already in a chat, show or hide the prompt of the NPC
        if (interactionManager.canStartDialog) ActivateNPCPrompt();
    }


    private void ViewBuilding()
    {
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Building").Length; i++)
        {
            Debug.Log(GameObject.FindGameObjectsWithTag("Building")[i].name);
            if (Vector3.Distance(transform.position, GameObject.FindGameObjectsWithTag("Building")[i].transform.position) > 100)
            {
                GameObject.FindGameObjectsWithTag("Building")[i].transform.GetChild(0).transform.gameObject.SetActive(false);
            }
            else
            {
                GameObject.FindGameObjectsWithTag("Building")[i].transform.GetChild(0).transform.gameObject.SetActive(true);
            }
        }
    }


    //Event => When clicking, detect nearest NPC and interact with it
    public void OnClickNPC(InputAction.CallbackContext context)
    {
        if (!GameObject.FindObjectOfType<MapScript>().mapContent.activeInHierarchy)
        {
            if (context.phase == InputActionPhase.Started)
            {
                if (canClickNPC && !ButtonManager.buttonManager.menu)
                {
                    SearchNearNPC();
                    RoboLibreta();
                    CallConversationNPC();

                }
            }
        }
        
    }


    public void OnClickNPCAcusate(InputAction.CallbackContext context)
    {
        if (canClickNPC && !ButtonManager.buttonManager.menu)
        {
            SearchNearNPC();
            CheckMurder();
        }

    }


    //Search for near NPCS and activate their prompts
    private void ActivateNPCPrompt()
    {
        foreach (GameObject npc in GameObject.FindGameObjectsWithTag("NPC"))
        {
            //Activate / Deactivate the npc prompt
            if (Vector3.Distance(transform.position, npc.transform.position) <= 3f)
            {
                PromptScript promptScript = npc.GetComponentInChildren<PromptScript>();
                promptScript.SetActive(true);
            }
            else
            {
                PromptScript promptScript = npc.GetComponentInChildren<PromptScript>();
                promptScript.SetActive(false);
            }
        }

        foreach (GameObject npc in GameObject.FindGameObjectsWithTag("Gossip"))
        {
            //Activate / Deactivate the npc prompt
            if (Vector3.Distance(transform.position, npc.transform.position) <= 3f)
            {
                PromptScript promptScript = npc.GetComponentInChildren<PromptScript>();
                promptScript.SetActive(true);
            }
            else
            {
                PromptScript promptScript = npc.GetComponentInChildren<PromptScript>();
                promptScript.SetActive(false);
            }
        }
    }


    private void RoboLibreta()
    {
        if (target != null && Vector3.Distance(transform.position, target.transform.position) < 3 && !conversation)
        {
            if (target.transform.GetComponent<NPC>().preset.characterRol == Rol.Ladrón)
            {
                interactionManager.stealNotepad = true;
                teclaNotepad.SetActive(false);
                padlock.SetActive(true);
                notepadLocked = true;
            }

            if (target.transform.GetComponent<NPC>().preset.characterRol == Rol.BuenSamaritano)
            {
                padlock.SetActive(false);
                teclaNotepad.SetActive(true);
                notepadLocked = false;
            }
        }
        
    }

    //Find the nearest npc, and activate it's prompt if so
    private void SearchNearNPC()
    {
        foreach (GameObject npc in GameObject.FindGameObjectsWithTag("NPC"))
        {
            float distance = Vector2.Distance(new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.z), new Vector2(npc.transform.position.x, npc.transform.position.z));

            if (distance <= 5f) 
            { 
                target = npc;
            }
        }

        foreach (GameObject npc in GameObject.FindGameObjectsWithTag("Gossip"))
        {
            float distance = Vector2.Distance(new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.z), new Vector2(npc.transform.position.x, npc.transform.position.z));

            if (distance <= 5f)
            {
                target = npc;
            }
        }

        if (target != null && Vector3.Distance(target.transform.position, transform.position) > 5)
        {
            target = null;
        }
    }


    //Speak with the NPC
    private void CallConversationNPC()
    {
        bool canConvert = false;
        if (FindObjectOfType<MapScript>() && FindObjectOfType<MapScript>().mapIsOpen)
        {
            canConvert = true;
        }

        if (FindObjectOfType<NotepadScript>() && FindObjectOfType<NotepadScript>().notepadIsOpen)
        {
            canConvert = true;
        }

        if (target != null && !conversation  && !canConvert)
        {
            Debug.Log("Entra");
            target.transform.GetComponent<NPC>().preset.OnInterectablePlayer();

            string conversationDialogue = target.transform.GetComponent<NPC>().preset.SetDialogue();

            if(conversationDialogue !="")
            {
                Transform lookPoint = target.transform.GetComponent<NPC>().lookPoint;
                GetComponent<InteractionManager>().StartDialogue(conversationDialogue, target, lookPoint);
            }
               
            target.transform.GetChild(0).transform.gameObject.SetActive(true);
        }
    }

    private void CheckMurder()
    {
        if (target != null && Vector3.Distance(transform.position, target.transform.position) < 3 && !conversation)
        {


            if (GameManager.gameManager.saveDataPreset.seenHints.Count >= 2)
            {
                if (target.transform.GetComponent<NPC>().preset.characterRol == Rol.Asesino)
                {
                    target.transform.Find("PromptUiDual").gameObject.SetActive(false);
                    GetComponent<MeshRenderer>().enabled = false;
                    target.transform.GetComponent<NPC>().preset.Acusation(target.transform.Find("Camera").gameObject, target.transform.Find("GuardPosition1"), target.transform.Find("GuardPosition2"));
                    GetComponent<PlayerController>().enabled = false;
                }
                else
                {
                    target.transform.GetComponent<NPC>().preset.Inocent();
                    string conversationDialogue = target.transform.GetComponent<NPC>().preset.SetDialogue();

                    if (conversationDialogue != "")
                    {
                        Transform lookPoint = target.transform.GetComponent<NPC>().lookPoint;
                        GetComponent<InteractionManager>().StartDialogue(conversationDialogue, target, lookPoint);
                    }

                    target.transform.GetChild(0).transform.gameObject.SetActive(true);
                }
            }
            else
            {
                Transform lookPoint = target.transform.GetComponent<NPC>().lookPoint;
                GetComponent<InteractionManager>().StartDialogue("No creo que tengas pistas suficientes como para hacer ninguna acusación.", target, lookPoint);
            }



            

            


        }
    }


}
