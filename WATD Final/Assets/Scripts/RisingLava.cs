using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class RisingLava : MonoBehaviour
{
    public float riseSpeed = 3f;
    public float fallSpeed = 6f;
    public bool isRising = false;
    public bool isFalling = false;
    public float lavaHeight = 100f;
    public Collider2D lavaDeathZone;

    public Vector3 initialPosition;
    public bool isRisingLava = false;

    private TilemapRenderer re;
    private bool queuedRise = false;


    private void Start()
    {
        initialPosition = transform.position;

        re = GetComponent<TilemapRenderer>();
        if (re != null)
            re.enabled = false;

        if (lavaDeathZone != null)
            lavaDeathZone.enabled = false;
    }

    public void BeginRising()
    {
        if (isFalling && isRisingLava)
        {
            // Delay rising until fall completes
            queuedRise = true;
            return;
        }

        StartRising();
    }

    
    private void StartRising()
    {
        isRising = true;

        if (re != null)
            re.enabled = true;

        if (lavaDeathZone != null)
            lavaDeathZone.enabled = true;
    }



    private void Update()
    {
        if (isRising && !isFalling)
        {
            if (transform.position.y < lavaHeight)
            {
                transform.position += Vector3.up * riseSpeed * Time.deltaTime;
            }
            else
            {
                isRising = false;
            }
        }
    }

    public void RetreatLava()
    {
        StartCoroutine(RetreatCoroutine());
    }

    private IEnumerator RetreatCoroutine()
    {
        isRising = false;
        isFalling = true;

        while (transform.position.y > initialPosition.y)
        {
            transform.position -= Vector3.up * fallSpeed * Time.deltaTime;
            yield return null;
        }

        isFalling = false;
        transform.position = initialPosition;

        if (re != null)
            re.enabled = false;

        if (lavaDeathZone != null)
            lavaDeathZone.enabled = false;

        //Resume rise if queued
        if (queuedRise)
        {
            queuedRise = false;
            StartRising();
        }
    }

}
