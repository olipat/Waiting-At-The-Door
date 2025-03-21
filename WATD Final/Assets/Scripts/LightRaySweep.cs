using UnityEngine;

public class LightRaySweep : MonoBehaviour
{
    public float leftLimit = -5f;  // Leftmost X position
    public float rightLimit = 5f;  // Rightmost X position
    public float sweepSpeed = 3f;  // Speed of movement

    void Update()
    {
        float xPosition = Mathf.PingPong(Time.time * sweepSpeed, rightLimit - leftLimit) + leftLimit;
        transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);
    }
}
