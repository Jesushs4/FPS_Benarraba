using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class TimeController : MonoBehaviour
{
    public float timeDay; //Momento en el que empezo el dia
    private float timeLastSave; //Ultimo momento en el que se guardo
    public bool transition;
    public float delaySave; //Tiempo que tiene que pasar para volver a autoguardar 
    public float fullDayLength; //Tiempo que dura el día
    public GameObject BDead;
    public Animator animator;
    public TextMeshProUGUI TxtMuerto;
    private string muerto;
    private GameObject seHaMuerto;
    public Transform PosInicial;
    public GameObject camNight;

    private bool muerte;
    bool canGoToNight = true;

    //public TextMeshProUGUI timeText;

    public float hour = 420;
    public Image minuteNeedle;
    public Image hourNeedle;
    private void Start()
    {
        timeLastSave = Time.timeSinceLevelLoad;
        timeDay = Time.timeSinceLevelLoad;
        muerte = false;

    }

    private void FixedUpdate()
    {
        PassTime();
        //Si el tiempo actual menos el ultimo momento en el que se guardo es mayor que el tiempo que tiene que pasar para el autoguardado, se autoguarda
        if (Time.timeSinceLevelLoad - timeLastSave > delaySave)
        {
            ButtonManager.buttonManager.OnBtSaving();
            timeLastSave = Time.timeSinceLevelLoad;
        }

        //Si el tiempo actual menos el momento en el que empezo el dia es mayor que el tiempo que dura el dia cambiamos de dia
        if(Time.timeSinceLevelLoad - timeDay > fullDayLength)
        //if(Time.timeSinceLevelLoad - timeDay > fullDayLength)
        {
            camNight.SetActive(false);
            transition = false;
            Time.timeScale = 1;
            ChangeDay();
            MurderNPC();
            Dead();
            Invoke("DeadDeasactivate", 8);
            GameObject.FindGameObjectWithTag("Player").transform.position = PosInicial.transform.position; //GameManager.gameManager.mayorObject.transform.position;
        }

        //Debug.Log(Time.time - timeDay);
        //Debug.Log(Time.timeSinceLevelLoad - timeDay + "/" + fullDayLength);



        if(Time.timeSinceLevelLoad - timeDay >= fullDayLength/2)
        {
            //if (canGoToNight)
            //{
                camNight.transform.GetChild(1).GetComponent<Animator>().SetTrigger("isNight");
                canGoToNight = false;
            //}
                
            //Debug.Log("Transición");
            transition = true;
            camNight.SetActive(true);
            //Time.timeScale = 1;
            Time.timeScale = 25;
        }
        else
        {
            IsDay();
        }

    }

    private void Dead()
    {

        if (muerte)
        {
            TxtMuerto.SetText("Esta noche ha muerto " + muerto);
        }
        else
        {
            TxtMuerto.SetText("Esta noche no ha muerto nadie");
        }
        BDead.SetActive(true);
        transition = true;
        animator.Play("BackgroundDead");
    }

    private void DeadDeasactivate()
    {

        BDead.SetActive(false);
        transition = false;
        muerte = false;

    }

    private void IsDay()
    {
        canGoToNight = true;
    }

    //Restablece el tiempo del día y suma el día que ha pasado
    private void ChangeDay()
    {
        GameManager.gameManager.score -= 5000;
        GameManager.gameManager.DayCur++;
        GameManager.gameManager.saveDataPreset.currentDay = GameManager.gameManager.DayCur;
        timeDay = Time.timeSinceLevelLoad;
    }
    private void PassTime()
    {
        hour += Time.deltaTime * 2.4f;
        //Debug.Log(hour);
        float hours = hour / 60;
        float minutes = hour % 60;
        if (hours >= 24)
        {
            hour = 0;
        }

        minuteNeedle.transform.rotation = Quaternion.Euler(0, 0, -minutes * (360 / 60));
        hourNeedle.transform.rotation = Quaternion.Euler(0, 0, -hours * (360 / 12));

        /*int hourss = (int)hours; //Para reloj digital
        timeText.text = hourss.ToString("00") + ":" + minutes.ToString("00");*/
    }
    //Devuelve el tiempo que ha pasado a lo largo del dia
    public float TimeDay()
    {
        float time = (Time.timeSinceLevelLoad - timeDay) / fullDayLength;

        if(time >= 1f)
        //if(time >= 1f)
        {
            timeDay = Time.timeSinceLevelLoad;
        }

        //Debug.Log(time);
        return time;
    }

    public void GetMurdered()
    {
        muerte = true;
        muerto = seHaMuerto.GetComponent<NPC>().preset.nameNPC;
        Debug.Log(muerto);
        seHaMuerto.transform.gameObject.SetActive(false);
    }

    private void MurderNPC()
    {
        switch (GameManager.gameManager.DayCur)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                for (int i = 0; i < GameObject.FindGameObjectsWithTag("NPC").Length; i++)
                {
                    if (GameObject.FindGameObjectsWithTag("NPC")[i].GetComponent<NPC>().preset.characterRol == Rol.None)
                    {

                        seHaMuerto = GameObject.FindGameObjectsWithTag("NPC")[i];
                        GetMurdered();
                        i = GameObject.FindGameObjectsWithTag("NPC").Length;
                    }
                }

                break;
            case 5: //Muerte buen samaritano
                for (int i = 0; i < GameObject.FindGameObjectsWithTag("NPC").Length; i++)
                {
                    if (GameObject.FindGameObjectsWithTag("NPC")[i].GetComponent<NPC>().preset.characterRol == Rol.BuenSamaritano)
                    {

                        seHaMuerto = GameObject.FindGameObjectsWithTag("NPC")[i];
                        GetMurdered();
                        i = GameObject.FindGameObjectsWithTag("NPC").Length;
                    }
                }
                break;
            case 6: //Muerte mentiroso
                for (int i = 0; i < GameObject.FindGameObjectsWithTag("NPC").Length; i++)
                {
                    if (GameObject.FindGameObjectsWithTag("NPC")[i].GetComponent<NPC>().preset.characterRol == Rol.Mentiroso)
                    {

                        seHaMuerto = GameObject.FindGameObjectsWithTag("NPC")[i];
                        GetMurdered();
                        i = GameObject.FindGameObjectsWithTag("NPC").Length;
                    }
                }
                break;
            case 7: //Muerte ladrón
                for (int i = 0; i < GameObject.FindGameObjectsWithTag("NPC").Length; i++)
                {
                    if (GameObject.FindGameObjectsWithTag("NPC")[i].GetComponent<NPC>().preset.characterRol == Rol.Ladrón)
                    {

                        seHaMuerto = GameObject.FindGameObjectsWithTag("NPC")[i];
                        GetMurdered();
                        i = GameObject.FindGameObjectsWithTag("NPC").Length;
                    }
                }
                break;
            case 8:
                break;
            default:
                break;
        }
    }

}
