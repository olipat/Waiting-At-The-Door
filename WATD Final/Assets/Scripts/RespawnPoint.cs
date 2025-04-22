using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public bool secret = false;
    private bool shown = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIController.Instance.saveGame(this.transform.position);
            Debug.Log("Saving at position: " + this.transform.position);

            if (secret)
            {
                if (MementoManager.instance.mementoSlots[2].color == Color.white)
                {
                    ToastNotification.Show("You’ve reclaimed the collar. The path is painful, but love remembers.", 4f, "success");
                }
                else
                {
                    if (!shown) {
                        ToastNotification.Show("You've uncovered a hidden path... but only the worthy may claim its secret.", 4f, "alert");
                        shown = true;
                    }
                }
            }
            

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
