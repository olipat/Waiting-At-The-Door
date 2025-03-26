using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public GameObject doorClosed;
    public GameObject doorOpened;
    public string lockedMessage = "You need all bone keys!";
    public float messageDuration = 2f;

    private bool isOpen = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpen || !other.CompareTag("Player")) return;

        if (BoneKeyManager.Instance.HasAllKeys())
        {
            OpenDoor();
        }
        else
        {
            UIController.Instance.ShowAbilityWarning(lockedMessage, messageDuration);
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        if (doorClosed != null) doorClosed.SetActive(false);
        if (doorOpened != null) doorOpened.SetActive(true);
        Debug.Log("Boss door unlocked!");
    }
}


