using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverSceneScript : MonoBehaviour
{

    public TextMeshProUGUI scoreText;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        SetScore();
    }


    //Get the score from the game manager
    public void SetScore()
    {
       scoreText.text = "Puntuación: " + GameManager.gameManager.score;
    }
}
