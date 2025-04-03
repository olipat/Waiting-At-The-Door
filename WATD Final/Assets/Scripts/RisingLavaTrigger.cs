using UnityEngine;

public class LavaTrigger : MonoBehaviour
{
    public RisingLava risingLava;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            risingLava.BeginRising();
            gameObject.SetActive(false);
        }
    }
}
