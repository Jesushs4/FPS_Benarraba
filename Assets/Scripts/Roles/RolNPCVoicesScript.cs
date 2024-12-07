using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RolNPCVoicesScript : MonoBehaviour
{
    public AudioClip[] npcAudios;
    private AudioSource npcAudioSource;
    private AudioClip previousPlayedVoice;


    //Start
    private void Start()
    {
        //Get the AudioSource component from the npc
        npcAudioSource = GetComponent<AudioSource>();
        
    }


    //Play a voice when entering the chat
    public void PlayVoice()
    {
        npcAudioSource.volume = GameManager.gameManager.saveDataPreset.volumenVoice;
        int randomVoice;

        //Choose a random voice that's different from the previous one
        if (npcAudios.Length > 1)
        {
            do
            {
                randomVoice = Random.Range(0, npcAudios.Length);
            } while (npcAudios[randomVoice] == previousPlayedVoice);
        }

        //If only has 1 audio
        else randomVoice = 0;

        npcAudioSource.clip = npcAudios[randomVoice];
        npcAudioSource.Play();

        //Save as backup
        previousPlayedVoice = npcAudios[randomVoice];
    }

    public void StopVoice()
    {
        npcAudioSource.Stop();
    }
}
