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
    public AudioSource[] bossMusic;
    public AudioSource[] cutSceneMusic;
    public int currentBGM;
    public bool playingBGM;
    public bool playingBossMusic;
    public bool playingCutsceneMusic;

    public bool BGMScene = false;

    public AudioSource[] sfx;

    public int cutSceneNum = 0;


    // Start is called before the first frame update
    void Start()
    {
        //This needs preloading to stop buffering
        for(int i = 0; i < bossMusic.Length; i++)
        {
            bossMusic[i].Play();
            bossMusic[i].Stop();
        }
        

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
                bgm[currentBGM].Play();
            }
        }
        else if (playingBossMusic)
        {
            if(bossMusic[currentBGM].isPlaying == false)
            {
                bossMusic[currentBGM].Play();
            }
        }
        else if (playingCutsceneMusic)
        {
            if (cutSceneMusic[cutSceneNum].isPlaying == false)
            {
                cutSceneMusic[cutSceneNum].Play();
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

        foreach (AudioSource track in bossMusic)
        {
            track.Stop();
        }
        foreach (AudioSource track in cutSceneMusic)
        {
            track.Stop();
        }

        playingBGM = false;
        playingBossMusic = false;
        playingCutsceneMusic = false;
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
        playingBossMusic = false;
        playingCutsceneMusic = false;
    }

    public void PlayBossMusic()
    {
        StopMusic();

        bossMusic[currentBGM].Play();
        playingBossMusic = true;
        playingBGM = false;
        playingMenuMusic = false;
        playingCutsceneMusic = false;
    }

    public void PlayCutsceneMusic(int cutSceneNum)
    {
        StopMusic();

        cutSceneMusic[cutSceneNum].Play();
        playingBossMusic = false;
        playingBGM = false;
        playingMenuMusic = false;
        playingCutsceneMusic = true;
    }

    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
    }

    /*
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
     */

    void OnApplicationFocus(bool hasFocus)
    {
        UIController.Instance.Pause();
    }
}
   