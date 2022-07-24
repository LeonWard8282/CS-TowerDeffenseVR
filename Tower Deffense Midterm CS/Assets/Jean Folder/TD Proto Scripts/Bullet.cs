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

    }

    void HitTarget()
    {
        //casting the impact effect onto the bullet and then deleting it after 2seconds
        GameObject effectInstance = (GameObject) Instantiate(impactEffect, transform.position, transform.rotation);
        //Destroy the effect particle
        Destroy(effectInstance, 1f);
        // Distroying enemy
        Destroy(target.gameObject);
        //Destroy bullet
        Destroy(gameObject);
    }


}
