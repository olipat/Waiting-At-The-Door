using UnityEngine;

public class BeamTrigger : MonoBehaviour
{
    public StoplightBeam controller; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            controller.OnPlayerEnter();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            controller.OnPlayerExit();
    }
}

