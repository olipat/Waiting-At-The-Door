using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    private bool flag = false;
    public GameObject enemyPreFab;

    public Transform target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && flag == false)
        {
            flag = true;
            Instantiate(enemyPreFab, target.transform.position, Quaternion.identity);
        }
    }
}
