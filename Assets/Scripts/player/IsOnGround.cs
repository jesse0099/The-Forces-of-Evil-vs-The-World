using UnityEngine;

public class IsOnGround : MonoBehaviour
{
    public static bool isGrounded;

    void Start()
    {
        UnitySystemConsoleRedirector.Redirect();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            isGrounded = false;
    }
}
