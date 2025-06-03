using UnityEngine;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float crawlSpeed = 2f;
    public float jumpForce = 7f;
    public float climbSpeed = 3f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private CapsuleCollider2D col;

    private bool isGrounded;
    private bool isCrawling;
    private bool isOnLadder;
    private bool isClimbing;
    public ShadowFollower shadowFollower;

    private Animator anim;

    private float stuckCheckTimer = 0f;
    private float stuckDurationThreshold = 0.25f;
    private Vector2 lastPosition;

    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        transform.localScale = new Vector3(1.3f, 1.3f, 1f);
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
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

                if (audioSource != null)
                    audioSource.Play();
            }

        }

        if (move != 0)
        {
            bool facingLeft = move < 0;
            sr.flipX = facingLeft;

            if (shadowFollower != null)
                shadowFollower.SetOppositeSide(facingLeft);
        }

        if (isClimbing)
            col.size = new Vector2(1f, 1f);
        else if (isCrawling)
            col.size = new Vector2(0.7f, 0.3f);
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
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 && isGrounded)
        {
            stuckCheckTimer += Time.deltaTime;

            if (Vector2.Distance(transform.position, lastPosition) < 0.01f)
            {
                if (stuckCheckTimer >= stuckDurationThreshold)
                {
                    rb.position += Vector2.up * 0.05f;
                    Debug.Log("Nudged player upward to unstick.");
                    stuckCheckTimer = 0f;
                }
            }
            else
            {
                stuckCheckTimer = 0f;
            }

            lastPosition = transform.position;
        }
        else
        {
            stuckCheckTimer = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isOnLadder = true;

            if (shadowFollower != null && !shadowFollower.isFinalClimb)
            {
                shadowFollower.isFinalClimb = true;
                shadowFollower.fadeStartY = transform.position.y;
                shadowFollower.fadeEndY = transform.position.y + 5f;
            }
        }
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
