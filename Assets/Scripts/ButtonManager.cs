using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager buttonManager;

    public GameObject menuPrincipal;
    public GameObject menuGame;
    public GameObject menuCredits;
    public GameObject menuOptions;
    public GameObject txtCredits;
    public GameObject btContinue;
    public GameObject btSkip;

    public PlayerSaveData saveDataPreset;

    public NPCData[] nPCDatas;

    public bool menu;

    public const string SaveGame = "SaveGame";
    private string saveGame;

    public GameObject pnlLoading;


    //public Slider volumenMusic;
    //public Slider volumenVoices;


    private void Start()
    {
        string recoveryDay = PlayerPrefs.GetString(SaveGame);
        buttonManager = this;
        //if(saveDataPreset != null)
            //saveDataPreset.Load();



        if (menuPrincipal && !GameObject.FindGameObjectWithTag("Player"))
        {
            SelectMenu(menuPrincipal);
        }     
        
        if(btContinue != null && recoveryDay=="")
        {
            btContinue.GetComponent<Button>().interactable = false;
        }

        
        if(saveDataPreset != null)
        {
            if (!saveDataPreset.CheckFile())
            {
                if (btContinue != null)
                {
                    btContinue.GetComponent<Button>().interactable = false;
                    btContinue.transform.GetChild(0).GetComponent<Button>().interactable = false;
                }
            }
        }


    }

    public void OnBtPlay()
    {

        for (int i = 0; i < nPCDatas.Length; i++)
        {
            nPCDatas[i].characterRol = Rol.None;
            nPCDatas[i].position = Vector3.zero;
            nPCDatas[i].interaction = 0;
            nPCDatas[i].Save();
        }


        //Debug.Log("BORRANDO");
        //for (int i = 0; i < saveDataPreset.scooterPositionData.Count; i++)
        //{
        //    saveDataPreset.scooterPositionData[i] = Vector3.zero;
        //}

        saveDataPreset.scooterPositionData.Clear();
        saveDataPreset.scooterQuaternionData.Clear();

        //for (int i = 0; i < saveDataPreset.scooterQuaternionData.Count; i++)
        //{
        //    saveDataPreset.scooterQuaternionData[i] = new Quaternion(0,0,0,0);
        //}

        Debug.Log("Pulso botón: "+Time.time);
        if(pnlLoading)
            pnlLoading.SetActive(true);
        //Debug.Log("Jugar partida nueva");
        Time.timeScale = 1;
        saveGame = "";
        PlayerPrefs.SetString(SaveGame, saveGame);
        saveDataPreset.EraseData();
        SceneManager.LoadSceneAsync("IntroSceneBenarraba");
        Cursor.lockState = CursorLockMode.Locked;
        //SceneManager.LoadScene("Benarraba");
    }

    public void OnBtPlayAgain()
    {
        if (pnlLoading)
            pnlLoading.SetActive(true);
        Time.timeScale = 1; 
        SceneManager.LoadSceneAsync("Benarraba");
        //SceneManager.LoadScene("Benarraba");
    }

    public void OnBtMenuPpal()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnBtCredits()
    {
        
        SelectMenu(menuCredits);
    }

    public void OnBtOptions()
    {

        //Debug.Log(saveDataPreset.volumen);
        menuPrincipal.SetActive(false);
        SelectMenu(menuOptions);

        //if (volumenMusic != null)
        //{
        //    Debug.Log("SOLO EN MENU");
        //    Debug.Log(saveDataPreset.volumen);
        //    volumenMusic.value = saveDataPreset.volumen;
        //}
        //if (volumenVoices != null)
        //{
        //    volumenVoices.value = saveDataPreset.volumenVoice;
        //}





    }

    public void OnBtMenu(InputAction.CallbackContext context)
    {
        if (!GameManager.gameManager.pnlLoading.activeInHierarchy)
        {
            if (context.phase == InputActionPhase.Started)
            {
                if (!menu) //Si no está el menu activo 
                {
                    if (!InteractionManager.interactionManager.isOnConversation) // y no está conversación
                    {
                        Cursor.lockState = CursorLockMode.None;
                        menu = true;
                        menuGame.SetActive(true);
                        Time.timeScale = 0.0f;
                    }
                    else
                    {
                        Debug.Log("Aquí no puede pasar");
                        InteractionManager.interactionManager.isSkipping = true;
                    }
                }
                else
                {
                    if (!InteractionManager.interactionManager.isOnConversation)
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        Time.timeScale = 1.0f;
                        menu = false;
                        menuGame.SetActive(false);
                    }
                    else
                    {
                        InteractionManager.interactionManager.isSkipping = true;
                    }
                }
            }
        }        
    }

    public void OnBtContinuePlay()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
        menu = false;
        menuGame.SetActive(false);
    }

    //When clicked or from time to time. Save the data on scriptable object in the Presets folder
    public void OnBtSaving()
    {
        //Posicion jugador y npc principales
        //Día actual
        //Personajes asesinados
        //Roles/personajes

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        ////Guardamos el día, la posicion del jugador y su rotacion;
        //saveGame = GameManager.gameManager.DayCur+":"+ 
        //    player.transform.position.x + ":" + player.transform.position.y + ":" + player.transform.position.z + ":" +
        //    player.transform.rotation.x + ":" + player.transform.rotation.y + ":" + player.transform.rotation.z + ":" + player.transform.rotation.w;
        //PlayerPrefs.SetString(SaveGame, saveGame);

        //Save on scriptable object
        saveDataPreset.hourData = GameObject.FindObjectOfType<TimeController>().hour;
        saveDataPreset.timeData = GameObject.FindObjectOfType<DayNightCycle>().time;


        saveDataPreset.SaveData(true); //Has the game started
        saveDataPreset.SaveData(GameManager.gameManager.DayCur , GameManager.gameManager.score); //Current day and score
        saveDataPreset.SaveData(player.transform.position, player.transform.rotation); //Position and rotation

        //Debug.Log("GUARDANDO");
        GameObject[] scootersPosition = GameObject.FindGameObjectsWithTag("Scooter");
        
        saveDataPreset.scooterPositionData.Clear();
        saveDataPreset.scooterQuaternionData.Clear();

        foreach (GameObject scooter in scootersPosition)
        {
            saveDataPreset.scooterPositionData.Add(scooter.transform.position);
            saveDataPreset.scooterQuaternionData.Add(scooter.transform.rotation);
        }

        //for (int i = 0; i < GameObject.FindGameObjectsWithTag("Scooter").Length; i++)
        //{
        //    saveDataPreset.scooterPositionData[i] = GameObject.FindGameObjectsWithTag("Scooter")[i].transform.position;
        //    saveDataPreset.scooterQuaternionData[i] = GameObject.FindGameObjectsWithTag("Scooter")[i].transform.rotation;
        //    //Debug.Log(GameObject.FindGameObjectsWithTag("Scooter")[i] + ":" + GameObject.FindGameObjectsWithTag("Scooter")[i].transform.position);
        //}
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
        menu = false;
        menuGame.SetActive(false);
    }

    public void OnBtReturnMenuPrincipal()
    {
        SelectMenu(menuPrincipal);
    }

    public void OnBtExit()
    {
        Application.Quit();
        Debug.Log("Has salido");
    }

    public void SelectMenu(GameObject menu)
    {
        menuPrincipal.SetActive(false);
        menuCredits.SetActive(false);
        menuOptions.SetActive(false);

        menu.SetActive(true);
    }
    public void CinematicIntro()
    {
        //QUITAR TILDE TODO
        SceneManager.LoadScene("IntroSceneBenarrabá");
    }
    

    //Skip unity scene, but if it's Benarraba one, jump on the time
    public void OnBtSkipVideo(string nextScene)
    {
        if (nextScene != "Benarraba")
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            btSkip.SetActive(false);
            PlayableDirector playable = FindObjectOfType<PlayableDirector>();
            playable.Stop();
            playable.time = 10.5f;
            playable.Play();
        }
    }
}
