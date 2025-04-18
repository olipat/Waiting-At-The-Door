using UnityEngine;

public class ToastFollower : MonoBehaviour
{
    public Transform player;
    public ToastNotification toastNotification;
    public float yOffset = -500f;

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        if (toastNotification == null)
            toastNotification = GetComponent<ToastNotification>();
    }

    private void Update()
    {
        if (player != null && toastNotification != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(player.position);

            // Screen height - player Y gives distance from top of screen
            float distanceFromTop = Screen.height - screenPos.y;

            // Add optional vertical offset
            ToastNotification.margin = new Vector2(0, distanceFromTop + yOffset);
        }
    }
}
