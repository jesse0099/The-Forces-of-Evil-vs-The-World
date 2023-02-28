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
    // Start is called before the first frame update
    void Start()
    {
        UnitySystemConsoleRedirector.Redirect();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Console.WriteLine("Current Heath: " + player_health);
        Console.WriteLine("Current Mana: " + player_mana);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("potion"))
        {
            Console.WriteLine("Player Restored Health");
            player_health= 100;
            Destroy(collision.gameObject);
            Console.WriteLine("Current Heath: " + player_health);
            Console.WriteLine("Current Mana: " + player_mana);
        }
        if (collision.gameObject.CompareTag("mana"))
        {
            Console.WriteLine("Player Restored Mana");
            player_mana= 100;
            Destroy(collision.gameObject);
            Console.WriteLine("Current Heath: " + player_health);
            Console.WriteLine("Current Mana: " + player_mana);
        }
    }
}
