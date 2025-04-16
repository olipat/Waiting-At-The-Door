using UnityEngine;

public class ProximityHintManager : MonoBehaviour
{
    public Transform player;
    public Transform[] hintTargets;
    public string[] hintMessages;
    public float triggerDistance = 3f;

    private int lastHintIndex = -1;

    void Update()
    {
        for (int i = 0; i < hintTargets.Length; i++)
        {
            float dist = Vector3.Distance(player.position, hintTargets[i].position);

            if (dist <= triggerDistance && lastHintIndex != i)
            {
                if (i == 4)
                {
                    ToastNotification.Show(hintMessages[i], 4f, "alert");
                    lastHintIndex = i;
                    break; // only show one at a time
                }
                else
                {
                    ToastNotification.Show(hintMessages[i], 4f, "info");
                    lastHintIndex = i;
                    break; // only show one at a time
                }
            }

            if (dist > triggerDistance && lastHintIndex == i)
            {
                ToastNotification.Hide();
                lastHintIndex = -1;
            }
        }
    }
}
