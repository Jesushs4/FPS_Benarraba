using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPC : MonoBehaviour
{
    public NPCData preset;
    public Transform lookPoint;

    public string[] dialoguePresentation;
    public string[] dialoguePresentationBuenSamaritano;
    public string[] dialoguePresentationMentiroso;
    public string[] dialoguePresentationLadron;
    public string[] dialoguePresentationAsesino;

    public string[] dialoguePistaBuenSamaritano;
    public string[] dialoguePistaTestigo;
    public string[] dialoguePistaMentiroso;
    public string[] dialoguePistasAsesino;

    public string[] pistaDoneTestigo;
    public string[] pistaDoneBuenSamaritano;
    public string[] pistaDoneAsesino;
    public string[] pistaDoneMentiroso;

    public string[] dialogueWithoutRol;

    public string[] dialogueInocent;
    public string[] dialogueAcusation;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        preset.interaction = 0;
        preset.acusate = false;
        preset.position = transform.position;


        string asesino = preset.RecoveryNameRol(Rol.Asesino); //Recuperamos el nombre del asesino
        string mentiroso = preset.RecoveryNameRol(Rol.Mentiroso); //Recuperamos el nombre del mentiroso
        string buenSamaritano = preset.RecoveryNameRol(Rol.BuenSamaritano); //Recuperamos el nombre del buen samaritano
        string ladron = preset.RecoveryNameRol(Rol.Ladrón); //Recuperamos el nombre del ladrón
        string none = preset.RecoveryNameRol(Rol.None); //Recuperamos el nombre del ladrón

        string[] namesRoles = new string[9];
        //Debug.Log("Buen samaritano: " + buenSamaritano + ":"+ gameObject.name);
        namesRoles[0] = buenSamaritano;
        //Debug.Log("Ladrón: " + ladron);
        namesRoles[1] = ladron;
        //Debug.Log("Ninguno: " + none);
        namesRoles[2] = none;
        yield return new WaitForSeconds(1f);

        string placeWrong = preset.RecoveryPlaceRol(Rol.None);






        string lugar = "la plaza";

        for (int i = 0; i < dialoguePistaTestigo.Length; i++)
        {
            string cadenaRemplazada = dialoguePistaTestigo[i].Replace("<Asesino/Ladrón>", asesino);
            cadenaRemplazada = cadenaRemplazada.Replace("<LugarAsesinoLadrón>", lugar);
            cadenaRemplazada = cadenaRemplazada.Replace("<LugarCrimen>", placeWrong);
            dialoguePistaTestigo[i] = cadenaRemplazada;
            //Debug.Log(cadenaRemplazada + " : " + preset.name);
        }

        for (int i = 0; i < dialoguePistaMentiroso.Length; i++)
        {
            string cadenaRemplazada = dialoguePistaMentiroso[i].Replace("<PersonaAleatoria>", namesRoles[Random.Range(0,namesRoles.Length)]);
            cadenaRemplazada = cadenaRemplazada.Replace("<LugarIncorrecto>", placeWrong);
            cadenaRemplazada = cadenaRemplazada.Replace("<LugarCrimen>", lugar);
            cadenaRemplazada = cadenaRemplazada.Replace("<BuenSamaritano>", buenSamaritano);
            dialoguePistaMentiroso[i] = cadenaRemplazada;

        }

        for (int i = 0; i < dialoguePistaBuenSamaritano.Length; i++)
        {
            string cadenaRemplazada = dialoguePistaBuenSamaritano[i].Replace("<Mentiroso>", mentiroso);
            cadenaRemplazada = cadenaRemplazada.Replace("<Chismoso>", "el chismoso");
            cadenaRemplazada = cadenaRemplazada.Replace("<LugarNiñoBici>", "ahí");
            cadenaRemplazada = cadenaRemplazada.Replace("<LugarChismoso>", "LUGAR DONDE LO PONGAMOS");
            dialoguePistaBuenSamaritano[i] = cadenaRemplazada;
            //Debug.Log(cadenaRemplazada + " : " + preset.name);
        }




        preset.dialoguePistaTestigo.Clear();
        preset.pistaDoneMentiroso.Clear();
        preset.dialoguePistaBuenSamaritano.Clear();


        for (int i = 0; i < dialoguePistaTestigo.Length; i++)
        {
            preset.dialoguePistaTestigo.Add(dialoguePistaTestigo[i]);
        }

        for (int i = 0; i < pistaDoneMentiroso.Length; i++)
        {
            preset.pistaDoneMentiroso.Add(pistaDoneMentiroso[i]);
        }

        for (int i = 0; i < dialoguePistaBuenSamaritano.Length; i++)
        {
            preset.dialoguePistaBuenSamaritano.Add(dialoguePistaBuenSamaritano[i]);
        }

    }
}
