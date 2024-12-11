using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool isPaused = false;
    private bool inDialogue = false;
    private int sheepCounter = 0;
    private float elapsedTime = 0f;


    public static GameManager Instance { get; private set; }
    public bool IsPaused { get => isPaused; set => isPaused = value; }
    public bool InDialogue { get => inDialogue; set => inDialogue = value; }
    public int SheepCounter { get => sheepCounter; set => sheepCounter = value; }

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject fadePanel;
    private TextMeshProUGUI sheepCounterText;
    private TextMeshProUGUI timerText;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI endTime;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Time.timeScale = 1f;
        isPaused = false;
        inDialogue = false;
        SheepCounter = GameObject.FindGameObjectsWithTag("Sheep").Length;
        sheepCounterText = hudPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        timerText = hudPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        
        
    }

    private void Start()
    {
        FadeIn();
    }

    private void Update()
    {
        sheepCounterText.text = "Ovejas restantes: " + sheepCounter;

        if (!IsPaused)
        {
            UpdateTimer();
        }

        if (sheepCounter <= 0 && !endPanel.activeSelf)
        {
            EndGame();
        }
    }

    public void Cancel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TogglePause();
        }
    }

    private void FadeIn()
    {
        fadePanel.GetComponent<Image>().DOFade(0f, 1.5f).OnComplete(() =>
        {
            fadePanel.SetActive(false);
        });
    }


    public void EndGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        hudPanel.SetActive(false);
        endPanel.SetActive(true);
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        endTime.text = "Tiempo: " + $"{minutes:00}:{seconds:00}";
        

    }

    private void UpdateTimer()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = "Tiempo: "+$"{minutes:00}:{seconds:00}";
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;

        if (IsPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        fadePanel.SetActive(true);
        fadePanel.GetComponent<Image>().DOFade(1f, 1.5f).OnComplete(() =>
        {
            SceneManager.LoadScene("MainMenu");
        });
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        fadePanel.SetActive(true);
        fadePanel.GetComponent<Image>().DOFade(1f, 1.5f).OnComplete(() =>
        {
            SceneManager.LoadScene("OutdoorsScene");
        });
        
    }
}