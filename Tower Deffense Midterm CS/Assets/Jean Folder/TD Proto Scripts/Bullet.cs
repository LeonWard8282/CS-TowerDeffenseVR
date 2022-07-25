using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    [Header("Bullet Speed")]
    public float speed = 70f;
    [Header("Bullet Impact VFX")]
    public GameObject impactEffect;
    [Header("Bullet Explosion Radius")]
    public float explosionRadius = 0f;
    [Header("Bullet Damage")]
    public int damage = 50;


    public void Seek(Transform _target)
    {
        target = _target;

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);


    }


    void HitTarget()
    {
        //casting the impact effect onto the bullet and then deleting it after 2seconds
        GameObject effectInstance = (GameObject) Instantiate(impactEffect, transform.position, transform.rotation);
        //Destroy the effect particle
        Destroy(effectInstance, 5f);

        if(explosionRadius > 0f)
        {
            Explode();

        }
        else
        {
            Damage(target);
        }


        //Destroy bullet
        Destroy(gameObject);
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider collider in colliders)
        {
            if(collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemyGO)
    {
        //enemymovement is enemy type in script.... TODO :Fix this
        EnemyMovement e = enemyGO.GetComponent<EnemyMovement>();

        if (e!= null)
        {
            e.TakeDamage(damage);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
