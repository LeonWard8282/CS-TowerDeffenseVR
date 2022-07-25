using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10f;

    private Transform target;
    private int wavepointIndex = 0;

    public int health = 100;
    public int moneyGained = 50;


    //public GameObject deathEffect;

    private void Start()
    {
        target = Waypoints.points[0];
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if(health <=0)
        {
            Die();
        }

    }

    private void Die()
    {
        //GameObject death_Effect = (GameObject) Instantiate(deathEffect, transform.position, Quaternion.identity);
        //Destroy(death_Effect, 5f);
        PlayerStats.Money += moneyGained;
        Debug.Log("Money gained1");
        Destroy(gameObject);
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
            EndPath();
            return;

        }

        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];

    }

    void  EndPath()
    {
        PlayerStats.Lives--;
        Destroy(gameObject);
    }


}
