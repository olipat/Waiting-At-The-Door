using UnityEngine;
using Controller;

public class BounceObj : MonoBehaviour
{
    public float bounceForce = 25f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.TriggerBounce(bounceForce);
                Debug.Log("Bounce triggered!");
            }
        }
    }
}

