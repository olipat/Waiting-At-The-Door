using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            instance.StopMusic();
            Destroy(gameObject);
            return;
        }
    }




    public AudioSource menuMusic;
    public bool playingMenuMusic;
    public AudioSource[] bgm;
    public int currentBGM;
    public bool playingBGM;

    public bool BGMScene = false;

    public AudioSource[] sfx;



    // Start is called before the first frame update
    void Start()
    {
        
       
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentLevel > 0)
        {
            BGMScene = true;
        }

        if (playingBGM)
        {
            if (bgm[currentBGM].isPlaying == false)
            {
                currentBGM++;
                if (currentBGM >= bgm.Length)
                {
                    currentBGM = 0;
                }


                bgm[currentBGM].Play();
            }
        }
    }

    public void StopMusic()
    {
        menuMusic.Stop();
     
        foreach (AudioSource track in bgm)
        {
            track.Stop();
        }

        playingBGM = false;
    }

    public void PlayMenuMusic()
    {
        StopMusic();
        menuMusic.Play();
        playingMenuMusic = true;
        GameManager.instance.currentLevel = 0;
    }

    public void PlayBGM()
    {
        StopMusic();

        currentBGM = GameManager.instance.currentLevel - 1;

        bgm[currentBGM].Play();
        playingBGM = true;
        playingMenuMusic = false;
    }

    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
    }
   
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            //StopAllCoroutines(); // Cancel any ongoing coroutine to avoid conflicts

            if (BGMScene == true)
            {
                StartCoroutine(WaitBeforeResumingBGM(3f)); // Wait 3 seconds before setting playingBGM to true
            }

        }
        else
        {
            playingBGM = false;
        }

    }

    IEnumerator WaitBeforeResumingBGM(float delay)
    {
        yield return new WaitForSeconds(delay);
        playingBGM = true;
    }
    
}
   