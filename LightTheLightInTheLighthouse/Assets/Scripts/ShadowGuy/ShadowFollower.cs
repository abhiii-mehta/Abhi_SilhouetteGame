using System.Collections;
using UnityEngine;

public class ShadowFollower : MonoBehaviour
{
    public Transform player;
    public Vector2 offset = new Vector2(-1f, -0.1f);
    public float followSpeed = 5f;
    private Rigidbody2D rb;

    public float glitchCheckInterval = 10f;
    public float glitchChance = 0.1f;
    public float glitchDuration = 2f;
    public float jumpForce = 5f;

    private float idleTimer = 0f;
    private float idleThreshold = 3f;
    private bool isWandering = false;

    private SpriteRenderer sr;
    private SpriteRenderer playerSR;

    public bool playerIsClimbing;
    private Vector2 ladderTargetPosition;
    private bool shadowClimbing = false;
    private float climbDelay = 0.4f;

    public Animator shadowAnimator;
    public Animator playerAnimator;

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        playerSR = player.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {

        if (!shadowAnimator || !playerAnimator) return;

        shadowAnimator.SetBool("Run", playerAnimator.GetBool("Run"));
        shadowAnimator.SetBool("Idle", playerAnimator.GetBool("Idle"));
        shadowAnimator.SetBool("Jump", playerAnimator.GetBool("Jump"));
        shadowAnimator.SetBool("Push", playerAnimator.GetBool("Push"));

        float playerHorizontal = Mathf.Abs(player.GetComponent<Rigidbody2D>().linearVelocity.x);
        bool playerIdle = playerHorizontal < 0.05f;

        if (playerIdle && !isWandering)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleThreshold)
            {
                StartCoroutine(WanderAwayAndReturn());
            }
        }
        else
        {
            idleTimer = 0f;
        }

        bool playerFacingLeft = player.localScale.x < 0;

        float shadowOffsetX = playerFacingLeft ? 1.2f : -1.2f; 
        
        Vector2 targetPos = (Vector2)player.position + offset;
        transform.position = targetPos;

        sr.flipX = player.GetComponent<SpriteRenderer>().flipX;

        sr.flipX = playerSR.flipX;

        string side = shadowOffsetX > 0 ? "right" : "left";
        Debug.Log($"Shadow is on the {side} of the player. Player is facing {(playerFacingLeft ? "left" : "right")}");

        if (playerIsClimbing)
        {
            if (!shadowClimbing)
            {
                shadowClimbing = true;
                StartCoroutine(FollowPlayerClimb());
            }
            return;
        }

    }
    IEnumerator FollowPlayerClimb()
{
    Vector2 startOffset = new Vector2(player.localScale.x < 0 ? 1.2f : -1.2f, 0f);
    yield return new WaitForSeconds(climbDelay);

    Vector2 targetPos = (Vector2)player.position + startOffset;

    float climbDuration = 0.5f;
    float elapsed = 0f;

    Vector2 initialPos = transform.position;

    while (elapsed < climbDuration)
    {
        transform.position = Vector2.Lerp(initialPos, targetPos, elapsed / climbDuration);
        elapsed += Time.deltaTime;
        yield return null;
    }

    transform.position = targetPos;
    shadowClimbing = false;
}

    public void SetOppositeSide(bool playerFacingLeft)
    {
        float shadowOffsetX = playerFacingLeft ? 1.2f : -1.2f;
        offset = new Vector2(shadowOffsetX, offset.y);
    }
    IEnumerator WanderAwayAndReturn()
    {
        isWandering = true;

        Vector2 originalPos = transform.position;
        Vector2 wanderTarget = originalPos + new Vector2(player.localScale.x > 0 ? -2f : 2f, 0f);

        shadowAnimator.SetBool("Run", true);

        float elapsed = 0f;
        float duration = 0.75f;

        while (elapsed < duration)
        {
            transform.position = Vector2.Lerp(originalPos, wanderTarget, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector2.Lerp(wanderTarget, originalPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        shadowAnimator.SetBool("Run", false);
        isWandering = false;
        idleTimer = 0f;
    }

}
