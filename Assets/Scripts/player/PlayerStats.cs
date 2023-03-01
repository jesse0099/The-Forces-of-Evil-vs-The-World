using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    public int player_health = 50;
    public int player_mana = 50;
    public int damage;
    Animator animator;

    public HealthBar healthBar;
    public int maxHealth = 100;
    public ManaBar manaBar;
    public int maxMana = 100;

    public Points score;
    public int actualPoints = 0;

    // Start is called before the first frame update
    void Start()
    {
        UnitySystemConsoleRedirector.Redirect();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Console.WriteLine("Current Heath: " + player_health);
        Console.WriteLine("Current Mana: " + player_mana);

        player_health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        player_mana = maxMana;
        manaBar.SetMaxMana(maxMana);

        score.SetPoints(actualPoints);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
            SpendMana(20);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            SumPoints(20);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SubPoints(40);
        }
    }

    void TakeDamage(int damage)
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

    void Heal(int heal)
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

    void SpendMana(int mana)
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

    void RecoverMana(int mana)
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

    void SumPoints(int points)
    {
        actualPoints += points;
        score.SetPoints(actualPoints);
    }

    void SubPoints(int points)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("potion"))
        {
            Console.WriteLine("Player Restored Health");
            Heal(40);
            Destroy(collision.gameObject);
            Console.WriteLine("Current Heath: " + player_health);
            Console.WriteLine("Current Mana: " + player_mana);
        }
        if (collision.gameObject.CompareTag("mana"))
        {
            Console.WriteLine("Player Restored Mana");
            RecoverMana(50);
            Destroy(collision.gameObject);
            Console.WriteLine("Current Heath: " + player_health);
            Console.WriteLine("Current Mana: " + player_mana);
        }
    }
}
