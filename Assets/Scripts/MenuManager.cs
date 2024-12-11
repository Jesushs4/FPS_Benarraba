using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject fadePanel;

    private void Start()
    {
        FadeIn();
    }

    public void StartGame()
    {
        fadePanel.SetActive(true);
        fadePanel.GetComponent<Image>().DOFade(1f, 1.5f).OnComplete(() =>
        {
            SceneManager.LoadScene("OutdoorsScene");
        });
    }

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void FadeIn()
    {
        fadePanel.GetComponent<Image>().DOFade(0f, 1.5f).OnComplete(() =>
        {
            fadePanel.SetActive(false);
        });
    }
}
