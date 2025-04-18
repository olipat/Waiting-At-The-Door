using UnityEngine;

public class LavaTrigger : MonoBehaviour
{
    public RisingLava risingLava;
    public TeleportingBoss boss;
    public bool isBossLavaTrigger = false;

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

        if (isBossLavaTrigger)
        {
            gameObject.SetActive(false);
        }
    }
}
