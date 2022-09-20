using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class FireBulletOnActivate : MonoBehaviour
{
    public GameObject bulletprefab;
    public Transform spawnpoint;
    [SerializeField] LineRenderer lineRenderer;
    public float fireSpeed = 20;
    public AudioSource source;
    public AudioClip clip;

    public Transform gunTransform;
    public float rayCastLength = 100f;

    private XRGrabInteractable grabbable;

    private bool hasHit;
    private RaycastHit hit;
    


    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);

        
    }

    // Update is called once per frame
    void Update()
    {
        var ray = new Ray(origin: spawnpoint.transform.position, spawnpoint.transform.forward );
        RaycastHit updatedHit;
        if (Physics.Raycast(ray, out updatedHit, 100))
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, spawnpoint.transform.position);
            lineRenderer.SetPosition(1, updatedHit.point);

            hit = updatedHit;
        }

        bool hasHit = Physics.Raycast(ray, out updatedHit, rayCastLength);

        ;
    }

    public void FireBullet(ActivateEventArgs arg)
    {


        GameObject spawnedBullet = Instantiate(bulletprefab);
        spawnedBullet.transform.position = spawnpoint.position;
        spawnedBullet.GetComponent<Rigidbody>().velocity = spawnpoint.forward * fireSpeed;
        //Destroy(spawnpoint, 6f);
        //Shoot();
        source.PlayOneShot(clip);


    }

    public void Shoot()
    {

            //GameObject bulletGo = (GameObject) Instantiate(bulletprefab, spawnpoint.position, spawnpoint.rotation);
            //Enemy_Bullet bullet = bulletGo.GetComponent<Enemy_Bullet>();

            //if (hasHit)
            //{

            //    bullet.Seek(hit.transform);

            //}

    }

}
