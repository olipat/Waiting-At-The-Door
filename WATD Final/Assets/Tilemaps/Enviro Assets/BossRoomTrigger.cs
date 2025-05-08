using UnityEngine;

public class BBossRoomTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            BargainingBoss boss = FindObjectOfType<BargainingBoss>();
            if (boss != null)
            {
                boss.ActivateBoss();
                triggered = true;
            }
        }
    }
}
