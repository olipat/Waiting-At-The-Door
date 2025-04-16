using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public bool secret = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIController.Instance.saveGame(this.transform.position);
            Debug.Log("Saving at position: " + this.transform.position);

            if (secret)
            ToastNotification.Show("You've uncovered a hidden path... but only the worthy may claim its secret.", 4f, "alert");

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
