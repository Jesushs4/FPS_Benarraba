using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.AI;
public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    private string namePlayer = "Player";

    private int dayCur;
    public int score;
    public int initalScore;
    public int attemps=3;
    public string NamePlayer { get => namePlayer; set => namePlayer = value; }
    public int DayCur { get => dayCur; set => dayCur = value; }
    public int Score { get => score; set => score = value; }
    public int Attemps { get => attemps; set => attemps = value; }

    public GameObject hintsDictionary;

    public const string SaveGame = "SaveGame";
    private string saveGame;
    string recoveryDataGame;
    public bool playAgain;

    public PlayerSaveData saveDataPreset;
    public NPCData[] presets;
    public List<NPCData> presetsList = new List<NPCData>();
    public GameObject posSpawn;
    public GameObject civilGuard;

    public GameObject mayorObject;
    private GameObject player;
    private PlayerMovementScript playerMovementScript;
    private InteractionManager interactionManager;

    public GameObject BDead;
    public GameObject pnlLoading;
    public GameObject clock;
    public GameObject notepad;
    public GameObject padlock;
    public GameObject teclaNotepad;


    public string asesino; //Recuperamos el nombre del asesino
    public string mentiroso; //Recuperamos el nombre del mentiroso
    public string buenSamaritano; //Recuperamos el nombre del buen samaritano
    public string ladron; //Recuperamos el nombre del ladrón
    public string none; //Recuperamos el nombre del ladrón

    public string[] namesRoles = new string[9];
    //Debug.Log("Buen samaritano: " + buenSamaritano + ":"+ gameObject.name);


    public string lugar;
    public string placeWrong;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("quality"))
        {
            int quality = PlayerPrefs.GetInt("quality");
            QualitySettings.SetQualityLevel(quality, true);
        }



        saveDataPreset.Load();
        recoveryDataGame = PlayerPrefs.GetString(SaveGame);
        //Check the number of GameManager instances
        //int instancesNumber = FindObjectsOfType<GameManager>().Length;

        ////If there is another one destroy it
        //if (instancesNumber != 1)
        //{
        //    Destroy(this.gameObject);
        //}
        ////if there is not any other keep it
        //else
        //{
        //    DontDestroyOnLoad(this.gameObject);
        //}

        //Find the mayor
        mayorObject = GameObject.FindGameObjectWithTag("Mayor");

        //Find the player
        player = GameObject.FindGameObjectWithTag("Player");

        //Find the player movement script
        playerMovementScript = player.GetComponent<PlayerMovementScript>();

        //Find the interaction manager script
        interactionManager = player.GetComponent<InteractionManager>();
    }
    public string RecoveryNameRol(Rol rol, string checkRol)
    {
        if (checkRol == "")
        {
            for (int i = 0; i < presets.Length; i++)
            {
                if (presets[i].characterRol == rol)
                {
                    //Debug.Log(nPCDatas[i].name);
                    return presets[i].nameNPC;
                }
            }
        }
        return checkRol;
    }


    public string RecoveryPlaceRol(Rol rol)
    {
        for (int i = 0; i < presets.Length; i++)
        {
            if (presets[i].characterRol == rol)
            {
                //Debug.Log(nPCDatas[i].namePosition);
                return presets[i].namePosition;
            }
        }
        return null;
    }


    private void Start()
    {
        BDead.SetActive(false);

        pnlLoading.SetActive(true);

        initalScore = score;

        gameManager = this;

        //Creamos los NPCs con roles
        SpawnNPC();


        
        RecoveryObjectGameManager();
        //RecoveryDataGame();
        playAgain = false;

        Invoke(nameof(StartGame), 1f);

        asesino = RecoveryNameRol(Rol.Asesino, asesino); //Recuperamos el nombre del asesino
        mentiroso = RecoveryNameRol(Rol.Mentiroso, mentiroso); //Recuperamos el nombre del mentiroso
        buenSamaritano = RecoveryNameRol(Rol.BuenSamaritano, buenSamaritano); //Recuperamos el nombre del buen samaritano
        ladron = RecoveryNameRol(Rol.Ladrón, ladron); //Recuperamos el nombre del ladrón
        none = RecoveryNameRol(Rol.None, none); //Recuperamos el nombre del ladrón

        //Debug.Log("Buen samaritano: " + buenSamaritano + ":"+ gameObject.name);
        namesRoles[0] = buenSamaritano;
        //Debug.Log("Ladrón: " + ladron);
        namesRoles[1] = ladron;
        //Debug.Log("Ninguno: " + none);
        namesRoles[2] = none;

        lugar = "la plaza";
        placeWrong = RecoveryPlaceRol(Rol.None);




    }


    public void StartGame()
    {
        pnlLoading.SetActive(false);
        clock.SetActive(true);
        notepad.SetActive(true);
        padlock.SetActive(false);
        teclaNotepad.SetActive(true);
        if (saveDataPreset.isStarted)
        {
            RecoveryObjectGameManager();
            RecoveryDataGame();
            ButtonManager.buttonManager.OnBtSaving();
            playerMovementScript.hasMetTheMayor = true;
            mayorObject.SetActive(false);
        }
        else
        {
            //Meet the mayor
            playerMovementScript.hasMetTheMayor = false;
            StartCoroutine(FirstMayorEncounter());
        }
    }

    private void Update()
    {
        if(mayorObject.activeInHierarchy && Vector3.Distance(mayorObject.transform.position, player.transform.position) > 20)
        {
            mayorObject.SetActive(false);
        }
    }

    public void Detener(Transform position, Transform position2)
    {

        Invoke("EndTheGame", 4f);
        GameObject guard = Instantiate(civilGuard, position);
        guard.GetComponent<ArrestCtrl>().Arrest(position2, position.parent);
        //guard.GetComponent<Animator>().Play("walk");
    }


    //Go to the game over screen when called
    public void EndTheGame()
    {
        //Kill all the dotweens
        DOTween.KillAll();

        //Load the scene
        SceneManager.LoadScene(4);
    }


    //Coroutine => The mayor will move to you and start it's dialog
    private IEnumerator FirstMayorEncounter()
    {
        mayorObject.transform.DOLookAt(new Vector3(
            player.transform.position.x, 
            mayorObject.transform.position.y, 
            player.transform.position.z), 1.25f);

        yield return new WaitForSeconds(1.25f);

        Transform lookTarget = mayorObject.transform.GetChild(0).transform;
        string mensaje = "Gracias por venir, encontraron a la víctima muerta. " +
            "Con desgarros por todo el cuerpo y amputaciones. " +
            "Eran heridas muy salvajes, y aún así, dijeron que no pertecían a ningún animal. " +
            "Encontraron algo en el bosque, quizás tenga que ver con esto. " +
            "Según los análisis, había algo dentro, algo que ya no está. " +
            "Es por eso que estás tú aquí. ¡Encuéntralo!" + " Creo que no hace falta que te lo diga pero no puede salir de aquí se lo mas discreto posible.";
        interactionManager.StartDialogue(mensaje, mayorObject, lookTarget);

        //Enable again the character movement
        playerMovementScript.hasMetTheMayor = true;
    }





    public void RecoveryDataGame()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        //recoveryDataGame = PlayerPrefs.GetString(SaveGame);

        //string[] dataGame = recoveryDataGame.Split(':');

        //DayCur = int.Parse(dataGame[0]);
        DayCur = saveDataPreset.currentDay;

        if(saveDataPreset.playerPosition != Vector3.zero)
        {
            player.gameObject.GetComponent<CharacterController>().enabled = false;
            player.position = saveDataPreset.playerPosition;
            //player.position = new Vector3(float.Parse(dataGame[1]), float.Parse(dataGame[2]), float.Parse(dataGame[3]));
            //player.rotation = new Quaternion(float.Parse(dataGame[4]), float.Parse(dataGame[5]), float.Parse(dataGame[6]), float.Parse(dataGame[7]));
            player.rotation = new Quaternion(saveDataPreset.playerRotation.x, saveDataPreset.playerRotation.y, saveDataPreset.playerRotation.z, saveDataPreset.playerRotation.w);
            player.gameObject.GetComponent<CharacterController>().enabled = true;
            GameObject.FindObjectOfType<TimeController>().hour = saveDataPreset.hourData;
            GameObject.FindObjectOfType<DayNightCycle>().time = saveDataPreset.timeData;




            //Debug.Log("RECUPERANDO");
            for (int i = 0; i < GameObject.FindGameObjectsWithTag("Scooter").Length; i++)
            {
                if(saveDataPreset.scooterPositionData[i] != null)
                {
                    GameObject.FindGameObjectsWithTag("Scooter")[i].transform.position = saveDataPreset.scooterPositionData[i];
                    GameObject.FindGameObjectsWithTag("Scooter")[i].transform.rotation = saveDataPreset.scooterQuaternionData[i];
                }
            }
        }


    }


    public void SpawnNPC()
    {


        presetsList = presetsList.OrderBy(x => Random.value).ToList();






        for (int i = 0; i < presetsList.Count; i++)
        {
            //Debug.Log(posSpawn.transform.GetChild(i).name);
            GetComponent<CreateRolNPCs>().OnBtCreateNPCRol(presetsList[i], posSpawn.transform.GetChild(i).transform.position, posSpawn.transform.GetChild(i).name);
        }

    }

    private void RecoveryObjectGameManager()
    {
        hintsDictionary = GameObject.FindGameObjectWithTag("HintsDictionary");
    }


    //function shuffle(array)
    //{
    //    let currentIndex = array.length, randomIndex;

    //    // While there remain elements to shuffle.
    //    while (currentIndex != 0)
    //    {

    //        // Pick a remaining element.
    //        randomIndex = Math.floor(Math.random() * currentIndex);
    //        currentIndex--;

    //        // And swap it with the current element.
    //        [array[currentIndex], array[randomIndex]] = [
    //          array[randomIndex], array[currentIndex]];
    //    }

    //    return array;
    //}






}
