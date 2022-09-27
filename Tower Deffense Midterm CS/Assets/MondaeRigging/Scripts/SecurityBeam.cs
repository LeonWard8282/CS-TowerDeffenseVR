using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SecurityBeam : MonoBehaviour
{
    public GameObject[] enemyAI;

    public AudioSource alarmSource;
    public AudioClip alarmClip;

    public GameObject securityDrone;
    public Material beamMaterial;
    public Color beamColor;


    // Start is called before the first frame update
    void Start()
    {
        beamColor = beamMaterial.color;
        enemyAI = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            beamMaterial.color = Color.red;
            WanderingAI wander = securityDrone.GetComponent<WanderingAI>();
            wander.agent.SetDestination(other.transform.position);
            foreach (GameObject enemy in enemyAI)
            {
                FollowAI followAI = enemy.GetComponent<FollowAI>();
                followAI.targetTransform = other.gameObject.transform;
                followAI.maxFollowDistance = 500;
                followAI.agent.speed = 3;
                followAI.agent.SetDestination(followAI.targetTransform.position);
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {
        beamMaterial.color = beamColor;
        WanderingAI wander = securityDrone.GetComponent<WanderingAI>();
        wander.agent.SetDestination(RandomNavSphere(transform.position, wander.wanderRadius, -1));
        foreach (GameObject enemy in enemyAI)
        {
            FollowAI followAI = enemy.GetComponent<FollowAI>();
            followAI.maxFollowDistance = 5;
            followAI.agent.speed = 1;
            followAI.currentWaypoint = (0 + Random.Range(0, 6)) % followAI.waypoints.Length;
            followAI.agent.SetDestination(followAI.waypoints[followAI.currentWaypoint].position);
        }

    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
