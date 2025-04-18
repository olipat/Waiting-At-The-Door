using System.ComponentModel;
using Unity.Cinemachine;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private Controller.PlayerController playerController;
    private Rigidbody2D playerRB;

    public bool isRisingLava = false;
    public Transform Respawn;

    private void Start()
    {
        playerController = FindFirstObjectByType<Controller.PlayerController>();
        playerRB = playerController.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
                
            if (UIController.Instance.playerHealth <= 1)
            {
                Debug.Log("Fell into void, stopping player.");
                playerController.enabled = false;
                playerRB.linearVelocity = Vector2.zero;
                UIController.Instance.ApplyDamage(3);
                AudioManager.instance.PlaySFX(6);
            }
            else
            {
                if (isRisingLava && Respawn != null)
                {
                    playerController.transform.position = Respawn.position;
                }
                else
                {
                    UIController.Instance.ApplyDamage(1);
                    AudioManager.instance.PlaySFX(6);
                    PlayerGroundTracker.instance.RespawnAtLastGround();
                }
            }
        }
    }
}
