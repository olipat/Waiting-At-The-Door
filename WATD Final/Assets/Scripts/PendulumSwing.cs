using UnityEngine;

public class PendulumSwing : MonoBehaviour
{
    public float swingAngle = 45f; // Maximum angle (degrees)
    public float swingSpeed = 2f;  // Speed of swinging motion

    private float startRotation;

    void Start()
    {
        startRotation = transform.rotation.z; // Store initial rotation
    }

    void Update()
    {
        float angle = swingAngle * Mathf.Sin(Time.time * swingSpeed); // Swing back and forth
        if(angle < -13.5f)
        {
            angle = -13.5f;
        }
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}