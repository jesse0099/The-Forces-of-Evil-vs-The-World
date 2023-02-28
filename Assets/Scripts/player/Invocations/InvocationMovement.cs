using Assets.Scripts.Player.Invocations;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemies.Hero_Knight
{
    public class InvocationMovement : MonoBehaviour
    {

        public InvocationStats stats;
        public float attackDelay = 0.1f;

        private Animator animator;
        private bool isAttacking;
        private string currentAnimaton;

        private void Start()
        {
            stats = GetComponent<InvocationStats>();
            animator = GetComponent<Animator>();
            stats.canDamage = true;
            isAttacking = false;
        }

        private void Update()
        {
            //Death
            if (stats.health <= 0) {
                //GetComponent<Collider2D>().enabled = false; //Drop from scene effect
                var huntingBehavior = GetComponent<Seeker>();  //Disable hunting
                huntingBehavior.enabled = false;
                GetComponent<Rigidbody2D>().simulated = false;//No interaction with dynamic bodies
                ChangeAnimationState(stats.deathAnimation);
            }
        }

        void AttackLogic(Collision2D collision)
        {
            var tags = new List<string>(collision.gameObject.tag.Split(","));

            if (tags.Contains("Enemy") && !isAttacking && stats.canDamage && 
                stats.health > 0)
            {
                var enemyStats = collision.gameObject.GetComponent<EnemyStats>();
                //Stop animating attacks on death enemy
                if (enemyStats.health <= 0) return;

                isAttacking = true;
                stats.canDamage = false;
                //An Attack animation
                animator.SetLayerWeight(0, 1);
                animator.SetLayerWeight(1, 1); //Layer where attack animation reside


                enemyStats.health -= stats.damage;

                if (enemyStats.health <= 0)
                {
                    //Avoid death body to cause damage
                    enemyStats.damage = 0;

                    Destroy(collision.gameObject, 1.3f);
                }

                Invoke("AttackCompleted", attackDelay);
            }
        }

        void AttackCompleted()
        {
            isAttacking = false;
            stats.canDamage = true;

            animator.SetLayerWeight(0, 1);
            animator.SetLayerWeight(1, 0); // Turning off attack animation
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            AttackLogic(collision);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            AttackLogic(collision);
        }

        public void ChangeAnimationState(string newAnimation)
        {
            if (currentAnimaton == newAnimation) return;

            animator.Play(newAnimation);
            currentAnimaton = newAnimation;
        }
    }
}