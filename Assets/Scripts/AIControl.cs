using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour {

    private GameObject[] goalLocations;
    private NavMeshAgent agent;
    Animator anim;

    public float detectionRadius; //Radius AI detects obstacles
    public float fleeRadius; //Distance the AI flees when an obstacle is detected

    void Start() {

        agent = GetComponent<NavMeshAgent>();

        goalLocations = GameObject.FindGameObjectsWithTag("Goal");
        //select a random goal location and set it as the agents destination
        int i = Random.Range(0, goalLocations.Length);
        agent.SetDestination(goalLocations[i].transform.position);

        //Initialize animator component
        anim = GetComponent<Animator>();        
        anim.SetFloat("wOffset", Random.Range(0.0f, 1.0f));
        
        //Reset the agent´s properties to a randomized state
        ResetAgent();
    }

    private void ResetAgent()
    {
        //Randomly scale the movement speed withing a range
        float ms = Random.Range(0.5f, 1f);
        anim.SetFloat("multSpeed", ms); //update  
        agent.speed *= ms;
        anim.SetTrigger("isWalking");
        agent.angularSpeed = 120;
        //agent.ResetPath();
    }

    public void DetectNewObstacle(Vector3 position)
    {
        if (Vector3.Distance(position, transform.position) < detectionRadius)
        {
            Vector3 fleeDirection = (transform.position - position).normalized;
            Vector3 newgoal = transform.position + fleeDirection * fleeRadius;

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(newgoal, path);

            if (path.status != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(path.corners[path.corners.Length - 1]);
                anim.SetTrigger("isRunning");
                agent.speed = 10;
                agent.angularSpeed = 500;
            }

        }
    }


    void Update() {
        if (agent.remainingDistance < 1)
        {
            //ResetAgent();
            if (goalLocations != null) { 
                int i = Random.Range(0, goalLocations.Length);
                agent.SetDestination(goalLocations[i].transform.position);
            }
        }
    }
}