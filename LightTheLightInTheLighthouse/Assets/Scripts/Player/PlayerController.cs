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
    public ShadowFollower shadowFollower;
   
   private Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        float move = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        bool isCrouch = (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && isGrounded;
        isCrawling = isCrouch;
        float speed = isCrawling ? crawlSpeed : moveSpeed;

        if (isOnLadder && Mathf.Abs(vInput) > 0f)
            isClimbing = true;
        else if (!isOnLadder)
            isClimbing = false;

        if (shadowFollower != null)
            shadowFollower.playerIsClimbing = isClimbing;

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
        {
            bool facingLeft = move < 0;
            sr.flipX = facingLeft;
            transform.localScale = new Vector3(1f, transform.localScale.y, 1f);

            if (shadowFollower != null)
                shadowFollower.SetOppositeSide(facingLeft);
        }

        if (isClimbing)
            col.size = new Vector2(1f, 1f);
        else if (isCrawling)
            col.size = new Vector2(1f, 0.5f);
        else
            col.size = new Vector2(1f, 1f);

        bool isJumping = !isGrounded && !isClimbing;
        bool isActuallyRunning = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 && isGrounded && !isCrawling && !isClimbing;
        bool isIdle = move == 0 && isGrounded && !isJumping && !isClimbing && !isCrawling;
        bool isPushing = isActuallyRunning && TouchingPushableObject();
        bool isRunning = isActuallyRunning && !isPushing;

        anim.SetBool("Push", isPushing);
        anim.SetBool("Run", isRunning);
        anim.SetBool("Idle", isIdle);
        anim.SetBool("Jump", isJumping);
        anim.SetBool("Crawl", isCrawling);
        anim.SetBool("Climb", isClimbing);

        Debug.DrawRay(transform.position, Vector2.down * 1f, Color.red);
        if (isPushing)
        {
            Debug.Log("Push is true, playing push animation");
        }

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

    private bool TouchingPushableObject()
    {
        Vector2 direction = sr.flipX ? Vector2.left : Vector2.right;
        Vector2 origin = transform.position;
        Vector2 size = col.size;
        float distance = 0.2f;

        RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0f, direction, distance, groundLayer);

        Debug.DrawRay(origin + Vector2.up * (size.y / 2), direction * distance, Color.green);
        Debug.DrawRay(origin - Vector2.up * (size.y / 2), direction * distance, Color.green);

        if (hit.collider != null)
        {
            Debug.Log("Hit object: " + hit.collider.name);
            return hit.collider.CompareTag("Pushable");
        }

        return false;
    }

}
