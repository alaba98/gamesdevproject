using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class zombiePatrolState : StateMachineBehaviour
{
    float timer;
    [SerializeField] public float patrolTime = 15f;
    Transform player;
    NavMeshAgent navAgent;
    [SerializeField] public float detectionRadius = 10f;
    [SerializeField] public float patrolSpeed = 3f;

    [SerializeField] private AudioClip patrolSound;
    private AudioSource audioSource;
    List<Transform> waypointsList = new List<Transform>();
    //this will determine the waypoints that the zombie will walk between
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = animator.GetComponent<NavMeshAgent>();
        navAgent.speed = patrolSpeed;
        audioSource = animator.GetComponent<AudioSource>();
        timer = 0; //reset the timer

        GameObject waypoints = GameObject.FindGameObjectWithTag("Waypoints");
        foreach (Transform t in waypoints.transform)
        {
            waypointsList.Add(t);
        }
        Vector3 nextPos = waypointsList[Random.Range(0, waypointsList.Count)].position;
        navAgent.SetDestination(nextPos);
        //we get a reference to all of the  waypoints,loop through the children
        //generate a random number from 0 to waypointcount which is returned

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (audioSource.isPlaying == false) // if the sound is not already playing, it will play it with a delay
        {
            //audioSource.Stop();
            audioSource.clip = patrolSound;
            audioSource.PlayDelayed(0.2f);
        }
        if (navAgent.remainingDistance <= navAgent.stoppingDistance)// this property being true means that the navagent has reached/very close to destination
        {
            navAgent.SetDestination(waypointsList[Random.Range(0, waypointsList.Count)].position);
            //sets a new random destination using list of waypoints
        }

        timer += Time.deltaTime;
        if(timer > patrolTime)
        {
            animator.SetBool("Patrolling",false);
            //transition back to idle state once patrolling has completed
        }

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionRadius)
        {
            animator.SetBool("Chasing", true);
            audioSource.Stop();
        }
        //this transitions to chasing state
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        
        navAgent.SetDestination(navAgent.transform.position);
        //sets current position to final position, thus stopping movement

    }
}
