using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float crawlSpeed = 2f;
    public float jumpForce = 7f;
    public float climbSpeed = 3f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D col;

    private bool isGrounded;
    private bool isCrawling;
    private bool isOnLadder;
    private bool isClimbing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        float move = Input.GetAxisRaw("Horizontal");

        bool isCrouch = (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && isGrounded;
        isCrawling = isCrouch;

        float speed = isCrawling ? crawlSpeed : moveSpeed;

        float vInput = Input.GetAxisRaw("Vertical");

        if (isOnLadder && Mathf.Abs(vInput) > 0f)
            isClimbing = true;
        else if (!isOnLadder)
            isClimbing = false;

        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(move * speed, vInput * climbSpeed);
        }
        else
        {
            rb.gravityScale = 3f;
            rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (move != 0)
            sr.flipX = move < 0;

        if (isClimbing)
        {
            sr.color = Color.blue;
            transform.localScale = new Vector3(1f, 1f, 1f);
            col.size = new Vector2(1f, 1f);
        }
        else if (isCrawling)
        {
            sr.color = Color.gray;
            transform.localScale = new Vector3(1f, 0.5f, 1f);
            col.size = new Vector2(1f, 0.5f);
        }
        else
        {
            sr.color = Color.white;
            transform.localScale = new Vector3(1f, 1f, 1f);
            col.size = new Vector2(1f, 1f);
        }

        Debug.DrawRay(transform.position, Vector2.down * 1f, Color.red);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
            isOnLadder = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
            isOnLadder = false;
    }
}
