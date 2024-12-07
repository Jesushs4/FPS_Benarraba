using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName ="NPC", menuName = "New NPC")]

public class NPCData : PersistentScriptableObject
{
    public NPCData[] nPCDatas;

    [Header("Info")]
    public string nameNPC;
    public Vector3 position;
    public GameObject prefab;
    public HintDataScriptable hintScriptable;

    public Rol characterRol = Rol.None;
    public string namePosition;

    //Presentacion
    public List<string> dialoguePresentation = new List<string>(); 
    public List<string> dialoguePresentationBuenSamaritano = new List<string>(); 
    public List<string> dialoguePresentationMentiroso = new List<string>(); 
    public List<string> dialoguePresentationLadron = new List<string>(); 
    public List<string> dialoguePresentationAsesino = new List<string>();

    //Pista
    public List<string> dialoguePistaBuenSamaritano = new List<string>();
    public List<string> dialoguePistaTestigo = new List<string>();
    public List<string> dialoguePistaMentiroso = new List<string>();
    public List<string> dialoguePistasAsesino = new List<string>();

    //Pista dada
    public List<string> pistaDoneTestigo = new List<string>();
    public List<string> pistaDoneBuenSamaritano = new List<string>();
    public List<string> pistaDoneAsesino = new List<string>();
    public List<string> pistaDoneMentiroso = new List<string>();
    
    //Dialogo de personajes sin rol
    public List<string> dialogueWithoutRol = new List<string>();


    //Acusado
    public List<string> dialogueInocent = new List<string>();
    public List<string> dialogueAcusation = new List<string>();



    public int interaction;
    public bool acusate;

    public string dialogueRandom = "";

    
    public void CopyArray(List<string>dialogue, string[]dialogueCopy)
    {
        if (dialogue.Count <= 0)
        {
            foreach (var item in dialogueCopy)
            {
                dialogue.Add(item.Clone().ToString());
            }
        }
    }
    
    public void OnInterectablePlayer()
    {
        //Si las listas llegan a cero se copian del array
        //Presentaciones
        CopyArray(dialoguePresentation, prefab.GetComponent<NPC>().dialoguePresentation);
        CopyArray(dialoguePresentationMentiroso, prefab.GetComponent<NPC>().dialoguePresentationMentiroso);
        CopyArray(dialoguePresentationAsesino, prefab.GetComponent<NPC>().dialoguePresentationAsesino);
        CopyArray(dialoguePresentationLadron, prefab.GetComponent<NPC>().dialoguePresentationLadron);
        CopyArray(dialoguePresentationBuenSamaritano, prefab.GetComponent<NPC>().dialoguePresentationBuenSamaritano);

        //Pistas     
        CopyArray(dialoguePistaTestigo, prefab.GetComponent<NPC>().dialoguePistaTestigo);
        CopyArray(dialoguePistaMentiroso, prefab.GetComponent<NPC>().dialoguePistaMentiroso);
        CopyArray(dialoguePistasAsesino, prefab.GetComponent<NPC>().dialoguePistasAsesino);
        CopyArray(dialoguePistaBuenSamaritano, prefab.GetComponent<NPC>().dialoguePistaBuenSamaritano);

        //Pistas hechas
        CopyArray(pistaDoneTestigo, prefab.GetComponent<NPC>().pistaDoneTestigo);
        CopyArray(pistaDoneBuenSamaritano, prefab.GetComponent<NPC>().pistaDoneBuenSamaritano);
        CopyArray(pistaDoneAsesino, prefab.GetComponent<NPC>().pistaDoneAsesino);
        CopyArray(pistaDoneMentiroso, prefab.GetComponent<NPC>().pistaDoneMentiroso);
        
        CopyArray(dialogueWithoutRol, prefab.GetComponent<NPC>().dialogueWithoutRol);

        CopyArray(dialogueAcusation, prefab.GetComponent<NPC>().dialogueAcusation);
        CopyArray(dialogueInocent, prefab.GetComponent<NPC>().dialogueInocent);

        if (!acusate)
        {

            //In all interaction cases except the Rol.None, the case 1 is the hint
            switch (characterRol)
            {
                case Rol.None:

                    switch (interaction)
                    {
                        case 0:
                            Conversation(prefab.GetComponent<NPC>().preset.dialoguePresentation, prefab.GetComponent<NPC>().dialoguePresentation);
                            break;
                        case 1:
                            Conversation(prefab.GetComponent<NPC>().preset.dialogueWithoutRol, prefab.GetComponent<NPC>().dialogueWithoutRol);
                            break;
                        case 2:
                            Conversation(prefab.GetComponent<NPC>().preset.dialogueWithoutRol, prefab.GetComponent<NPC>().dialogueWithoutRol);
                            break;
                        default:
                            break;
                    }
                    break;

                case Rol.Mentiroso:

                    switch (interaction)
                    {
                        case 0:
                            Conversation(prefab.GetComponent<NPC>().preset.dialoguePresentationMentiroso, prefab.GetComponent<NPC>().dialoguePresentationMentiroso);
                            break;
                        case 1:
                            Conversation(prefab.GetComponent<NPC>().preset.dialoguePistaMentiroso, prefab.GetComponent<NPC>().dialoguePistaMentiroso, true);
                            break;
                        case 2:
                            Conversation(prefab.GetComponent<NPC>().preset.pistaDoneMentiroso, prefab.GetComponent<NPC>().pistaDoneMentiroso);
                            break;
                        default:
                            break;
                    }

                    break;
                case Rol.Asesino:

                    switch (interaction)
                    {
                        case 0:
                            Conversation(prefab.GetComponent<NPC>().preset.dialoguePresentationAsesino, prefab.GetComponent<NPC>().dialoguePresentationAsesino);
                            break;
                        case 1:
                            Conversation(prefab.GetComponent<NPC>().preset.dialoguePistasAsesino, prefab.GetComponent<NPC>().dialoguePistasAsesino, true);
                            break;
                        case 2:
                            Conversation(prefab.GetComponent<NPC>().preset.pistaDoneAsesino, prefab.GetComponent<NPC>().pistaDoneAsesino);
                            break;
                        default:
                            break;
                    }

                    break;
                case Rol.Ladrón:

                    switch (interaction)
                    {
                        case 0:
                            Conversation(prefab.GetComponent<NPC>().preset.dialoguePresentationLadron, prefab.GetComponent<NPC>().dialoguePresentationLadron);
                            break;
                        case 1:
                            Conversation(prefab.GetComponent<NPC>().preset.dialoguePistasAsesino, prefab.GetComponent<NPC>().dialoguePistasAsesino, true);
                            break;
                        case 2:
                            Conversation(prefab.GetComponent<NPC>().preset.pistaDoneAsesino, prefab.GetComponent<NPC>().pistaDoneAsesino);
                            break;
                        default:
                            break;
                    }

                    break;
                case Rol.BuenSamaritano:

                    switch (interaction)
                    {
                        case 0:
                            Conversation(prefab.GetComponent<NPC>().preset.dialoguePresentationBuenSamaritano, prefab.GetComponent<NPC>().dialoguePresentationBuenSamaritano);
                            break;
                        case 1:
                            Conversation(prefab.GetComponent<NPC>().preset.dialoguePistaBuenSamaritano, prefab.GetComponent<NPC>().dialoguePistaBuenSamaritano, true);
                            break;
                        case 2:
                            Conversation(prefab.GetComponent<NPC>().preset.pistaDoneBuenSamaritano, prefab.GetComponent<NPC>().pistaDoneBuenSamaritano);
                            break;
                        default:
                            break;
                    }

                    break;
                default:
                    break;
            }
        }
        else
        {
            Conversation(prefab.GetComponent<NPC>().preset.dialogueAcusation, prefab.GetComponent<NPC>().dialogueAcusation);
        }

        if (interaction >= 3) interaction = 2;
    }

    public void Acusation(GameObject camera, Transform guardPosition1, Transform guardPosition2)
    {
        camera.SetActive(true);
        camera.GetComponent<Camera>().enabled = true;
        Camera.main.enabled = false;
        //GO-TO ir a fin de juego
        GameManager.gameManager.Detener(guardPosition1, guardPosition2);
        //GameManager.gameManager.EndTheGame();
    }
    /*public void Acusation()
    {
        //GO-TO ir a fin de juego
        Debug.Log("Has ganado");
        Debug.Log("Detenido");
    }*/

    public void Inocent()
    {
        GameManager.gameManager.score = GameManager.gameManager.score - GameManager.gameManager.initalScore / 3;
        GameManager.gameManager.attemps--;
        if(GameManager.gameManager.attemps <= 0)
        {
            //GO-TO ir a fin de juego
            Debug.Log("Has perdido");
            GameManager.gameManager.score -= 1;
            //End the game
            GameManager.gameManager.EndTheGame();
        }
        CopyArray(dialogueInocent, prefab.GetComponent<NPC>().dialogueInocent);
        Conversation(prefab.GetComponent<NPC>().preset.dialogueInocent, prefab.GetComponent<NPC>().dialogueInocent);
    }


    //Default conversation method
    private void Conversation(List<string> dialogue, string[] dialogueCopy)
    {
        interaction++;
        int randomDialogue;
        randomDialogue = Random.Range(0, dialogue.Count);
        dialogueRandom = dialogue[randomDialogue];
        dialogue.RemoveAt(randomDialogue);
    }


    //Override conversation method for hint
    private void Conversation(List<string> dialogue, string[] dialogueCopy, bool isHint)
    {
        interaction++;
        Save();
        int randomDialogue;
        randomDialogue = Random.Range(0, dialogue.Count);
        dialogueRandom = dialogue[randomDialogue];

        if (dialogueRandom == null) Debug.Log("ES NULL");

        string asesino = RecoveryNameRol(Rol.Asesino); //Recuperamos el nombre del asesino
        string mentiroso = RecoveryNameRol(Rol.Mentiroso); //Recuperamos el nombre del mentiroso
        string buenSamaritano = RecoveryNameRol(Rol.BuenSamaritano); //Recuperamos el nombre del buen samaritano
        string ladron = RecoveryNameRol(Rol.Ladrón); //Recuperamos el nombre del ladrón
        string none = RecoveryNameRol(Rol.None); //Recuperamos el nombre del ladrón

        string[] namesRoles = new string[9];
        //Debug.Log("Buen samaritano: " + buenSamaritano + ":"+ gameObject.name);
        namesRoles[0] = buenSamaritano;
        //Debug.Log("Ladrón: " + ladron);
        namesRoles[1] = ladron;
        //Debug.Log("Ninguno: " + none);
        namesRoles[2] = none;

        string lugar = "la plaza";
        string placeWrong = RecoveryPlaceRol(Rol.None);

        string cadenaRemplazada = dialogueRandom.Replace("<Asesino/Ladrón>", GameManager.gameManager.asesino);
        cadenaRemplazada = cadenaRemplazada.Replace("<LugarAsesinoLadrón>", GameManager.gameManager.lugar);
        cadenaRemplazada = cadenaRemplazada.Replace("<LugarCrimen>", GameManager.gameManager.placeWrong);
        cadenaRemplazada = cadenaRemplazada.Replace("<PersonaAleatoria>", GameManager.gameManager.namesRoles[UnityEngine.Random.Range(0, namesRoles.Length)]);
        cadenaRemplazada = cadenaRemplazada.Replace("<LugarIncorrecto>", GameManager.gameManager.placeWrong);
        cadenaRemplazada = cadenaRemplazada.Replace("<LugarCrimen>", GameManager.gameManager.lugar);
        cadenaRemplazada = cadenaRemplazada.Replace("<BuenSamaritano>", GameManager.gameManager.buenSamaritano);
        cadenaRemplazada = cadenaRemplazada.Replace("<Mentiroso>", GameManager.gameManager.mentiroso);
        cadenaRemplazada = cadenaRemplazada.Replace("<Chismoso>", "el chismoso");
        cadenaRemplazada = cadenaRemplazada.Replace("<LugarNiñoBici>", "ahí");
        cadenaRemplazada = cadenaRemplazada.Replace("<LugarChismoso>", "LUGAR DONDE LO PONGAMOS");

        dialogueRandom = cadenaRemplazada;





        //If the conversation is a hint, add to the hints list
        if (isHint)
        {
            //Get the Notepad script
            NotepadScript notepadScript = FindObjectOfType<NotepadScript>();

            //Add the hint if the notepad is not stolen
            if (notepadScript != null) notepadScript.TryAddHint(dialogueRandom + " (" + FindObjectOfType<PlayerController>().target.GetComponent<NPC>().preset.nameNPC + ")");
        }

        dialogue.RemoveAt(randomDialogue);
    }


    public string SetDialogue()
    {
        //Debug.Log(dialogueRandom);
        return dialogueRandom;
    }

    /// <summary>
    /// Recupera el nombre del rol 
    /// </summary>
    /// <param name="rol"></param>
    /// <returns></returns>
    public string RecoveryNameRol(Rol rol)
    {
        for (int i = 0; i < nPCDatas.Length; i++)
        {
            if (nPCDatas[i].characterRol == rol)
            {
                //Debug.Log(nPCDatas[i].name);
                return nPCDatas[i].nameNPC;
            }
        }
        return null;
    }


    public string RecoveryPlaceRol(Rol rol)
    {
        for (int i = 0; i < nPCDatas.Length; i++)
        {
            if (nPCDatas[i].characterRol == rol)
            {
                //Debug.Log(nPCDatas[i].namePosition);
                return nPCDatas[i].namePosition;
            }
        }
        return null;
    }
}
