using UnityEngine;

public class crawler : MonoBehaviour
{

    public float distance;
    public Transform groundDetection;
    public float speed;
    private bool movingRight = true;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);

        if (groundInfo.collider == false)
        {

            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if ((col.gameObject.tag == "Wall" || col.gameObject.tag == "Breakable Wall"))
        {
            //print("prtol collison with ground is working");
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else if (movingRight == false)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }
}
