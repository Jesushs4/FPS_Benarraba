using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerSave" , menuName = "New Save")]
public class PlayerSaveData : PersistentScriptableObject
{
    public bool isStarted = false;
    public int currentDay = 0;
    public int score = 0;
    public float hourData = 0;
    public float timeData = 0;
    public Vector3 playerPosition = Vector3.zero;
    public Quaternion playerRotation = Quaternion.identity;
    public List<string> seenHints = new List<string>();

    //public Vector3[] scooterPositionData;
    //public Quaternion[] scooterQuaternionData;

    public List<Vector3> scooterPositionData = new List<Vector3>();
    public List<Quaternion> scooterQuaternionData = new List<Quaternion>();


    //Volumen
    public float volumen;
    public float volumenVoice;
    public float SFX;

    //Save data methods with override
    public void SaveData(bool started)
    {
        isStarted = started;
        Save();
    }

    public void SaveData(int day, int points)
    {
        currentDay = day;
        score = points;
        Save();
    }

    public void SaveData(Vector3 position, Quaternion rotation)
    {
        playerPosition = position;
        playerRotation = rotation;
        Save();
    }

    public void SaveData(string hintString)
    {
        //Try to add the hint
        if (!seenHints.Contains(hintString) && hintString != null)
        {
            seenHints.Add(hintString);
        }
        else if (hintString != null) Debug.Log("Hint already in list");
        else Debug.Log("Hint is null");
        Save();
    }


    //Clear the save Data
    public void EraseData()
    {
        float previewVolumen = volumen;
        float previewVoiceVolumen = volumenVoice;
        Debug.Log(volumen);

        isStarted = false;
        currentDay = 0;
        score = 0;
        playerPosition = Vector3.zero;
        playerRotation = Quaternion.identity;
        seenHints.Clear();
        volumen = previewVolumen;
        volumenVoice = previewVoiceVolumen;
        Save();
    }
}
