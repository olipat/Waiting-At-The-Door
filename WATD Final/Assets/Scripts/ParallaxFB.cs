using UnityEngine;

public class ParallaxFB : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxFactor = 0.3f; 

    private Vector3 lastCameraPosition;

    void Start()
    {
        lastCameraPosition = cameraTransform.position;
    }

    void Update()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxFactor, deltaMovement.y * parallaxFactor, 0);
        lastCameraPosition = cameraTransform.position;
    }
}
