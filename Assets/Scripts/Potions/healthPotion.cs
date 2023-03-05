using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPotion : MonoBehaviour
{
    public AudioSource healAudio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var tags = new List<string>(collision.gameObject.tag.Split(","));
        if (tags.Contains("Player"))
        {
            Debug.Log("Taken");
            healAudio.Play();
        }
    }
}
