using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AttackRadius : MonoBehaviour
{
    public SphereCollider collider;
    protected List<iDamageable> Damageables = new List<iDamageable>();
    public int Damage = 1;
    public float AttackDelay = 0.5f;
    public float TowerAttackDelay = 1.0f;

    public delegate void AttackEvent(iDamageable Target);
    public AttackEvent OnAttack;
    protected Coroutine AttackCoroutine;

    public EnemyMovement enemyMovement;
    public EnemyLineOfSightCheck enemyLineOfSightCheck;

    protected virtual void Awake()
    {
        collider = GetComponent<SphereCollider>();
        enemyMovement = GetComponentInParent<EnemyMovement>();
        enemyLineOfSightCheck = GetComponentInParent<EnemyLineOfSightCheck>();
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" )
        {
            enemyMovement.TriggerAttackModeOn();
        }
       if(other.tag == "Tower" )
        {
            enemyMovement.TriggerAttackModeOn();
            InvokeRepeating("TowerAttack", 4, 1);
          
            
        }

        iDamageable damageable = other.GetComponent<iDamageable>();
        if(damageable != null)
        {
            Damageables.Add(damageable);

            if(AttackCoroutine == null)
            {
                AttackCoroutine = StartCoroutine(Attack());
            }
        }
    }

    //protected virtual void OnTriggerStay(Collider other)
    //{
    //    if (other.tag == "Tower") 
    //    {
    //        StartCoroutine(TowerAttack(other));
    //    }
    //}


    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Tower")
        {

            enemyMovement.TriggerAttackModeOff();
        }

        iDamageable damageable = other.GetComponent<iDamageable>();
        if(damageable != null)
        {
            Damageables.Remove(damageable);
            if(Damageables.Count == 0)
            {
                StopCoroutine(AttackCoroutine);
                AttackCoroutine = null;

            }
        }
    }

    protected virtual IEnumerator Attack()
    {
        WaitForSeconds Wait = new WaitForSeconds(AttackDelay);

        yield return Wait;

        iDamageable closestDamageable = null;
        float closestDistance = float.MaxValue;

        while (Damageables.Count > 0)
        {
            for (int i = 0; i < Damageables.Count; i++)
            {
                Transform damageableTransform = Damageables[i].GetTransform();
                float distance = Vector3.Distance(transform.position, damageableTransform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDamageable = Damageables[i];
                }
            }

            if (closestDamageable != null)
            {
                OnAttack?.Invoke(closestDamageable);
                closestDamageable.TakeDamage(Damage);
            }

            closestDamageable = null;
            closestDistance = float.MaxValue;

            yield return Wait;

            Damageables.RemoveAll(DisableDamages);
        }

        AttackCoroutine = null;
    }

    public void TowerAttack(Collider other)
    {
        iDamageable damageable = other.GetComponent<iDamageable>();
        if (damageable != null)
        {
                OnAttack?.Invoke(damageable);
                damageable.TakeDamage(Damage);
        }
        else
        {
            enemyMovement.TriggerAttackModeOff();
            StopCoroutine("TowerAttack");
        }
        
        //WaitForSeconds Wait = new WaitForSeconds(TowerAttackDelay);

        //yield return Wait;


    }

    protected bool DisableDamages(iDamageable damageable)
    {
        return damageable != null && !damageable.GetTransform().gameObject.activeSelf;


    }



}
