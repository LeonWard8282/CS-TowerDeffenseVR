using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMover))]
public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float updateSpeed = 0.1f; // How quickly we recalcula te the targets transform path. 
    [SerializeField] Animator animator;
    private NavMeshAgent agent;
    private AgentLinkMover LinkMover;

    private const string IsWalking = "IsWalking";
    private const string Jump = "Jump";
    private const string Landed = "Landed";



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        LinkMover = GetComponent<AgentLinkMover>();


        //LinkMover.OnLinkStart += HandleLinkStart;
        //LinkMover.OnLinkEnd += HandleLinkEnd;
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FollowTarget());
    }

    private void HandleLinkStart()
    {
        //Animator.SetTrigger(Jump);
    }

    private void HandleLinkEnd()
    {

        //Animator.SetTrigger(Landed);
    }


    private IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(updateSpeed);

        while(enabled)
        {
            agent.SetDestination(target.transform.position);

            yield return wait;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
