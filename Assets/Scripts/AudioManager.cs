using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource powerUpAudio;
    [SerializeField] private AudioSource damageAudio;

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

    public void PlayDamageSound()
    {
        damageAudio.Play();
    }

    public void PlayPowerUpSound()
    {
        powerUpAudio.Play();
    }
}
