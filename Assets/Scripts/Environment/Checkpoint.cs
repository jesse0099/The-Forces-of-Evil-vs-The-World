using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public AudioSource reachedAudio;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player,Knockout"))
        {
            Debug.Log("Cheakpoint Reached");
            collision.GetComponent<PlayerRespawn>().ReachedCheckpoint(SceneManager.GetActiveScene().name, collision.transform.position.x, collision.transform.position.y);
            reachedAudio.Play();
        }
    }
}
