using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(SphereCollider))]
public class EnemyLineOfSightCheck : MonoBehaviour
{

    public delegate void GainSightEvent(PlayerStats player);
    public GainSightEvent onGainSight;
    public delegate  void LoseSightEvent(PlayerStats player);
    public LoseSightEvent onLoseSight;

    public CharacterStats currentTarget;
    public AISensor aISensor;
    public EnemyMovement enemyMovement;

    float scanInterval;
    float scanTimer;
    public float scanFrequency = 1.5f;

    private void Start()
    {
        aISensor = GetComponent<AISensor>();
        enemyMovement = GetComponent<EnemyMovement>();
        scanInterval = 1.0f / scanFrequency;
        
    }

    private void Update()
    {
        scanTimer -= Time.deltaTime;
        
            if (scanTimer < 0)
            {
                scanTimer += scanInterval;
                LineOfSight();
            }
       
      
       
    }


    public void LineOfSight()
    {
        // If object count array is greater than zero.
        if(aISensor.Objects.Count > 0 )
        {
            currentTarget = aISensor.Objects[0].GetComponent<CharacterStats>();

            if(aISensor.Objects[0].CompareTag("Player") )
            {
                enemyMovement.HandleGainSight(currentTarget);
            }
            if (aISensor.Objects[0].CompareTag("Tower") || aISensor.Objects[0].CompareTag("HomeBase")) ;
            {
                enemyMovement.GainedSightOfTower(currentTarget);

            }

            //enemyMovement.HandleGainSight(currentTarget);

        }
        else if(aISensor.Objects.Count == 0)
        {
            currentTarget = null;
            enemyMovement.HandleLoseSight(currentTarget);
        }


    }


}

