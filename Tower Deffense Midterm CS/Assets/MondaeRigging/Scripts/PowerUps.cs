using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PowerUps : MonoBehaviour
{
    public GameObject shieldObject;
    public GameObject shieldText;

    public PlayerStats playerStats;
    //public CharacterStats playerStats;

    private GameObject shieldPowerUp;
    private GameObject healthPowerUp;

    private bool shieldActive;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HealthRegen());
        shieldActive = false;
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
            shieldPowerUp = other.gameObject;
            shieldActive = true;
        }

        if (other.CompareTag("HealthPowerUp"))
        {
            StartCoroutine(HealthPowerUp());
            healthPowerUp = other.gameObject;
        }

        if(other.CompareTag("Enemy)") && shieldActive == false)
        {
            playerStats.currentHealth -= 10;
        }
    }

    private IEnumerator HealthRegen()
    {
        while (shieldObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(1);
            playerStats.currentHealth++;
        }
    }

    IEnumerator ShieldPowerUp()
    {
        yield return new WaitForSeconds (0);
        shieldObject.SetActive (true);
        shieldText.SetActive(true);
        Destroy(shieldPowerUp);
        yield return new WaitForSeconds(3);
        shieldObject.GetComponent<MeshRenderer>().enabled = false;

        yield return new WaitForSeconds (15);
        shieldObject.SetActive(false);
        shieldText.SetActive(false);
        shieldObject.GetComponent<MeshRenderer>().enabled = true;
        shieldActive = false;
    }    
    IEnumerator HealthPowerUp()
    {
        yield return new WaitForSeconds(0);
        playerStats.currentHealth += 50;
        Destroy(healthPowerUp);
    }
}
