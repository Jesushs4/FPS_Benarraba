using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager interactionManager;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    PromptScript promptScript;

    private float typingTime = 0.05f;
    public bool canStartDialog = true;
    public bool isSkipping = false;
    public bool isOnConversation = false;
    //private bool isPlayerInRange;
    //private bool didDialogueStart;
    //private int lineIndex;

    public bool stealNotepad;
    public bool findNotepad;

    private void Start()
    {
        interactionManager = this;
    }

    public void StartDialogue(string phrase, GameObject target, Transform pointToLook)
    {
        isOnConversation = true;
        dialoguePanel.SetActive(true);
        //lineIndex = 0;

        if (canStartDialog)
        {
            Debug.Log("PPA");
            StartCoroutine(ShowLine(phrase, target, pointToLook));
        }
    }
    

    //Show the dialog 
    private IEnumerator ShowLine(string phrase, GameObject target, Transform pointToLook)
    {
        Debug.Log(phrase); 
        if (stealNotepad && !findNotepad)
        {
            if (target.GetComponent<NPC>())
            {
                if (target.GetComponent<NPC>().preset.characterRol == Rol.BuenSamaritano)
                {
                    phrase = "Aquí tienes tu libreta, debes tener más cuidado con tus cosas.";
                    findNotepad = true;
                }
                else if (target.GetComponent<NPC>().preset.characterRol == Rol.Ladrón)
                {
                    phrase = "No sé de que me hablas. ¿Qué libreta?";
                }
                else
                {
                    phrase = "Te han robado la libreta, habla con el buen samaritano.";
                }
            }
        }





        Cursor.lockState = CursorLockMode.None;
        isSkipping = false;
        canStartDialog = false;
        GetComponent<PlayerController>().canClickNPC = false;

        ButtonManager.buttonManager.menu = true;

        dialogueText.text = string.Empty;

        //Get the prompt script
        promptScript = target.GetComponentInChildren<PromptScript>();

        //Hide the prompt if exists
        if (promptScript != null) promptScript.SetActive(false);

        transform.DOLookAt(new Vector3(
            target.transform.position.x, 
            transform.position.y, 
            target.transform.position.z), 1f);

        target.transform.DOLookAt(new Vector3(
            transform.position.x,
            target.transform.position.y,
            transform.position.z), 1f);

        Camera.main.transform.DOLookAt(pointToLook.position, 1f);

        yield return new WaitForSeconds(1f);




        if (target.GetComponent<NPC>())
        {
            string asesino = "";
            if(asesino =="")
                asesino = target.GetComponent<NPC>().preset.RecoveryNameRol(Rol.Asesino); //Recuperamos el nombre del asesino
            Debug.Log("Asesino: " + asesino);
            string mentiroso = target.GetComponent<NPC>().preset.RecoveryNameRol(Rol.Mentiroso); //Recuperamos el nombre del mentiroso
            string buenSamaritano = target.GetComponent<NPC>().preset.RecoveryNameRol(Rol.BuenSamaritano); //Recuperamos el nombre del buen samaritano
            string ladron = target.GetComponent<NPC>().preset.RecoveryNameRol(Rol.Ladrón); //Recuperamos el nombre del ladrón
            string none = target.GetComponent<NPC>().preset.RecoveryNameRol(Rol.None); //Recuperamos el nombre del ladrón
            Debug.Log("Ninguno: " + none);



            string[] namesRoles = new string[3];
            //Debug.Log("Buen samaritano: " + buenSamaritano + ":"+ gameObject.name);
            namesRoles[0] = buenSamaritano;
            //Debug.Log("Ladrón: " + ladron);
            namesRoles[1] = ladron;
            //Debug.Log("Ninguno: " + none);
            namesRoles[2] = none;
            yield return new WaitForSeconds(1f);

            string placeWrong = target.GetComponent<NPC>().preset.RecoveryPlaceRol(Rol.None);
            Debug.Log("Lugar equivocado: " + placeWrong);


            string lugar = "la plaza";

            string cadenaRemplazada = phrase.Replace("<Asesino/Ladrón>", asesino);
            cadenaRemplazada = cadenaRemplazada.Replace("<LugarAsesinoLadrón>", lugar);
            cadenaRemplazada = cadenaRemplazada.Replace("<LugarCrimen>", placeWrong);
            cadenaRemplazada = cadenaRemplazada.Replace("<PersonaAleatoria>", namesRoles[UnityEngine.Random.Range(0, namesRoles.Length)]);
            cadenaRemplazada = cadenaRemplazada.Replace("<LugarIncorrecto>", placeWrong);
            cadenaRemplazada = cadenaRemplazada.Replace("<LugarCrimen>", lugar);
            cadenaRemplazada = cadenaRemplazada.Replace("<BuenSamaritano>", buenSamaritano);
            cadenaRemplazada = cadenaRemplazada.Replace("<Mentiroso>", mentiroso);
            cadenaRemplazada = cadenaRemplazada.Replace("<Chismoso>", "el chismoso");
            cadenaRemplazada = cadenaRemplazada.Replace("<LugarNiñoBici>", "ahí");
            cadenaRemplazada = cadenaRemplazada.Replace("<LugarChismoso>", "LUGAR DONDE LO PONGAMOS");
            phrase = cadenaRemplazada;
            //Debug.Log(cadenaRemplazada + " : " + preset.name);
        }
        Debug.Log(phrase);











        //Deactivate exclamation mark
        target.transform.GetChild(0).transform.gameObject.SetActive(false);

        Time.timeScale = 0f;

        //Play the NPC audio
        target.GetComponentInChildren<RolNPCVoicesScript>().PlayVoice();

        //Guardamos los valores previos
        float lastSizeFont = dialogueText.fontSize;
        float lastTypingTime = typingTime;

        //Si quien habla es el alcalde se ajusta el tiempo a su discurso
        if (target.GetComponentInChildren<RolNPCVoicesScript>().npcAudios[0].name == "Voz alcalde")
        {
            typingTime = target.GetComponentInChildren<RolNPCVoicesScript>().npcAudios[0].length/ phrase.Length -.005f;
            dialogueText.fontSize = 18;
        }

        foreach (char ch in phrase)
        {
            if (!isSkipping)
            {
                dialogueText.text += ch;
                yield return new WaitForSecondsRealtime(typingTime);
            } 
        }

        if (!isSkipping) yield return new WaitForSecondsRealtime(1);
        else
            target.GetComponentInChildren<RolNPCVoicesScript>().StopVoice();

        GetComponent<PlayerController>().conversation = false;

        dialoguePanel.SetActive(false);
        Time.timeScale = 1f;
        ButtonManager.buttonManager.menu = false;

        //Show again the prompt
        if (promptScript != null) promptScript.SetActive(true);


        isOnConversation = false;
        canStartDialog = true;
        GetComponent<PlayerController>().canClickNPC = true;

        Cursor.lockState = CursorLockMode.Locked;

        //Restauramos los valores
        if (target.GetComponentInChildren<RolNPCVoicesScript>().npcAudios[0].name == "Voz alcalde")
        {
            typingTime = lastTypingTime;
            dialogueText.fontSize = lastSizeFont;
        }
    }

    public void OnBtSkip()
    {
        isSkipping = true;
    }

}
