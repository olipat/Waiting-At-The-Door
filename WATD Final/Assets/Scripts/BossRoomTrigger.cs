using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using System; // Fix IEnumerator error

public class BossRoomTrigger : MonoBehaviour
{
    public Collider2D bossRoomBounds; // Assign Boss Camera Bounds in Inspector
    public float zoomOutSize = 10f;   // Final zoom level
    public float zoomSpeed = 2f;      // Speed of zooming out
    public float moveSpeed = 2f;      // Speed of camera movement

    public Vector3 cameraMoveRightOffset = new Vector3(5f, 0f, 0); // Move right first
    public Vector3 cameraMoveUpOffset = new Vector3(0f, 3f, 0);    // Then move up

    public GameObject player;

    private CinemachineConfiner2D confiner;
    private CinemachineCamera vCam;
    private bool inBossRoom = false;
    private Transform originalFollowTarget; // Store the original target
    private Controller.PlayerController playerController;
    private Rigidbody2D playerRB;

    private void Start()
    {
        vCam = FindFirstObjectByType<CinemachineCamera>(); // Get Cinemachine Camera
        confiner = vCam.GetComponent<CinemachineConfiner2D>(); // Get Camera Confiner
        playerController = FindFirstObjectByType<Controller.PlayerController>();
        playerRB = playerController.GetComponent<Rigidbody2D>(); // Get Rigidbody2D
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !inBossRoom)
        {
            Debug.Log("Entered Boss Room, stopping player.");
            playerController.enabled = false;
            inBossRoom = true;
            playerRB.linearVelocity = Vector2.zero; // Stop all movement
            StartCoroutine(HandleCameraMovement());
        }
    }

    private IEnumerator HandleCameraMovement()
    {
        Vector3 startPosition = vCam.transform.position;
        Vector3 rightPosition = startPosition + cameraMoveRightOffset; // Move right
        Vector3 finalPosition = rightPosition + cameraMoveUpOffset;   // Move up after right

        float startSize = vCam.Lens.OrthographicSize;
        float elapsedTime = 0f;
        float duration = 2f; // Total movement time

        // Disable Camera Follow and store the original target
        originalFollowTarget = vCam.Follow;
        vCam.Follow = null; // This allows manual camera movement

        // Disable the confiner so the camera can move freely
        confiner.enabled = false;

        // Step 1: Move Right Only (No Zoom)
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            vCam.transform.position = Vector3.Lerp(startPosition, rightPosition, elapsedTime / duration);
            yield return null;
        }

        // Ensure it's at the right position before moving up
        vCam.transform.position = rightPosition;

        elapsedTime = 0f; // Reset timer for next movement

        // Step 2: Move Up While Zooming Out
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            vCam.transform.position = Vector3.Lerp(rightPosition, finalPosition, elapsedTime / duration);
            vCam.Lens.OrthographicSize = Mathf.Lerp(startSize, zoomOutSize, elapsedTime / duration);
            yield return null;
        }

        // Ensure final values are set
        vCam.transform.position = finalPosition;
        vCam.Lens.OrthographicSize = zoomOutSize;

        // Re-enable the confiner and set new bounds after camera moves
        confiner.BoundingShape2D = bossRoomBounds;
        confiner.InvalidateBoundingShapeCache();
        confiner.enabled = true; // Re-enable the confiner

        //yield return new WaitForSeconds(0.5f); // Small delay before re-enabling follow

        // Re-enable Camera Follow so it follows the player again
        vCam.Follow = originalFollowTarget;
        vCam.PreviousStateIsValid = false;
        playerController.enabled = true;
    }
}