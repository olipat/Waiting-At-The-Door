using UnityEngine;
using UnityEngine.Video;

public class VideoTrigger : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject videoDisplayUI; 

    private void Start()
    {
        videoDisplayUI.SetActive(false);
        videoPlayer.Stop(); 
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            videoDisplayUI.SetActive(true); 
            videoPlayer.Play();
        }
    }
}
