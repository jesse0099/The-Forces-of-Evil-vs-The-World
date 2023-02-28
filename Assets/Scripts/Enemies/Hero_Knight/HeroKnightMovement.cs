using Assets.Commons;
using Assets.Scripts.Enemies.Hero_Knight;
using Assets.Scripts.Player.Invocations;
using System.Collections.Generic;
using UnityEngine;

public class HeroKnightMovement : MonoBehaviour
{
    public float attackDelay = 0.1f;
    public EnemyStats stats;
    private bool isAttacking;
    private string attackPattern = Literals.HERO_KNIGHT_ANIMATIONS.Attack1.ToString();
    #region Vars
    private Animator animator;
    readonly System.Random rand_attack = new();
    SpriteRenderer sr;
    GameObject deathTarget;

    [SerializeField] public float attackRange = 0.5f;

    private string currentAnimaton;

    #endregion

    private void Start()
    {
        stats = GetComponent<EnemyStats>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        sr.flipX = true;
        stats.canDamage = true;
    }

    private void Update()
    {
        //Death
        if (stats.health <= 0) {
            //GetComponent<Collider2D>().enabled = false; //Drop from scene effect
            var huntingBehavior = GetComponent<Seeker>();
            huntingBehavior.enabled = false; //Disable hunting
            GetComponent<Rigidbody2D>().simulated = false; //No interaction with dynamic bodies
            ChangeAnimationState(Literals.HERO_KNIGHT_ANIMATIONS.Death.ToString());
        }
        if (!isAttacking && stats.health > 0)
            ChangeAnimationState(Literals.HERO_KNIGHT_ANIMATIONS.run.ToString());
    }

    void ChangeAnimationState(string newAnimation)
    {
        if (!newAnimation.Contains("Attack"))
            if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

    void AttackCompleted()
    {
        isAttacking = false;
        stats.canDamage = true;
        attackPattern = $"{Literals.HERO_KNIGHT_ANIMATIONS.Attack}{rand_attack.Next(1, 4)}";
    }

    void AttackLogic(Collision2D collision)
    {
        var tags = new List<string>(collision.gameObject.tag.Split(","));

        if ((tags.Contains("Player") || tags.Contains("Ally")) && !isAttacking && stats.canDamage
            && stats.health > 0)
        {
            //Damagin an ally 
            if (tags.Contains("Ally"))
            {
                var allyStats = collision.gameObject.GetComponent<InvocationStats>();
                //Stop animating attacks on death ally
                if (allyStats.health <= 0) return;

                isAttacking = true;
                stats.canDamage = false;
                //Attack animation
                ChangeAnimationState(attackPattern);

                allyStats.health -= stats.damage;

                if (allyStats.health <= 0)
                {
                    //Avoid death body to cause damage
                    allyStats.damage = 0;

                    Destroy(collision.gameObject, 1f);
                }
            }
            Invoke("AttackCompleted", attackDelay);
        }
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
