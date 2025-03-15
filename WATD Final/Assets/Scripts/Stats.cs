using UnityEngine;

[System.Serializable]
public struct SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public Vector3 GetPos()
    {
        return new Vector3(x, y, z);
    }

    public void SetPos(Vector3 pos)
    {
        x = pos.x; y = pos.y; z = pos.z;
    }
}


[System.Serializable]
public class Stats
{
    public int health = 3;
    public int level = 1;
    public SerializableVector3 myPos;
}
