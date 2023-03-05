using Assets.Scripts.Enemies;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    public int player_health;
    public int player_mana;
    public int damage;
    Animator animator;

    public HealthBar healthBar;
    public int maxHealth;
    public ManaBar manaBar;
    public int maxMana;

    public Points score;
    public int actualPoints;

    public AudioSource healAudio;
    public AudioSource manaAudio;

    // Start is called before the first frame update
    void Start()
    {
        UnitySystemConsoleRedirector.Redirect();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        //init health
        player_health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        //init mana
        player_mana = maxMana;
        manaBar.SetMaxMana(maxMana);
        //init points
        score.SetPoints(actualPoints);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void TakeDamage(int damage)
    {
        player_health -= damage;
        if (player_health <= 0)
        {
            healthBar.SetHealth(0);
            player_health = 0;
        }
        else
        {
            healthBar.SetHealth(player_health);
        }

    }

    public void Health(int heal)
    {
        player_health += heal;

        if (player_health >= maxHealth)
        {
            healthBar.SetHealth(maxHealth);
            player_health = maxHealth;
        }
        else
        {
            healthBar.SetHealth(player_health);
        }
    }

    public void SpendMana(int mana)
    {
        player_mana -= mana;

        if (player_mana <= 0)
        {
            manaBar.SetMana(0);
            player_mana = 0;
        }
        else
        {
            manaBar.SetMana(player_mana);
        }
    }

    public void RecoverMana(int mana)
    {
        player_mana += mana;

        if (player_mana >= maxMana)
        {
            manaBar.SetMana(maxMana);
            player_mana = maxMana;
        }
        else
        {
            manaBar.SetMana(player_mana);
        }
    }

    public void SumPoints(int points)
    {
        actualPoints += points;
        score.SetPoints(actualPoints);
    }

    public void SubPoints(int points)
    {
        actualPoints -= points;

        if (actualPoints <= 0)
        {
            score.SetPoints(0);
            actualPoints = 0;
        }
        else
        {
            score.SetPoints(actualPoints);
        }
    }

    public void Taking_Damage(EnemyStats enemyStats)
    {
        int damage = enemyStats.damage;
        
        if (damage >= player_health) {
            TakeDamage(damage);
            //Play death animation

            //Player Death
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Checkpoint return
            Health(maxHealth);
            RecoverMana(maxMana);
       
        } else if (damage < player_health && player_health != 0)
            TakeDamage(damage);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("potion"))
        {
            Health(maxHealth);
            healAudio.Play();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("mana"))
        {
            RecoverMana(maxMana);
            manaAudio.Play();
            Destroy(collision.gameObject);
        }
    }
}
