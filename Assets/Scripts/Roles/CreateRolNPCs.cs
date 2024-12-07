using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Rol
{
    None,
    Mentiroso,
    Asesino,
    Ladrón,
    BuenSamaritano
}


public class CreateRolNPCs : MonoBehaviour
{
    public GameObject containerNPCs;

    public List<Rol> takenRols = new List<Rol>();

    public void OnBtCreateNPCRol(NPCData newNPC, Vector3 posSpawn, string namePosition)
    {
        //Debug.Log(newNPC.name);

        Vector3 newPos = Vector3.zero;

        if(newNPC.position == Vector3.zero)
        {
            newPos = posSpawn;
            newNPC.namePosition = namePosition;
        }
        else
        {
            newPos = newNPC.position;
        }




        GameObject NPC = Instantiate(newNPC.prefab, newPos, Quaternion.identity);
        NPC.transform.parent = containerNPCs.transform;

        //Obtain a random characterRol that hasn't be taken
        NPCData npcData = NPC.GetComponent<NPC>().preset;

        int numberOfRoles = Enum.GetValues(typeof(Rol)).Length;

        int count = 0;

        //Mientras el rol esté asignado o sea ninguno, se repite su asignación
        while (takenRols.Contains(npcData.characterRol) || npcData.characterRol.Equals(Rol.None))
        {
            //Debug.Log("Cogidos: " + takenRols.Count + " / " + numberOfRoles);
            //If all the roles have been asigned, exit
            if (takenRols.Count >= numberOfRoles -1) break;
            else npcData.characterRol = (Rol)UnityEngine.Random.Range(0, numberOfRoles);  
        }

        //Make sure the rol was not taken before
        //do
        //{
        //    npcData.characterRol = (Rol)UnityEngine.Random.Range(0, numberOfRoles);

        //} while (takenRols.Contains(npcData.characterRol) || npcData.characterRol.Equals(Rol.None));

        //npcData.namePosition = namePosition;
        npcData.Save();
        //Add the rol to the already taken list
        takenRols.Add(npcData.characterRol);

        //Rols.Remove(Rols[rolRandom]);      
    }    
}
