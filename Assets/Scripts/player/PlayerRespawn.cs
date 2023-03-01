using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    private float CheckpointPositionX, CheckpointPositionY;

    // Start is called before the first frame update
    void Start()
    {

        if (PlayerPrefs.GetString("Scene") == SceneManager.GetActiveScene().name)
        {
            transform.position = (new Vector2(PlayerPrefs.GetFloat("CheckpointPositionX"), PlayerPrefs.GetFloat("CheckpointPositionY")));
        }
    }

    public void ReachedCheckpoint(string Scene, float x, float y)
    {
        PlayerPrefs.SetString("Scene", Scene);
        PlayerPrefs.SetFloat("CheckpointPositionX", x);
        PlayerPrefs.SetFloat("CheckpointPositionY", y);
    }
}
