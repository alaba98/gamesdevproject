using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieIdleState : StateMachineBehaviour
{
  

    float timer;
    public float idleTime = 0f;
    Transform player;
    [SerializeField] public float detectionRadius = 10f;//lets enemy know if we are close enough to change states
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;// issues a cooldown period between transition
        if (timer > idleTime)
        {
            animator.SetBool("Patrolling", true);
        }
        //this transitions to patrolling state

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionRadius)
        {
            animator.SetBool("Chasing", true);
        }
        //this transitions to chasing state
    }
}