using UnityEngine;

public class RespawnPoint : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIController.Instance.saveGame(this.transform.position);
            Debug.Log("Saving at position: " + this.transform.position);

            FallingPlatforms.instance.returnPlatforms();
            StalactiteManager.instance.ResetAllStalactites();
        }
    }
}
