using Assets.Scripts.Enemies;
using System.Collections.Generic;
using UnityEngine;

public class SpikeMovement : MonoBehaviour
{
    public EnemyStats stats;
    public float attackDelay = 0.1f;
    private bool isAttacking;

    private void Start()
    {
        stats = GetComponent<EnemyStats>();
        stats.canDamage = true;
        isAttacking = false;
    }

    void AttackLogic(Collision2D collision)
    {
        var tags = new List<string>(collision.gameObject.tag.Split(","));

        if (tags.Contains("Player") && !isAttacking && stats.canDamage)
        {
            var playerStats = collision.gameObject.GetComponent<PlayerStats>();
            
            isAttacking = true;
            stats.canDamage = false;
          
            playerStats.Taking_Damage(stats);

            Invoke("AttackCompleted", attackDelay);
        }
    }

    void AttackCompleted()
    {
        isAttacking = false;
        stats.canDamage = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        AttackLogic(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AttackLogic(collision);
    }
}
