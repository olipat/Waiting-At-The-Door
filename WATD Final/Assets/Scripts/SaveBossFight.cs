using UnityEngine;

public class SaveBossFight : MonoBehaviour
{

    public bool saved;

    public Vector3 position;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !saved)
        {
            saved = true;
            UIController.Instance.saveGame(position);
        }
    }
}
