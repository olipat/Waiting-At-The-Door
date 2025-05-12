using System.Collections;
using UnityEngine;

public class StoplightBeam : MonoBehaviour
{
    public float timeToTrigger = 1f;

    public GameObject redLightLeft;
    public GameObject redLightRight;
    public GameObject rollingBallPrefab;

    private Transform spawnPointLeft;
    private Transform spawnPointRight;

    private bool facingRight = false;
    private bool isPlayerInLight = false;
    private float timeInLight = 0f;

    private void Start()
    {
        spawnPointLeft = redLightLeft.transform.Find("spawnLeft");
        spawnPointRight = redLightRight.transform.Find("spawnRight");

        SetFacingDirection(false);
        StartCoroutine(FlipRoutine());
    }

    public void OnPlayerEnter()
    {
        isPlayerInLight = true;
        timeInLight = 0f;
        StartCoroutine(PlayerLightCheck());
    }

    public void OnPlayerExit()
    {
        isPlayerInLight = false;
        timeInLight = 0f;
    }

    private IEnumerator PlayerLightCheck()
    {
        while (isPlayerInLight)
        {
            timeInLight += Time.deltaTime;

            if (timeInLight >= timeToTrigger)
            {
                Transform spawnPoint = facingRight ? spawnPointRight : spawnPointLeft;
                GameObject tire = Instantiate(rollingBallPrefab, spawnPoint.position, Quaternion.identity);
                tire.GetComponent<Projectile>().direction = facingRight ? 1 : -1;

                Debug.Log("Spawned from " + (facingRight ? "Right" : "Left"));
                timeInLight = 0f;
            }

            yield return null;
        }
    }

    private IEnumerator FlipRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            SetFacingDirection(!facingRight);
        }
    }

    private void SetFacingDirection(bool right)
    {
        facingRight = right;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (facingRight ? 1 : -1);
        transform.localScale = scale;

        redLightLeft.SetActive(!facingRight);
        redLightRight.SetActive(facingRight);
    }
}


