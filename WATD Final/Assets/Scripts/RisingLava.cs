using UnityEngine;
using UnityEngine.Tilemaps;

public class RisingLava : MonoBehaviour
{
    public float riseSpeed = 3f;
    private bool isRising = false;
    public Collider2D lavaDeathZone;

    private TilemapRenderer re;

    private void Start()
    {
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
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;
        }
    }
}
