using UnityEngine;
using System.Collections.Generic;

public class Knockout: MonoBehaviour
{
    public float knockbackForce = 60.0f; // adjust as needed
    public float knockbackDuration = 3.0f; // adjust as needed

    //Control knockback duration
    private float knockbackTimer = 0.0f;
    private float knockbackDelay = 0.8f;
    private bool isKnockedBack = false;

    private Rigidbody2D rb;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer += Time.deltaTime;
            if (knockbackTimer >= knockbackDelay)
            {
                isKnockedBack = false;
                knockbackTimer = 0.0f;
                this.enabled = true; // re-enable the script
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //if (!collision.gameObject.name.Equals("player")) return;

        var tags = new List<string>(collision.gameObject.tag.Split(","));

        if (tags.Contains("Knockout"))
        {
            Vector2 knockbackDirection = -collision.GetContact(0).normal;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            // disable the dynamic body temporarily
            if (this.name.Equals("player") && tags.Contains("Enemy")) {
                rb.isKinematic = true;
                Invoke("EnableDynamicBody", knockbackDuration);
            }

            isKnockedBack = true;
            this.enabled = false; // disable the script during knockback effect
        }
    }

    private void EnableDynamicBody()
    {
        rb.isKinematic = false;
    }

}
