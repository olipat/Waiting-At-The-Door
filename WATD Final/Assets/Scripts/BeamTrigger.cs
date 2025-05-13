using UnityEngine;

public class BeamTrigger : MonoBehaviour
{
    public StoplightBeam controller; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            controller.OnPlayerEnter();
            //Debug.Log("palyer in the light guy beam ");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            controller.OnPlayerExit();
            //Debug.Log("palyer left the light guy beam ");
    }
}

