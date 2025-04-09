using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGroundTracker : MonoBehaviour
{
    public static PlayerGroundTracker instance;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }


    [Header("Ground Detection Settings")]
    [SerializeField] private LayerMask groundLayer;
    public float checkDistance = 0.2f;

    [Header("Last Safe Position")]
    public Vector3 lastGroundedPosition;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastGroundedPosition = transform.position; // start with current pos
    }

    void Update()
    {
        // Cast a ray down to detect ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, checkDistance, groundLayer);

        if (hit.collider != null)
        {
            lastGroundedPosition = transform.position;
            // Optional: Debug line to visualize
            Debug.DrawRay(transform.position, Vector2.down * checkDistance, Color.green);
        }
        else
        {
            Debug.DrawRay(transform.position, Vector2.down * checkDistance, Color.red);
        }
    }

    // Call this method when the player falls in a death zone
    public void RespawnAtLastGround()
    {
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;

        transform.position = lastGroundedPosition + Vector3.left * 0.5f;
        this.enabled = true;
        Time.timeScale = 1;
        Debug.Log(Time.timeScale);
        Debug.Log("Respawned at: " + lastGroundedPosition);

    }
}