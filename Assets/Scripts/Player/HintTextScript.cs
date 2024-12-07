using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class HintTextScript : MonoBehaviour
{
    public TextMeshProUGUI hintText;
    [HideInInspector] public bool isStrike = false;


    //Event => Change strike on click
    public void OnClickChangeStrike()
    {
        isStrike = !isStrike;

        if (isStrike) hintText.fontStyle = FontStyles.Strikethrough;
        else hintText.fontStyle = FontStyles.Normal;
    }


    //Set text of the hint
    public void SetHintText(string textToSet)
    {
        hintText.text = textToSet;
    }
}
