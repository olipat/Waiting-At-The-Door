using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class RisingLava : MonoBehaviour
{
    public float riseSpeed = 3f;
    public float fallSpeed = 6f;
    public bool isRising = false;
    public float lavaHeight = 100f;
    public Collider2D lavaDeathZone;

    public Vector3 initialPosition;

    private TilemapRenderer re;

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
        isRising = true;
        if (re != null)
            re.enabled = true;

        if (lavaDeathZone != null)
            lavaDeathZone.enabled = true;
    }

    private void Update()
    {
        if (isRising)
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
        isRising = false; // cancel rising if it's happening

        while (transform.position.y > initialPosition.y)
        {
            transform.position -= Vector3.up * fallSpeed * Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition;

        if (re != null)
            re.enabled = false;

        if (lavaDeathZone != null)
            lavaDeathZone.enabled = false;
    }
}
