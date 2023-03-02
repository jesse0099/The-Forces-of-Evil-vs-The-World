using Assets.Commons;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Vars
    private Animator animator;
    private Rigidbody2D rb2d;
    public Transform attackPoint;
    SpriteRenderer sr;
    public LayerMask enemyLayers;
    PlayerStats stats;
    public AudioSource attackAudio;
    public AudioSource summonAudio;
    public AudioSource swingAudio;

    private float xAxis;
    private float yAxis;

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float jumpForce = 600;
    [SerializeField] public float attackRange = 0.5f;
    [SerializeField] private float attackDelay = 0.3f;
    [SerializeField] private float summonDelay = 0.3f;
    [SerializeField] private float damageDelay = 0.6f;

    private string currentAnimaton;

    private bool isGrounded;
    private bool isSummonPressed;
    private bool isSummoning;
    private bool isAttackPressed;
    private bool isAttacking;
    private bool isJumpPressed;
    private bool isBeingHurted;


    enum ANIMATOR_PARAMS
    {
        playerIsBeingHurted
    }

    #endregion

    private void Awake()
    {
        stats = GameObject.Find("player").GetComponent<PlayerStats>();
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //Checking for inputs
        xAxis = Input.GetAxisRaw("Horizontal");

        //space jump key pressed?
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumpPressed = true;
        }

        //space Atatck key pressed?
        if (Input.GetKeyDown(KeyCode.F))
        {
            isAttackPressed = true;
        }

        //Summon key pressed?
        if (Input.GetKeyDown(KeyCode.V))
        {
            isSummonPressed = true;
        }
    }

    // Physics based time step loop
    private void FixedUpdate()
    {
        //isGrounded = IsGrounded();
        isGrounded = IsOnGround.isGrounded;

        //Check update movement based on input
        Vector2 vel = new(0, rb2d.velocity.y);

        if (xAxis < 0 && !isBeingHurted)
        {
            vel.x = -walkSpeed;
            transform.localScale = new Vector2(-1, 1);
            sr.flipX = false;
        }
        else if (xAxis > 0 && !isBeingHurted)
        {
            vel.x = walkSpeed;
            transform.localScale = new Vector2(1, 1);
            sr.flipX = false;
        }
        else
            vel.x = 0;

        if (isGrounded && !isAttacking && !isBeingHurted && !isSummoning)
        {
            if (xAxis != 0)
                ChangeAnimationState(Literals.PLAYER_ANIMATIONS.run.ToString());
            else
                ChangeAnimationState(Literals.PLAYER_ANIMATIONS.idle.ToString());
        }

        //------------------------------------------

        //Check if trying to jump 
        if (isJumpPressed && isGrounded && !isBeingHurted)
        {
            rb2d.AddForce(new Vector2(0, jumpForce));
            isJumpPressed = false;
            ChangeAnimationState(Literals.PLAYER_ANIMATIONS.jump.ToString());
        }

        //Check if trying to summon
        if (isSummonPressed)
        {
            isSummonPressed = false;

            if (!isSummoning)
            {
                isSummoning = true;

                ChangeAnimationState(Literals.PLAYER_ANIMATIONS.summon.ToString());
                summonAudio.Play();
                Invoke("SummonComplete", summonDelay);
            }
        }

        //assign the new velocity to the rigidbody
        rb2d.velocity = vel;


        //attack
        if (isAttackPressed)
        {
            isAttackPressed = false;

            if (!isAttacking)
            {
                isAttacking = true;

                if (isGrounded)
                    ChangeAnimationState(Literals.PLAYER_ANIMATIONS.attack.ToString());
                else
                    ChangeAnimationState(Literals.PLAYER_ANIMATIONS.attack.ToString()); // Future air attack
                                    
                AttackingLogic();
                swingAudio.Play();
                Invoke("AttackComplete", attackDelay);
            }
        }
    }

    #region Custom Methods
    void AttackingLogic()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            var enemyStats = enemy.GetComponent<EnemyStats>();
            enemyStats.health -= stats.damage;
            attackAudio.Play();
            /* Logica de da�ar Enemigos */
            Debug.Log("hit");
        }
    }

    void AttackComplete()
    {
        isAttacking = false;
    }

    void SummonComplete()
    {
        isSummoning = false;
    }

    void HurtComplete()
    {
        isBeingHurted = false;
        //Change animation to jump when the player is mid air
        if (!isGrounded)
            ChangeAnimationState(Literals.PLAYER_ANIMATIONS.jump.ToString());
    }

    void HurtBegin()
    {
        isBeingHurted = true;
    }

    //=====================================================
    // mini animation manager
    //=====================================================
    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }
    #endregion

    #region Events 
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        var tags = new List<string>(collision.gameObject.tag.Split(","));

        if (tags.Contains("Enemy") || tags.Contains("Spike") && !isBeingHurted)
        {
            HurtBegin();
            //var enemyStats = collision.gameObject.GetComponent<EnemyStats>();
            //stats.Taking_Damage(enemyStats);

            Vector2 hitted_on = (collision.transform.position - transform.position).normalized;
            rb2d.position -= new Vector2(hitted_on.x, hitted_on.y);

            ChangeAnimationState(Literals.PLAYER_ANIMATIONS.hurt.ToString());

            Invoke("HurtComplete", damageDelay);

        }
    }
    #endregion

}
