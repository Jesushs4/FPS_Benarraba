using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.05f;
    private bool isTyping = false;
    private string currentDialogue;
    [SerializeField] private string[] texts;
    private int dialoguePosition = 0;
    private LayerMask playerLayer;
    [SerializeField] private GameObject dialoguePanel;


    private void Awake()
    {
        playerLayer = LayerMask.GetMask("Player");
    }



    public void Talk()
    {
        if (!dialoguePanel.activeSelf)
        {
            ActivePanel();
            GameManager.Instance.InDialogue = true;
        }

        if (!isTyping)
        {
            ManageDialogues();
        }
    }

    /// <summary>
    /// Starts a dialogue
    /// </summary>
    /// <param name="dialogue"></param>
    public void StartDialogue(string dialogue)
    {
        currentDialogue = dialogue;
        StartCoroutine(TypeDialogue());

    }

    /// <summary>
    /// Does the typing dialogue by each letter until finished
    /// </summary>
    /// <returns></returns>
    private IEnumerator TypeDialogue()
    {
        dialogueText.text = string.Empty;
        isTyping = true;
        AudioManager.Instance.TalkAudio.Play();
        foreach (char letter in currentDialogue)
        {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        AudioManager.Instance.TalkAudio.Stop();
    }



    /// <summary>
    /// Activates the dialogue panel and starts dialogue
    /// </summary>
    private void ActivePanel()
    {
        dialoguePanel.SetActive(true);
        
        StartDialogue(texts[dialoguePosition]);
        dialoguePosition++;
        return;
    }

    /// <summary>
    /// Manages the dialogue array
    /// </summary>
    private void ManageDialogues()
    {
        if (texts.Length <= dialoguePosition)
        {
            dialoguePosition = 0;
            dialoguePanel.SetActive(false);
            GameManager.Instance.InDialogue = false;
            return;
        }

        dialogueText.text = "";
        StartDialogue(texts[dialoguePosition]);
        dialoguePosition++;
    }

}
