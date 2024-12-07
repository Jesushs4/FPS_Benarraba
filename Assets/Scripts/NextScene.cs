using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public GameObject[] sounds;
    public PlayerSaveData playerSaveData;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].GetComponent<AudioSource>().volume = playerSaveData.volumen;
        }


        if (PlayerPrefs.HasKey("quality"))
        {
            int quality = PlayerPrefs.GetInt("quality");
            QualitySettings.SetQualityLevel(quality, true);
        }
    }


    public void LoadNextScene()
    {
        SceneManager.LoadScene("IntroSceneForest");
    }

    public void LoadBenarraba()
    {
        SceneManager.LoadScene("Benarraba");
    }
}
