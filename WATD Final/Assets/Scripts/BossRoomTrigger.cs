using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using System;
using DG.Tweening; // Fix IEnumerator error

public class BossRoomTrigger : MonoBehaviour
{
    public static BossRoomTrigger Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }
    public Collider2D bossRoomBounds; // Assign Boss Camera Bounds in Inspector
    public Collider2D bossRoomDoor;
    public float zoomOutSize = 10f;   // Final zoom level
    public float zoomSpeed = 2f;      // Speed of zooming out
    public float moveSpeed = 2f;      // Speed of camera movement

    public Vector3 cameraMoveRightOffset = new Vector3(5f, 0f, 0); // Move right first
    public Vector3 cameraMoveUpOffset = new Vector3(0f, 3f, 0);    // Then move up

    public GameObject player;
    public Vector3 savePosition;

    public float additionalStartHeight = 2f;
    public bool denialBoss = false; 

    private CinemachineConfiner2D confiner;
    private CinemachineCamera vCam;
    public bool inBossRoom = false;
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
            AudioManager.instance.PlayBossMusic();

            Debug.Log("Entered Boss Room, stopping player.");

            player.GetComponentInChildren<Controller.PlayerAnimator>().ForceIdle();
            player.GetComponentInChildren<Controller.PlayerAnimator>().enabled = false;

            playerController.enabled = false;
            inBossRoom = true;

            playerRB.linearVelocity = Vector2.zero; // Stop all movement

            UIController.Instance.saveGame(savePosition);
            //BossEntranceTrigger.Instance.TriggerBlockade();

            StartCoroutine(HandleCameraMovement());
        }
    }

    private IEnumerator HandleCameraMovement()
    {
        Vector3 startPosition = vCam.transform.position;
        if (denialBoss)
        {
            startPosition += new Vector3(0f, additionalStartHeight, 0f);
            vCam.transform.position = startPosition; // Actually move camera upward only for denial boss
        }

        Vector3 rightPosition = startPosition + cameraMoveRightOffset;
        Vector3 finalPosition = rightPosition + cameraMoveUpOffset;
        float startSize = vCam.Lens.OrthographicSize;

        originalFollowTarget = vCam.Follow;
        vCam.Follow = null;
        confiner.enabled = false;

        Sequence cameraSequence = DOTween.Sequence();

        cameraSequence.Append(vCam.transform.DOMove(rightPosition, 2f).SetEase(Ease.InOutSine));

        cameraSequence.Append(
            DOTween.To(() => vCam.Lens.OrthographicSize, x => vCam.Lens.OrthographicSize = x, zoomOutSize, 2f)
            .SetEase(Ease.InOutQuad)
            .OnUpdate(() =>
            {
                vCam.transform.position = Vector3.Lerp(rightPosition, finalPosition,
                    (vCam.Lens.OrthographicSize - startSize) / (zoomOutSize - startSize));
            })
        );

        cameraSequence.OnComplete(() =>
        {
            confiner.BoundingShape2D = bossRoomBounds;
            confiner.InvalidateBoundingShapeCache();
            confiner.enabled = true;

            vCam.Follow = originalFollowTarget;
            vCam.PreviousStateIsValid = false;
            playerController.enabled = true;
            GameManager.instance.FightingBoss = true;
            player.GetComponentInChildren<Controller.PlayerAnimator>().enabled = true;
        });

        yield return cameraSequence.WaitForCompletion();
    }
}