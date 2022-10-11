using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMover))]
public class EnemyMovement : MonoBehaviour
{
    public Transform player;

    public Transform targeted_HomeBase;

    public EnemyLineOfSightCheck lineOfSightChecker;
    //public AISensor AI_Sensor;
    public NavMeshTriangulation triangulation;
    public float updateRate = 0.01f; // How quickly we recalcula te the targets transform path. 
    private NavMeshAgent agent;
    private AgentLinkMover LinkMover;
    [SerializeField]
    private Animator animator = null;

    public Vector3 PositionDisplacement = new Vector3(.5f, 0, .5f);
    // creating property of the state method, where _state is the old, and value is the new. And 
    public EnemyState DefaultState;
    public EnemyState _oldToNewState;
    public EnemyState State
    {
        get
        {
            return _oldToNewState;
        }
        set
        {
            onStateChange?.Invoke(_oldToNewState, value);
            _oldToNewState = value;

        }
    }

    public delegate void StateChangeEvent(EnemyState oldstate, EnemyState newState);
    public StateChangeEvent onStateChange;
    public float IdleLocationRadius = 4f;
    public float IdleMoveSpeedMultiplier = 0.5f;
    public Vector3[] waypoints = new Vector3[4];
    public Vector3[] waypointMarchingPath; 

    [SerializeField]
    private int WaypoinIndex = 0;

    private const string IsWalking = "IsWalking";
    private const string Jump = "Jump";
    private const string Landed = "Landed";

    private Coroutine FollowCoroutine;


    private float distanceFromTarget;
    public float stoppingDistance = 0.5f;


    public  bool inAttackMode = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        LinkMover = GetComponent<AgentLinkMover>();

        LinkMover.OnLinkStart += HandleLinkStart;
        LinkMover.OnLinkEnd += HandleLinkEnd;

        lineOfSightChecker.onGainSight += HandleGainSight;
        lineOfSightChecker.onLoseSight += HandleLoseSight;

        onStateChange += HandleStateChange;

        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        targeted_HomeBase = GameObject.FindGameObjectWithTag("HomeBase").GetComponent<Transform>();

    }

    private void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        targeted_HomeBase = GameObject.FindGameObjectWithTag("HomeBase").GetComponent<Transform>();

    }


    public void HandleGainSight(CharacterStats _player)
    {
        Debug.Log("Enemy has spotted you!! RUN");
        player = _player.GetComponent<Transform>();
        State = EnemyState.Chase;
    }

    public void HandleLoseSight(CharacterStats player)
    {
        player = null;
        State = DefaultState;
    }

    public void GainedSightOfTower(CharacterStats _player)
    {
        player = _player.GetComponent<Transform>();
        State = EnemyState.HeadToTower;
    }

    public void TriggerAttackModeOn()
    {
        inAttackMode = true;
    }

    public void TriggerAttackModeOff()
    {
        inAttackMode = false;
    }

    private void OnDisable()
    {
        _oldToNewState = DefaultState; // use _state to avoid triggering OnStateChange when recyclin object in the pool
    }

    public void Spawn()
    {
        for (int i = 0; i < waypoints.Length; i ++)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(triangulation.vertices[Random.Range(0, triangulation.vertices.Length)], out hit, 2f, agent.areaMask))
            {
                waypoints[i] = hit.position;
            }
            else
            {
                Debug.LogError("Unable to find position for navmesh new Triangulation vertex!");
            }

        }
        onStateChange?.Invoke(EnemyState.Spawn, DefaultState);

    }

    // Start is called before the first frame update
     void Update()
    {
        //UpdateStates();
        // may need to remove line
        animator.SetFloat("Speed", agent.velocity.magnitude);


        if (!agent.isOnOffMeshLink)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }





    private void HandleLinkStart(OffMeshLinkMoveMethod MoveMethod)
    {
        //animator.SetTrigger(Jump);
        if (MoveMethod == OffMeshLinkMoveMethod.NormalSpeed)
        {
            animator.SetBool(IsWalking, true);
        }
        else if (MoveMethod != OffMeshLinkMoveMethod.Teleport)
        {
            animator.SetTrigger(Jump);
        }
    }

    private void HandleLinkEnd(OffMeshLinkMoveMethod MoveMethod)
    {

        //animator.SetTrigger(Landed);
        if (MoveMethod != OffMeshLinkMoveMethod.Teleport && MoveMethod != OffMeshLinkMoveMethod.NormalSpeed)
        {
            animator.SetTrigger(Landed);
        }

    }



    public void HandleStateChange(EnemyState oldState, EnemyState newstate)
    {
        // if the old state does not equal the new state do the following. 
        if(oldState != newstate)
        {
            // if the FollowCotuoyine does not equle null do the following. 
            if(FollowCoroutine != null)
            {
                //stop the Follow cotoutine. 
                Debug.Log("Stopping the Follow coroutine ");
                StopCoroutine(FollowCoroutine);
            }
            if(oldState == EnemyState.Idle)
            {
                agent.speed /= IdleMoveSpeedMultiplier;

            }
       
            // If attack radius is triggerd 
            //we are staying still to decide what to do next
            switch (newstate)
            {
               
                case EnemyState.Idle:
                    Debug.Log("new state is " + newstate);
                    FollowCoroutine = StartCoroutine(DoIdleMotion());
                    break;

                case EnemyState.Patrol:
                    Debug.Log("new state is " + newstate);
                    FollowCoroutine = StartCoroutine(DoPatrolMotion());
                    break;

                case EnemyState.Chase:
                    Debug.Log("new state is " + newstate);
                    FollowCoroutine = StartCoroutine(FollowTarget());
                        break;

                case EnemyState.WaypointMarch:
                    Debug.Log("new state is " + newstate);
                    FollowCoroutine = StartCoroutine(DoMarchToHomeBase());
                    break;
                case EnemyState.HeadToTower:
                    FollowCoroutine = StartCoroutine(HeadToTower());
                    break;
                case EnemyState.StayStillDecide:
                    FollowCoroutine = StartCoroutine(StayingStillToDecide());
                    break;

            }


        }


    }

    private IEnumerator StayingStillToDecide()
    {
        WaitForSeconds wait = new WaitForSeconds(updateRate);
        while(true)
        {
            if(agent.enabled)
            {
                agent.isStopped = true;

            }

        }

    }


    private IEnumerator DoMarchToHomeBase()
    {
        WaitForSeconds wait = new WaitForSeconds(updateRate);

        while (true)
        {
            if (agent.enabled)
            {
                agent.SetDestination(targeted_HomeBase.transform.position);
            }
            yield return wait;
        }

    }



    private IEnumerator DoIdleMotion()
    {
        WaitForSeconds wait = new WaitForSeconds(updateRate);
        agent.speed *= IdleMoveSpeedMultiplier;

        while(true)
        {
            if (!agent.enabled || !agent.isOnNavMesh)
                {
                yield return wait;

                }
            else if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector2 point = UnityEngine.Random.insideUnitCircle * IdleLocationRadius;
                NavMeshHit hit;

                if(NavMesh.SamplePosition(agent.transform.position + new Vector3(point.x, 0, point.y), out hit, 2f, agent.areaMask))
                {
                    agent.SetDestination(hit.position);

                }

            }

            yield return wait;

        }

    }

    private IEnumerator DoPatrolMotion()
    {
        WaitForSeconds wait = new WaitForSeconds(updateRate);

        yield return new WaitUntil(() => agent.enabled && agent.isOnNavMesh);
        agent.SetDestination(waypoints[WaypoinIndex]);

        while(true)
        {
            if(agent.isOnNavMesh && agent.enabled && agent.remainingDistance <= agent.stoppingDistance)
            {
                WaypoinIndex++;
                if(WaypoinIndex >= waypoints.Length)
                {
                    WaypoinIndex = 0;

                }
                agent.SetDestination(waypoints[WaypoinIndex]);

            }

            yield return wait;
        }

    }

    private IEnumerator HeadToTower()
    {
        WaitForSeconds wait = new WaitForSeconds(updateRate);

        Vector3 targetDirection = player.transform.position - transform.position;
        float viwableAngle = Vector3.Angle(targetDirection, transform.forward);
        distanceFromTarget = Vector3.Distance(player.transform.position, transform.position);

        while (true)
        {
            if (agent.enabled)
            {
                if (inAttackMode)
                {
                    //agent.SetDestination(player.transform.position);
                    agent.isStopped = true;
                    yield return wait;
                }
                if(inAttackMode == false)
                {
                    agent.isStopped = false;
                    if (distanceFromTarget > stoppingDistance)

                    {
                        agent.SetDestination(player.transform.position);


                    }

                }
            }
            yield return wait;
        }

    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(updateRate);

        Vector3 targetDirection = player.transform.position - transform.position;
        float viwableAngle = Vector3.Angle(targetDirection, transform.forward);
        distanceFromTarget = Vector3.Distance(player.transform.position, transform.position);

        while (true)
        {
            if (agent.enabled)
            {
                if(inAttackMode)
                {
                    agent.isStopped = true;
                    yield return wait;
                }
                //Debug.Log("FOLLOWING AND CHASING THIS MOTHERFUCKING PLAYER!!!");
                //agent.SetDestination(player.transform.position);
                if (inAttackMode == false)
                {
                    agent.isStopped = false;
                    if (distanceFromTarget > stoppingDistance)
                    {
                        agent.SetDestination(player.transform.position + PositionDisplacement);

                    }

                }
            }
            yield return wait;
        }

    }


    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < waypoints.Length; i ++)
        {
            Gizmos.DrawWireSphere(waypoints[i], 0.25f);
            if(i+1 < waypoints.Length)
            {
                Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
            }
            else
            {
                Gizmos.DrawLine(waypoints[i], waypoints[0]);
            }
        }
    }


    public void FoundPlayer(Transform player_)
    {
        player = player_;

    }


}
