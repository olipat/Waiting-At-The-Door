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

    public bool canMove = true;

    private Controller.PlayerController playerController;

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
        
            Vector3 hitPoint = hit.point;

            // Optionally round the X to the nearest whole number to center on a tile (assuming tile size of 1)
            float tileSize = 1f; // or your actual tile size
            float centerX = Mathf.Floor(hitPoint.x / tileSize) * tileSize + tileSize / 2f;

            lastGroundedPosition = new Vector3(centerX, hitPoint.y, transform.position.z);

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

        transform.position = lastGroundedPosition;

        playerController = FindFirstObjectByType<Controller.PlayerController>();

        //playerController.canMove = false;
        playerController.enabled = false;

        StartCoroutine(ReenableAfterDelay());
        
    }

    private IEnumerator ReenableAfterDelay()
    {
        
        yield return new WaitForSecondsRealtime(1f); // unaffected by timescale

        playerController.enabled = true;

        // Briefly freeze to reset horizontal motion
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(0.1f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        rb.gravityScale = 1;
        
        rb.linearVelocity = Vector2.zero; // double ensure it doesn't slide

        Time.timeScale = 1f;

        
        playerController.canMove = true;

    }
}