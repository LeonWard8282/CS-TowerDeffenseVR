using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public enum Handness
{
    Left, Right
}
public class Gun : MonoBehaviour
{
    private Handness m_hand = Handness.Right;
    private Handness m_hand2 = Handness.Left;
    private string m_ShootButton;
    private string m_laserButton;
    [SerializeField] private GameObject m_prefabBullet;
    [SerializeField] private GameObject m_prefabLaser;
    [SerializeField] private float m_shootForce;
    [SerializeField] private float m_shootForceLaser;
    [SerializeField] private Transform m_spawn;
    [SerializeField] public int bulletLimit;
    [SerializeField] public int LaserLimit;
    [SerializeField] public float m_bulletCooldown = 5f;
    [SerializeField] private Transform m_spawnLaser;
    [SerializeField] public int bulletLimitLaser;
    [SerializeField] public float m_bulletCooldownLaser = 10f;

    public int initialBulletLimit;
    public int initialLaserLimit;
    public AudioSource source;
    public AudioClip fireSound;

    public bool shootingAvailable = true;
    public bool LaserAvailable = true;
    public GameObject reloadingCanvas;
    public GameObject rechargingCanvas;

    public AudioSource reloading;
    public AudioClip reloadingSound;

    public void Start()
    {
        reloadingCanvas.SetActive(false);
        bulletLimit = 15;
        LaserLimit = 100;
        initialBulletLimit = bulletLimit;
        m_ShootButton = "XRI_" + m_hand + "_TriggerButton";
        m_laserButton = "XRI_" + m_hand2 + "_TriggerButton";
    }
    // Start is called before the first frame update
    public void Update()
    {
        if (Input.GetButtonDown(m_ShootButton))
        {
            if (shootingAvailable == true)
            {
                GameObject bullet = Instantiate(m_prefabBullet, m_spawn.position, m_spawn.rotation);
                bulletLimit -= 1;
                if (bulletLimit <= 0)
                {
                    shootingAvailable = false;
                    StartCoroutine(ReloadTimer());
                }
                bullet.GetComponent<Rigidbody>().AddForce(m_shootForce * m_spawn.forward);
                if (!source.isPlaying)
                {
                    source.PlayOneShot(fireSound);
                }
            }
        }

        if(Input.GetButton(m_laserButton))
        {
            if (LaserAvailable == true)
            {
                GameObject laser = Instantiate(m_prefabLaser, m_spawnLaser.position, m_spawnLaser.rotation);
                bulletLimitLaser -= 1;
                if (bulletLimitLaser <= 0)
                {
                    LaserAvailable = false;
                    StartCoroutine(LaserReloadTimer());
                }
                laser.GetComponent<Rigidbody>().AddForce(m_shootForceLaser * m_spawnLaser.forward);
                if (!source.isPlaying)
                {
                    source.PlayOneShot(fireSound);
                }
            }
        }
    }

    private IEnumerator ReloadTimer()
    {
        reloadingCanvas.SetActive(true);
        if (!reloading.isPlaying)
        {
            reloading.Play();
        }
        yield return new WaitForSeconds(m_bulletCooldown);
        reloadingCanvas.SetActive(false);
        reloading.Stop();
        shootingAvailable = true;
        bulletLimit = initialBulletLimit;
    }
    private IEnumerator LaserReloadTimer()
    {
        rechargingCanvas.SetActive(true);
        if (!reloading.isPlaying)
        {
            reloading.Play();
        }
        yield return new WaitForSeconds(m_bulletCooldownLaser);
        rechargingCanvas.SetActive(false);
        reloading.Stop();
        LaserAvailable = true;
        initialLaserLimit = initialBulletLimit;
    }
}
