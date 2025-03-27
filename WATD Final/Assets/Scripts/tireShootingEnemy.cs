using UnityEngine;

public class tireShootingEnemy : MonoBehaviour
{
   // public Transform pointA, pointB;
    //public float patrolSpeed = 2f;
    public float detectionRange = 5f;
    public float attackCooldown = 1f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public Transform player;

    private Vector3 targetPosition;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    void Start()
    {
       // targetPosition = pointA.position;
    }

    void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);

        if (playerDistance <= detectionRange)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        if (isAttacking)
        {
            AttackPlayer();
        }
        //else
        //{
        //    Patrol();
        //}
    }

    //void Patrol()
    //{
    //    transform.position = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);

    //    if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
    //    {
    //        targetPosition = (targetPosition == pointA.position) ? pointB.position : pointA.position;
    //    }
    //}

    void AttackPlayer()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            ShootProjectile();
            attackTimer = 0f;
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null && UIController.Instance.playerHealth > 0)
        {
            Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        }
    }
}
