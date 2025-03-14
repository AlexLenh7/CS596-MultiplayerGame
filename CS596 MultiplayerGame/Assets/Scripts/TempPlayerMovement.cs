using UnityEngine;

public class TempPlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canDoubleJump; 
    private Collider2D playerCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // Get horizontal movement using arrow keys
        float move = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) move = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) move = 1f;
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        // Double Jumping logic             
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                isGrounded = false;
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                canDoubleJump = false;
            }
        }
    }
}
