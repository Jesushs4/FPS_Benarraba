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
    [SerializeField] private AudioSource lockpickCorrect;
    [SerializeField] private AudioSource lockpickFail;


    private bool startSheepSound;
    private bool startWhistleSound;

    public AudioSource FootStepAudio { get => footStepAudio; set => footStepAudio = value; }
    public bool StartSheepSound { get => startSheepSound; set => startSheepSound = value; }
    public bool StartWhistleSound { get => startWhistleSound; set => startWhistleSound = value; }
    public AudioSource FountainAudio { get => fountainAudio; set => fountainAudio = value; }
    public AudioSource TalkAudio { get => talkAudio; set => talkAudio = value; }

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

    public void PlayLockpickCorrect()
    {
        lockpickCorrect.Play();
    }

    public void PlayLockpickFail()
    {
        lockpickFail.Play();
    }
}
