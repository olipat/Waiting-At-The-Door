using UnityEngine;

public class StalactiteCollisionRelay : MonoBehaviour
{
    private Stalactite parentStalactite;

    void Start()
    {
        parentStalactite = GetComponentInParent<Stalactite>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (parentStalactite != null)
        {
            parentStalactite.OnCollisionEnter2D(collision);
        }
    }
}
