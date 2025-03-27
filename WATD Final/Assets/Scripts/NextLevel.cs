using UnityEngine;

public class NextLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        LevelLoader.Instance.LoadNextLevel();
    }   
}
