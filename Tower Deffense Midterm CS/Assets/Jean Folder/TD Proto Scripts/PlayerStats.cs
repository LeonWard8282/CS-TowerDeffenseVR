using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, iDamageable
{
    [SerializeField]
    private AttackRadius attackRadius;
    //possible future hand animation???
    //[SerializeField]
    //private Animator animator;
    


    public static int Money;
    public int startMoney = 400;

    public static int Lives;
    public int startLives = 3;

    [SerializeField]
    public int health = 300;

    public static int Rounds;

    private void Start()
    {
        Money = startMoney;
        Lives = startLives;

        Rounds = 0;
    }

    public void TakeDamage(int Damage)
    {
        health -= Damage;
        if(health <= 0 )
        {
            // TODO: Remove Life, respwan at base, Penalty?

        }

        throw new System.NotImplementedException();
    }

    public Transform GetTransform()
    {

        return transform;
    }
}
