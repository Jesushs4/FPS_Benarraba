using UnityEngine;
using UnityEngine.UIElements;

public class MenuAudioManager : MonoBehaviour
{
    public static MenuAudioManager Instance;
    [SerializeField] private AudioSource buttonClicked;





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
            //DontDestroyOnLoad(this);
        }
    }



    public void PlayButton()
    {
        buttonClicked.Play();
    }

}
