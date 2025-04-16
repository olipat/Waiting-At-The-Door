using UnityEngine;

public class LavaTrigger : MonoBehaviour
{
    public RisingLava risingLava;
    public TeleportingBoss boss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (risingLava != null)
        {
            risingLava.BeginRising();
        }

        if (boss != null)
        {
            boss.BeginBossFight();
        }

        gameObject.SetActive(false);
    }
}
