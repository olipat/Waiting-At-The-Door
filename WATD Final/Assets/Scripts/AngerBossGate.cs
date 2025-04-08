using System.Collections;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public GameObject doorClosed;
    public GameObject doorHalf;
    public GameObject doorOpened;
    public string lockedMessage = "You need all bone keys!";
    public float messageDuration = 2f;
    public float doorDelay = 2f;
    public AudioClip doorSound;
    private AudioSource audioS;

    private bool isOpen = false;
    private Collider2D closedDoorCollider;

    private void Start()
    {
        if (doorClosed != null) doorClosed.SetActive(true);
        if (doorHalf != null) doorHalf.SetActive(false); 
        if (doorOpened != null) doorOpened.SetActive(false);

        if (doorClosed != null)
            closedDoorCollider = doorClosed.GetComponent<Collider2D>();

        audioS = GetComponent<AudioSource>(); 
        if (audioS == null)
            audioS = gameObject.AddComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (isOpen)
            return;

        if (BoneKeyManager.Instance.HasAllKeys())
        {
            StartCoroutine(OpenDoorSequence());
        }
        else
        {
            UIController.Instance.ShowAbilityWarning(lockedMessage, messageDuration);
        }
    }

    IEnumerator OpenDoorSequence() 
    {
        isOpen = true;

        if (closedDoorCollider != null)
            closedDoorCollider.enabled = false; 

        if (doorClosed != null) doorClosed.SetActive(false);

        if (doorSound != null && audioS != null)
        {
            audioS.PlayOneShot(doorSound, 15f);
        }

        if (doorHalf != null)
        {
            doorHalf.SetActive(true); 
            Debug.Log("Half door active!");
        }

        yield return new WaitForSeconds(doorDelay); 

        if (doorHalf != null) 
        {
            doorHalf.SetActive(false);
            Debug.Log("Half door deactivated!");
        }
        if (doorOpened != null) 
        {
            doorOpened.SetActive(true);
            Debug.Log("Door opened!");
        }
    }
}


