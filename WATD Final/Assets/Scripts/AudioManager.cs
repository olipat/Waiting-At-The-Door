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
    public AudioSource battleSelectMusic;
    public AudioSource[] bgm;
    public int currentBGM;
    public bool playingBGM;

    public bool BGMScene = false;

    public AudioSource[] sfx;




    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayMenuMusic();
    }

    // Update is called once per frame
    void Update()
    {
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
        battleSelectMusic.Stop();
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
    }

    public void PlayBattleSelectMusic()
    {
        if (battleSelectMusic.isPlaying == false)
        {
            StopMusic();
            battleSelectMusic.Play();
        }
    }

    public void PlayBGM()
    {
        StopMusic();

        currentBGM = Random.Range(0, bgm.Length);

        bgm[currentBGM].Play();
        playingBGM = true;
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
}
   