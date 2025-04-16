using UnityEngine;

public class RespawnPoint : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIController.Instance.saveGame(this.transform.position);
            Debug.Log("Saving at position: " + this.transform.position);

            if (FallingPlatforms.instance != null)
                FallingPlatforms.instance.returnPlatforms();

            if (StalactiteManager.instance != null)
                StalactiteManager.instance.ResetAllStalactites();

            if (RisingLavaManager.instance != null)
            {
                Debug.Log("Inside");
                RisingLavaManager.instance.ResetAllLava();
            }


        }
    }
}
