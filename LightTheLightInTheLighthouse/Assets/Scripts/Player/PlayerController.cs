using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float crawlSpeed = 2f;
    public float jumpForce = 7f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isGrounded;
    private bool isCrawling;

    private BoxCollider2D col;

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
        bool isCrouch = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        isCrawling = isCrouch && isGrounded;

        float speed = isCrawling ? crawlSpeed : moveSpeed;
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        if (move != 0)
            sr.flipX = move < 0;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        if (isCrawling)
        {
            sr.color = Color.gray;
            transform.localScale = new Vector3(1f, 0.5f, 1f);
        }
        else
        {
            sr.color = Color.black;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

    }
}
