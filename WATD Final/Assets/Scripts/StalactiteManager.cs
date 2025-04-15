using UnityEngine;

public class StalactiteManager : MonoBehaviour
{
    public static StalactiteManager instance;

    public Stalactite[] stalactites;

    private Vector3[] topPositions;
    private Vector3[] bottomPositions;
    private Sprite[] originalTopSprites;
    private Sprite[] originalBottomSprites;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        int length = stalactites.Length;
        topPositions = new Vector3[length];
        bottomPositions = new Vector3[length];
        originalTopSprites = new Sprite[length];
        originalBottomSprites = new Sprite[length];

        for (int i = 0; i < length; i++)
        {
            if (stalactites[i] != null)
            {
                topPositions[i] = stalactites[i].topPiece.transform.position;
                bottomPositions[i] = stalactites[i].bottomPiece.transform.position;
                originalTopSprites[i] = stalactites[i].topPiece.GetComponent<SpriteRenderer>().sprite;
                originalBottomSprites[i] = stalactites[i].bottomPiece.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }

    public void ResetAllStalactites()
    {
        for (int i = 0; i < stalactites.Length; i++)
        {
            if (stalactites[i] != null)
            {
                stalactites[i].ResetStalactite(topPositions[i], bottomPositions[i], originalTopSprites[i], originalBottomSprites[i]);
            }
        }
    }
}
