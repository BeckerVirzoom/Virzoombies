using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fat_ZombieAI : MonoBehaviour
{
    public GameObject player;
    public float totalDistance;
    public Vector3 target;
    
    Animator zombieAnimator;

    public bool isDead; ////Once isdead == True zombie will play the death animation, then destroy itself.

    private IEnumerator coroutine;

    //once key has been pressed wait 1 second for the animation to play then
    // destroy the zombie. If not placed here then you create circular dependency;
    IEnumerator WaitAndKill() 
    {

        //lets the death animation run for a second then destroys the "Zombie"
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        yield break;

    }

    void Start()
    {
       
        player = GameObject.Find("VZPlayer");
        
        //starts zombie alive by making it false (True will make it kill itself instantly) 
        isDead = false;
        zombieAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        target = (Vector3)player.transform.position;
        totalDistance = Vector3.Distance(transform.position, player.transform.position);

        //print(totalDistance);
    
        if (totalDistance > 3 & isDead == false)
        {

            zombieAnimator.SetTrigger("idleTrig");
            
        }
            if (totalDistance < 13 & isDead == false)
            {
            
                zombieAnimator.SetTrigger("attackTrig");
            }

            //Sets death animation then calls other scripts to destroy the "Zombie" after it is "Dead"
        if (isDead == true)
        {
            zombieAnimator.SetTrigger("isDead");
            transform.gameObject.GetComponent<BoxCollider>().enabled = false;
            coroutine = WaitAndKill();
            StartCoroutine(coroutine);
        }


   
    }
}
