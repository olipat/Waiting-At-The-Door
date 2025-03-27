using UnityEngine;
using Unity.Cinemachine;

public class CameraLookDownOnFall : MonoBehaviour
{
    public float fallThreshold = -1f;       // Threshold below which we consider the player is falling
    public float fallingDampingY = 0.5f;      // Camera follows faster when falling
    public float normalDampingY = 2f;       // Default camera Y damping
    public float transitionSpeed = 10f;      // Smoothness of damping transition

    private CinemachinePositionComposer composer;
    private Rigidbody2D playerRB;

    void Start()
    {
        // Find player and get Rigidbody2D
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerRB = player.GetComponent<Rigidbody2D>();
        }

        // Get the Position Composer from this camera
        composer = GetComponent<CinemachinePositionComposer>();
    }

    void Update()
    {
        if (playerRB == null || composer == null) return;

        float currentY = composer.Damping.y;
        float targetY = playerRB.linearVelocity.y < fallThreshold ? fallingDampingY : normalDampingY;

        // Smoothly transition the Y damping value
        currentY = Mathf.Lerp(currentY, targetY, Time.deltaTime * transitionSpeed);

        Vector3 damping = composer.Damping;
        damping.y = currentY;
        composer.Damping = damping;
    }
}
