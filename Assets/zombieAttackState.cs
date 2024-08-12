using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class zombieAttackState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent navAgent;
    [SerializeField] public float stopAttacking = 1.5f ;

    private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = animator.GetComponent<NavMeshAgent>();
        audioSource = animator.GetComponent<AudioSource>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (audioSource.isPlaying == false) // if the sound is not already playing, it will play it with a delay
        {
            //audioSource.Stop();
            audioSource.PlayOneShot(attackSound);
        }

        LookAtPlayer();

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        //move the enemy to position of player and make itface the player's way

        if (distanceFromPlayer > stopAttacking)
        {
            animator.SetBool("Attacking", false);
            audioSource.Stop();
            //stop attacking the player
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    
    private void LookAtPlayer()
    {
        Vector3 direction = player.position - navAgent.transform.position;
        //Calcs direction vector from navagents pos to players pos
        navAgent.transform.rotation = Quaternion.LookRotation(direction);
        // creates rotation that points in direction of 1st line
        var Yrotation = navAgent.transform.eulerAngles.y;
        navAgent.transform.rotation = Quaternion.Euler(0,Yrotation,0);
        //makes it so that navagent only rotates around y-axis
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        
    }

}
