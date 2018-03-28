using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class alphaAI : MonoBehaviour
{
    public GameObject player;
    public float totalDistance;
    public NavMeshAgent agent;
    public Vector3 target;


    Animator zombieAnimator;
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("VZPlayer");


        zombieAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        target = (Vector3)player.transform.position;
        totalDistance = Vector3.Distance(transform.position, player.transform.position);
        //print(totalDistance);
        if (totalDistance > 75)
        {
      
            zombieAnimator.SetTrigger("idleTrig");
            agent.destination = gameObject.transform.position;
        }
            if (totalDistance <= 75 && totalDistance > 4)
        {
            agent.destination = target;
            zombieAnimator.SetTrigger("runTrig");


            if (totalDistance < 3)
            {
                
                zombieAnimator.SetTrigger("attackTrig");
            }
        }
    }
}