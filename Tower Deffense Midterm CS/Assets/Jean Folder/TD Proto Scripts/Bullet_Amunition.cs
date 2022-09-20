 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Amunition : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;
    
    //Damage
    public int explostionDamage;
    public float explosionRange;
    // LifeTime
    public int maxCollisions;
    public float maxLifeTime;
    public bool explodeOnTouch = true;


    int collisions;
    PhysicMaterial physicMaterial;



    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        //when to explode;
        if(collisions > maxCollisions)
        {
            Explode();
        }

        // Cound doen lifetime
        maxLifeTime -= Time.deltaTime;
        if(maxLifeTime <= 0)
        {
            Explode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Bullet"))
        {
            return;
        }

        collisions++;

        if(collision.collider.CompareTag("Enemy") && explodeOnTouch)
        {
            Explode();
        }
        
    }


    private void Explode()
    {
        if(explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }

        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange);

        foreach(Collider collider in enemies)
        {
            if(collider.tag == "Enemy")
            {

                Damage(collider.transform);
            }
        }

        //for(int i = 0; i < enemies.Length; i++)
        //{
        //    enemies[i].GetComponent<Enemigo>().TakeDamage(explostionDamage);

        //}
        Invoke("Delay", 0.05f);
         
    }


    void Damage(Transform enemyGo)
    {
        Enemigo enemigo = enemyGo.GetComponent<Enemigo>();

        if(enemigo !=null)
        {
            enemigo.TakeDamage(explostionDamage);

        }

    }


    private void Delay()
    {
        Destroy(gameObject);
    }

    public void SetUp()
    {
        physicMaterial = new PhysicMaterial();
        physicMaterial.bounciness = bounciness;
        physicMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        physicMaterial.bounceCombine = PhysicMaterialCombine.Maximum;
        //assign material to collider
        GetComponent<SphereCollider>().material = physicMaterial;

        //set gravity
        rb.useGravity = useGravity;

    }



    //adding this part
    private void OnTriggerEnter(Collider other)
    {
        iDamageable damageable;

        if (other.TryGetComponent<iDamageable>(out damageable))
        {
            damageable.TakeDamage(explostionDamage);

        }

    }

}
