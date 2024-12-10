using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource sheepAudio;
    [SerializeField] private AudioSource footStepAudio;
    [SerializeField] private AudioSource wistleAudio;
    [SerializeField] private AudioSource talkAudio;
    [SerializeField] private AudioSource fountainAudio;

    private bool startSheepSound;
    private bool startWhistleSound;

    public AudioSource FootStepAudio { get => footStepAudio; set => footStepAudio = value; }
    public bool StartSheepSound { get => startSheepSound; set => startSheepSound = value; }
    public bool StartWhistleSound { get => startWhistleSound; set => startWhistleSound = value; }
    public AudioSource FountainAudio { get => fountainAudio; set => fountainAudio = value; }

    private void Awake()
    {
        //if the instance is already created
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            //asign the static instance to this object
            Instance = this;
            //mantain it to next scene
            DontDestroyOnLoad(this);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.InDialogue)
        {
            talkAudio.enabled = true;
        }
        else
        {
            talkAudio.enabled = false;
        }

        if (StartSheepSound)
        {
            sheepAudio.Play();
            startSheepSound = false;
        }
        if (StartWhistleSound)
        {
            wistleAudio.Play();
            startWhistleSound = false;
        }

    }
}
