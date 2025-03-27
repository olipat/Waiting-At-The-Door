using UnityEngine;

public class TennisBallLauncher : MonoBehaviour
{
    public GameObject ballPrefab; // The ball to shoot
    public Transform target; // The target the ball should hit
    //public float launchForce = 10f; // How fast the ball moves

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    LaunchBall();
    //}
    private bool shooting = false;

    private void Update()
    {
        if(shooting == true)
        {
            LaunchBall();
        }
        if(ballPrefab.transform.position == target.position)
        {
            ballPrefab.SetActive(false);
            target.GetComponent<denialBoss>().health -= 1;
            target.GetComponent<denialBoss>().hb.SetHealth(target.GetComponent<denialBoss>().health);
            print(target.GetComponent<denialBoss>().health);
            target.GetComponent<denialBoss>().flag = true;
            this.gameObject.SetActive(false);
            this.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ballPrefab.SetActive(true);
            shooting = true;
        }
    }

    void LaunchBall()
    {
        ballPrefab.transform.position = Vector2.MoveTowards(ballPrefab.transform.position, target.position, 0.2f);
    }
}
