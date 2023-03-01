using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntreNewLevel : MonoBehaviour
{
    public string levelName;
    private bool inDoor = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player,Knockout"))
        {
            if (inDoor != true)
                inDoor = true;
            Debug.Log(inDoor);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
      if (inDoor != false)
            inDoor = false;
        Debug.Log(inDoor);
    }

    private void Update()
    {
        if (inDoor && Input.GetKey("e"))
        {
            SceneManager.LoadScene("lvl1");
        }
    }
}
