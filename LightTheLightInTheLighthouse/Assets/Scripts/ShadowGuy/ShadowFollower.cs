using System.Collections;
using UnityEngine;

public class ShadowFollower : MonoBehaviour
{
    public Transform player;
    public Vector2 offset = new Vector2(-1f, -0.1f);
    public float followSpeed = 5f;

    public float glitchCheckInterval = 10f;
    public float glitchChance = 0.1f;
    public float glitchDuration = 2f;
    public float jumpForce = 5f;

    private bool isMisbehaving = false;
    private Rigidbody2D rb;
    private float glitchTimer = 0f;

    private SpriteRenderer sr;
    private SpriteRenderer playerSR;
  

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        playerSR = player.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!player || isMisbehaving || !sr.enabled) return;

        glitchTimer += Time.deltaTime;

        if (glitchTimer >= glitchCheckInterval)
        {
            glitchTimer = 0f;

            if (Random.value <= glitchChance)
            {
                StartCoroutine(GlitchBehavior());
                return;
            }
        }

        bool playerFacingLeft = player.localScale.x < 0;

        float shadowOffsetX = playerFacingLeft ? 1.2f : -1.2f; 
        
        Vector2 targetPos = (Vector2)player.position + offset;
        transform.position = targetPos;

        transform.localScale = player.localScale;
        sr.flipX = playerSR.flipX;

        string side = shadowOffsetX > 0 ? "right" : "left";
        Debug.Log($"Shadow is on the {side} of the player. Player is facing {(playerFacingLeft ? "left" : "right")}");
    }

    public void SetOppositeSide(bool playerFacingLeft)
    {
        float shadowOffsetX = playerFacingLeft ? 1.2f : -1.2f;
        offset = new Vector2(shadowOffsetX, offset.y);
    }

    IEnumerator GlitchBehavior()
    {
        isMisbehaving = true;

        Vector2 oppositeOffset = -offset.normalized * 1.5f;
        Vector2 glitchTarget = (Vector2)transform.position + oppositeOffset;

        float t = 0f;
        while (t < 0.5f)
        {
            transform.position = Vector2.Lerp(transform.position, glitchTarget, t * 2f);
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        rb.linearVelocity = new Vector2(0f, jumpForce);

        yield return new WaitForSeconds(0.5f);

        isMisbehaving = false;
    }
}
