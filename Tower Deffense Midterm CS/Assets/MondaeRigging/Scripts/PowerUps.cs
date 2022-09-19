using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PowerUps : MonoBehaviour
{
    public GameObject shieldObject;
    public GameObject shieldText;

    public PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HealthRegen());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ShieldPowerUp"))
        {
            StartCoroutine(ShieldPowerUp());
        }

        if (other.CompareTag("HealthPowerUp"))
        {
            StartCoroutine(HealthPowerUp());
        }
    }

    private IEnumerator HealthRegen()
    {
        while (shieldObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(1);
            playerStats.health++;
        }
    }

    IEnumerator ShieldPowerUp()
    {
        yield return new WaitForSeconds (0);
        shieldObject.SetActive (true);
        shieldText.SetActive(true);

        yield return new WaitForSeconds (20);
        shieldObject.SetActive(false);
        shieldText.SetActive(false);
    }    
    IEnumerator HealthPowerUp()
    {
        yield return new WaitForSeconds(0);
        playerStats.health += 100;
    }
}
