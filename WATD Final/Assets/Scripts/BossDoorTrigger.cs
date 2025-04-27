using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BossEntranceTrigger : MonoBehaviour
{
    public static BossEntranceTrigger Instance;
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public GameObject blockadeWall; // Assign in Inspector
    public string playerTag = "Player";
    public float delayBeforeBlockade = 5f;

    public float moveDistance = 5f;      
    public float moveDuration = 1f;

    public bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag(playerTag))
        {
            triggered = true;

            GetComponent<Collider2D>().enabled = false;
            TriggerBlockade();
        }
    }



    public void TriggerBlockade()
    {
        if (blockadeWall != null)
        {
            blockadeWall.SetActive(true);

            Vector3 startPos = blockadeWall.transform.position;
            Vector3 endPos = startPos;
            endPos.y = 0.56f;

            // Delay before dropping
            DOVirtual.DelayedCall(delayBeforeBlockade, () =>
            {
                blockadeWall.transform.DOMoveY(endPos.y, moveDuration)
                    .SetEase(Ease.OutBounce); // You can pick other eases too
            });
        }
    }
}

