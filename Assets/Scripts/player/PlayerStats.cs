using System;
using System.Collections.Generic;
using Assets.Commons;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    public int player_health = 50;
    public int player_mana = 50;
    public int damage;
    Animator animator;

    private Dictionary<string, int> damage_list;
    // Start is called before the first frame update
    void Start()
    {
        UnitySystemConsoleRedirector.Redirect();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        damage_list = new Dictionary<string, int>();
        damage_list.Add(Literals.DAMAGE_LIST.common_enemy.ToString(), 20);
        damage_list.Add(Literals.DAMAGE_LIST.spikes.ToString(), 10);
        Console.WriteLine("Current Heath: " + player_health);
        Console.WriteLine("Current Mana: " + player_mana);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Taking_Damage(string contact)
    {
        if (damage_list != null)
        {
            if (damage_list.ContainsKey(contact))
            {
                int damage = damage_list[contact];
                if (damage >= player_health)
                {
                    /*Player Death*/
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    player_health = 100;

                }
                else if (damage < player_health && player_health != 0)
                {
                    player_health = player_health - damage;
                    Debug.Log($"Got hit by {contact}, {player_health} of life left");
                }
            }
        }
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
