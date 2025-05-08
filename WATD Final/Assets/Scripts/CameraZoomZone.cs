using UnityEngine;
using Unity.Cinemachine;
using DG.Tweening;

public class CameraZoomZone : MonoBehaviour
{
    public float zoomOutSize = 7f;
    public float zoomSpeed = 2f;

    private CinemachineCamera vCam;
    private float originalSize;
    private bool hasZoomedOut = false;

    private void Start()
    {
        vCam = FindFirstObjectByType<CinemachineCamera>();
        if (vCam == null)
        {
            Debug.LogError("[CameraZoomZone] CinemachineCamera not found in the scene!");
            return;
        }

        originalSize = vCam.Lens.OrthographicSize;
        Debug.Log("[CameraZoomZone] Found camera. Original size: " + originalSize);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered by: " + other.name);
        Debug.Log(hasZoomedOut);
        if (other.CompareTag("Player") && !hasZoomedOut)
        {
            hasZoomedOut = true;
            Debug.Log("[CameraZoomZone] Player entered trigger. Zooming out to: " + zoomOutSize);

            DOTween.To(
                () => vCam.Lens.OrthographicSize,
                x => {
                    vCam.Lens.OrthographicSize = x;
                    Debug.Log("[CameraZoomZone] Zooming... Current size: " + x);
                },
                zoomOutSize,
                zoomSpeed
            ).SetEase(Ease.InOutQuad);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && hasZoomedOut)
        {
            hasZoomedOut = false;
            Debug.Log("[CameraZoomZone] Player exited trigger. Zooming back to: " + originalSize);

            DOTween.To(
                () => vCam.Lens.OrthographicSize,
                x => {
                    vCam.Lens.OrthographicSize = x;
                    Debug.Log("[CameraZoomZone] Returning zoom... Current size: " + x);
                },
                originalSize,
                zoomSpeed
            ).SetEase(Ease.InOutQuad);
        }
    }
}
