using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10f;

    private Transform target;
    private int wavepointIndex = 0;


    private void Start()
    {
        target = Waypoints.points[0];
    }

     void Update()
    {

        Vector3 direction = target.position - transform.position; //direction vector
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if(Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWayPoiint();

        }

        


    }

    void GetNextWayPoiint()
    {
        if(wavepointIndex >= Waypoints.points.Length - 1)
        {
            Destroy(gameObject);
            return;

        }

        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];

    }
}
