using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public GameObject player;
    public float totalDistance;
    public NavMeshAgent agent;
    public Vector3 target;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); 
        player = GameObject.Find("VZPlayer");
    }

    void Update()
    {
        target = (Vector3)player.transform.position;
        totalDistance = Vector3.Distance(transform.position, player.transform.position);
        print(totalDistance);
        if (totalDistance < 25)
        {
            gameObject.GetComponent<Renderer> ().material.color = Color.red;
          //  destination = target.position;
            agent.destination = target;
            if (totalDistance <5)
            {
                gameObject.SetActive(false);
            }
        }
    }
}