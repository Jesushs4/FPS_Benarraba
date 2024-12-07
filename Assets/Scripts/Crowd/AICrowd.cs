using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICrowd : MonoBehaviour
{
    GameObject[] checkpoints;
    NavMeshAgent agent;
    Animator anim;
    public int distanceView=30;

    bool goToPlayer;
    bool closePlayer;
    private bool canEnterCoroutine = true;

    bool seeThePlayer;
    bool activeAnimator;

    float timeCur;
    Vector3 lastPosition;


    private void Start()
    {
        timeCur = Time.time;
        lastPosition = transform.position;

        anim = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        ResetAgent();
    }

    private void Update()
    {
        if (CompareTag("Gossip"))
        {
            if (canEnterCoroutine) StartCoroutine(MovementGossip());
        }
        else
        {
            MovementCrowd();
        }




        if(Time.time > timeCur + 2) //Si pasado dos segundos
        {
            if (Vector3.Distance(lastPosition, transform.position) > 0.5f) //si ha avanzado .5f
            {
                timeCur = Time.time;
                lastPosition = transform.position;
            }
            else //si no lo ha hecho buscamos un nuevo objetivo
            {
                if (GetComponent<Animator>().enabled)
                {
                    ResetAgent();
                    timeCur = Time.time;
                    lastPosition = transform.position;
                }
            }
        }



    }

    private IEnumerator MovementGossip()
    {
        canEnterCoroutine = false;


        //If needs to go to the player
        if (goToPlayer)
        {
            //If the distance to the player is < 1 and no path is pending, go to a point
            if (agent.remainingDistance < 1.5f && !agent.pathPending)
            {
                goToPlayer = false;
                yield return new WaitForSeconds(3f);
                agent.SetDestination(checkpoints[Random.Range(0, checkpoints.Length)].transform.position);
            }
            //If it's far from the player, travel to it
            else
            {
                agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
            }
        }

        //If needs to go far from player
        else
        {
            //If the distance to the random point is < 1 and no path is pending, go to the player
            if (agent.remainingDistance < 1 && !agent.pathPending)
            {
                goToPlayer = true;
                agent.SetDestination(checkpoints[Random.Range(0, checkpoints.Length)].transform.position);
            }
        }


        canEnterCoroutine = true;

    }


    private void MovementCrowd()
    {
        if (agent.remainingDistance < 1)
        {
            agent.SetDestination(checkpoints[Random.Range(0, checkpoints.Length)].transform.position);
        }

        //Calcula la distancia entre el jugador  y el npc y si es menor a distanceView dejará de realizar la animación
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > distanceView)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (!seeThePlayer) //Si aún no hemos visto al jugador
                {
                    //y la distancia es superior a 50
                    if(Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > 50)
                    {
                        //Deshabilitamos el animator
                        transform.GetComponent<Animator>().enabled = false;
                    }
                    else
                    {
                        //Si la distancia es menor ya hemos visto al jugador
                        seeThePlayer = true;
                    }
                }
                //Si ya hemos visto al jugador desactivamos al npc
                else
                {
                    if (!activeAnimator)
                    {
                        transform.GetComponent<Animator>().enabled = true;
                        activeAnimator = true;
                    }
                    else
                    {
                        transform.GetChild(i).gameObject.SetActive(false);//Hace que no se vean
                        anim.enabled = false;
                        agent.isStopped = true;
                    }
                }



            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);//Hace que se vean
                anim.enabled = true;
                agent.isStopped = false;
            }
        }
    }

    void ResetAgent()
    {
        //Modifica la velocidad de animación y de velocidad del npc y le otorga un destino
        float speedWalk = Random.Range(.9f, 1.2f);
        anim.SetFloat("walkingOffset", Random.Range(.5f, 1f));
        anim.SetFloat("walkingSpeed", speedWalk);
        agent.speed = 2;
        agent.SetDestination(checkpoints[Random.Range(0, checkpoints.Length)].transform.position);
        agent.speed *= speedWalk;
    }



}
