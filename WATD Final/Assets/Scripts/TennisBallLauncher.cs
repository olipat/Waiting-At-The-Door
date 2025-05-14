using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TennisBallLauncher : MonoBehaviour
{
    public GameObject ballPrefab; // The ball to shoot
    public Transform target; // The target the ball should hit
    //public float launchForce = 10f; // How fast the ball moves

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    LaunchBall();
    //}
    public TMP_Text interactionHint;
    bool playerInRange = false;
    private bool shooting = false;

    private void Update()
    {

        if (playerInRange && Input.GetKeyDown(KeyCode.F) && !shooting)
        {
            shooting = true;
            ballPrefab.SetActive(true);
        }
        if(shooting == true)
        {
            LaunchBall();
            if (Vector2.Distance(ballPrefab.transform.position, target.position) < 0.1f)
            {
                ballPrefab.SetActive(false);
                var boss = target.GetComponent<denialBoss>();
                boss.health -= 1;
                boss.hb.SetHealth(boss.health);
                boss.flag = true;

                interactionHint.enabled = false;
                gameObject.SetActive(false);
                enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            interactionHint.text = "Press F to launch!";
            interactionHint.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            interactionHint.enabled = false;
        }
    }

    void LaunchBall()
    {
        ballPrefab.transform.position = Vector2.MoveTowards(ballPrefab.transform.position, target.position, 0.2f);
    }
}
