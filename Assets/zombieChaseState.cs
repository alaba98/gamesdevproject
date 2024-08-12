using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class zombieChaseState : StateMachineBehaviour
{

    NavMeshAgent navAgent;
    Transform player;
    [SerializeField] public float chaseSpeed = 3f;
    [SerializeField] public float stopChasing = 10f;
    [SerializeField] public float attackingDistance = 1.5f;

    private AudioSource audioSource;
    [SerializeField] private AudioClip chaseSound;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = animator.GetComponent<NavMeshAgent>();
        navAgent.speed = chaseSpeed;
        audioSource = animator.GetComponent<AudioSource>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


        if (audioSource.isPlaying == false) // if the sound is not already playing, it will play it with a delay
        {
           // audioSource.Stop();
            audioSource.PlayOneShot(chaseSound);
        }

        navAgent.SetDestination(player.position);
        animator.transform.LookAt(player);
        
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        //move the enemy to position of player and make itface the player's way

        if (distanceFromPlayer > stopChasing)
        {
            animator.SetBool("Chasing",false);
            //stop chasing the player
        }

        if(distanceFromPlayer < attackingDistance)
        {
            animator.SetBool("Attacking",true);
            audioSource.Stop();
            //attack the player
        }


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        
        navAgent.SetDestination(animator.transform.position);
        //stop the agent from moving
        
    }
}
