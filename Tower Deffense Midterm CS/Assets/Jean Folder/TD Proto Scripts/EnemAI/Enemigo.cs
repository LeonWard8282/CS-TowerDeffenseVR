using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : PoolableObject , iDamageable
{
    public AttackRadius AttackRadius;
    public Animator Animator;

    public EnemyMovement movement;
    public NavMeshAgent agent;
    public int health = 100;

    public GameObject deathEffect;
    public int moneyGained = 50;

    private Coroutine lookCoroutine;
    private const string Attack_Trigger = "Attack";

    public delegate void DeathEvent (Enemigo enemy);
    public DeathEvent OnDie;

    private void Awake()
    {
        AttackRadius.OnAttack += OnAttack;
    }

    private void OnAttack(iDamageable target)
    {
        Animator.SetTrigger(Attack_Trigger);
        Debug.Log("Animator Trigger should have ran on attack of enemigo script ran. ");
      if(lookCoroutine != null)
        {

            StopCoroutine(lookCoroutine);
        }
        lookCoroutine = StartCoroutine(LookAt(target.GetTransform()));

    }

    private IEnumerator LookAt(Transform target)
    {
        Quaternion lookRotation  = Quaternion.LookRotation(target.position - transform.position);
        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime * 2;
            yield return null;
        }

        transform.rotation = lookRotation;

    }




    public override void OnDisable()
    {
        base.OnDisable();

        agent.enabled = false;
    }


    public Transform GetTransform()
    {
        return transform;

    }

    public void TakeDamage(int Damage)
    {
        health -= Damage;

        if(health <= 0)
        {
            OnDie?.Invoke(this);

            // Death Sequence
            //instantiate death VFX
            GameObject death_Effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
            //Destroys the death effect
            Destroy(death_Effect, 5f);

            // Linking to wave spawner 
            //WaveSpawner.enemiesAlive--;

            // Link to player stats money 
            //PlayerStats.Money += moneyGained;

            // setting the game object to false. 
            gameObject.SetActive(false);
        }

    }
}
