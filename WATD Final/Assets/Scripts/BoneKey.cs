using UnityEngine;

public class BoneKey : MonoBehaviour
{
    public int keyIndex = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BoneKeyManager.Instance.CollectKey(keyIndex);
            gameObject.SetActive(false);
        }
    }
}


