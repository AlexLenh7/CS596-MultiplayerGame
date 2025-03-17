using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private NetworkVariable<bool> isGrounded = new NetworkVariable<bool>();
    private NetworkVariable<bool> canDoubleJump = new NetworkVariable<bool>();

    [SerializeField] public AudioClip JumpFX;
    private AudioSource footstepSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        footstepSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Process if client owns this player
        if (!IsOwner) return;

        // Get horizontal movement
        float horizontalInput = Input.GetAxis("Horizontal");

        // Play footstepFX if the player is not still 
        if (horizontalInput != 0 && isGrounded.Value && !footstepSource.isPlaying)
        {
            footstepSource.Play();
        }
        else if ((horizontalInput == 0 || !isGrounded.Value) && footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }
        
        // Check to see if Jump is pressed locally
        bool jumpPressed = Input.GetButtonDown("Jump");

        // Send the movement and if jumped over to server
        MovementToServerRpc(horizontalInput, jumpPressed);
    }

    // Send the logic to the server
    [Rpc(SendTo.Server)]
    void MovementToServerRpc(float moveInput,  bool jumpInput)
    {
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // Double jumping logic
        if (jumpInput)
        {
            if (isGrounded.Value)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                SoundManager.instance.PlaySound(JumpFX, transform, 1f);
                isGrounded.Value = false;
                canDoubleJump.Value = true;
            }
            else if (canDoubleJump.Value)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                SoundManager.instance.PlaySound(JumpFX, transform, 1f);
                canDoubleJump.Value = false;
            }
        }
    }

    // Check if player is on the ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded.Value = true;
        }
    }

    // Check if player leaves the ground
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsServer) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded.Value = false;
        }
    }
}
