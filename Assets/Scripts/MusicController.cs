using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicController : MonoBehaviour
{
    public Transform positionGuayacan;
    public Transform positionBarroso;
    private Transform positionPlayer;
    public float maxDistance = 20f;
    public float maxVolume = 0.2f;

    public AudioSource musicAmbient;
    public AudioSource musicBar;

    public AudioClip guayacanMusic;
    public AudioClip barrosoMusic;

    public AnimationCurve curveVolume;

    private float volumenMusic;
    private float preVolumenMusic;

    private bool downVolumenGuayacan;
    private bool downVolumenBarroso;

    private void Start()
    {
        preVolumenMusic = volumenMusic;
        positionPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        volumenMusic = GameManager.gameManager.saveDataPreset.volumen;

        Debug.Log(volumenMusic);
    }


    private void Update()
    {
        float distanceGuayacan = Vector2.Distance(new Vector2(positionGuayacan.transform.position.x, positionGuayacan.transform.position.z),
            new Vector2(positionPlayer.transform.position.x, positionPlayer.transform.position.z));     
        float distanceBarroso = Vector2.Distance(new Vector2(positionBarroso.transform.position.x, positionBarroso.transform.position.z),
            new Vector2(positionPlayer.transform.position.x, positionPlayer.transform.position.z));

        //Debug.Log("BARROSO: "+distanceBarroso);


        //If the distance is in the curve, start evaluating the sound parameter
        if (distanceGuayacan <= maxDistance)
        {
            //musicBar.volume = maxVolume - (distanceGuayacan);
            if (!musicBar.isPlaying) musicBar.PlayOneShot(guayacanMusic);
            musicBar.loop = true;
            musicBar.volume = curveVolume.Evaluate(preVolumenMusic / distanceGuayacan);
            Debug.Log(musicBar.volume+":"+ musicAmbient.volume);
            downVolumenGuayacan = false;
        }
        else
        {
            downVolumenGuayacan = true;
        }


        if (distanceBarroso <= maxDistance)
        {
            //musicBar.volume = maxVolume - (distanceBarroso);
            if (!musicBar.isPlaying) musicBar.PlayOneShot(barrosoMusic);
            musicBar.loop = true;
            musicBar.volume = curveVolume.Evaluate(preVolumenMusic / distanceBarroso);

            Debug.Log("PPA: "+ curveVolume.Evaluate(preVolumenMusic / distanceBarroso));
            downVolumenBarroso = false;
        }
        else 
        {
            downVolumenBarroso = true;
        }


        if (downVolumenGuayacan && downVolumenBarroso)
        {
            musicBar.volume = 0;
        }


        Debug.Log(musicBar.volume + ":" + musicAmbient.volume);

        musicAmbient.volume = maxVolume - musicBar.volume * 4 * preVolumenMusic;
    }
}
