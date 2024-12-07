using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePeople : MonoBehaviour
{
    public GameObject contentPeople;
    public GameObject[] persons;
    public int numPersonsPerPoint=10;

    bool isCreatedNPC;

    private void Update()
    {
        if (!isCreatedNPC && Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 100)
        {
            CreateNPCs();
        }
    }

    private void CreateNPCs()
    {
        for (int i = 0; i < numPersonsPerPoint; i++)
        {
            GameObject person = Instantiate(persons[Random.Range(0, persons.Length)], transform.position, Quaternion.identity);
            person.transform.parent = contentPeople.transform;
            isCreatedNPC = true;
        }
    }
}
