using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public bool secret = false;
    private bool shown = false;

    [Header("Animation")]
    public Animator checkpointAnimator;
    public string triggerName = "Activate"; // Animator trigger to play

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        UIController.Instance.saveGame(this.transform.position);
        Debug.Log("Saving at position: " + this.transform.position);

        if (checkpointAnimator != null)
        {
            checkpointAnimator.SetTrigger(triggerName);
        }

        if (secret)
        {
            if (MementoManager.instance.mementoSlots[2].color == Color.white)
            {
                ToastNotification.Show("You’ve reclaimed the collar. The path is painful, but love remembers.", 4f, "success");
            }
            else if (!shown)
            {
                ToastNotification.Show("You've uncovered a hidden path... but only the worthy may claim its secret.", 4f, "alert");
                shown = true;
            }
        }

        FallingPlatforms.instance?.returnPlatforms();
        StalactiteManager.instance?.ResetAllStalactites();
        RisingLavaManager.instance?.ResetAllLava();
    }
}
