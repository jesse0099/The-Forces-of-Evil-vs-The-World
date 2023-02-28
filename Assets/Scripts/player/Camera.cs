using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject player_object;
    private Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
        movement = transform.position - player_object.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* Se llama después de todos los eventos */
    private void LateUpdate()
    {
        transform.position = player_object.transform.position + movement;
    }
}
