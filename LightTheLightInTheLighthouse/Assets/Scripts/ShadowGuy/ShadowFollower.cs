using UnityEngine;

public class ShadowFollower : MonoBehaviour
{
    public Transform player;
    public Vector2 offset = new Vector2(-1f, -0.1f);

    private SpriteRenderer sr;
    private SpriteRenderer playerSR;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        playerSR = player.GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = (Vector2)player.position + offset;

            sr.flipX = playerSR.flipX;
        }
    }
}
