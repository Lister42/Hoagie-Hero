using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioList : MonoBehaviour
{
    AudioSource _source;

    #region Sounds
    public AudioClip _chop;
    public AudioClip _cooking;
    public AudioClip _toasting;
    public AudioClip _running;
    public AudioClip _pickup;
    public AudioClip _place;
    public AudioClip _levelOneBackground;
    public AudioClip _levelTwoBackground;
    public AudioClip _levelThreeBackground;
    public AudioClip _titleScreenBackground;
    public AudioClip _endSceneBackground;
    public AudioClip _correctOrder;
    public AudioClip _wrongOrder;
    #endregion

    #region Sound is Playing Variables
    public bool _isChopping;
    public bool _isRunning;
    #endregion

    private bool _playLevelOneSongAgain;
    private bool _playLevelTwoSongAgain;
    private bool _playLevelThreeSongAgain;
    private bool _levelEnd; //used to end all tracks when game is over

    private bool _playCookingSoundAgain;
    private bool _playToastingSoundAgain;

    // Start is called before the first frame update
    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _isChopping = false;
        _isRunning = false;
        _playLevelOneSongAgain = false;
        _playLevelTwoSongAgain = false;
        _playLevelThreeSongAgain = false;
        _playCookingSoundAgain = true;
        _playToastingSoundAgain = true;
    }
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    { 
        if (_playLevelOneSongAgain)
        {
            PlayLevelOneBackgroundMusic();
        }
        if (_playLevelTwoSongAgain)
        {
            PlayLevelTwoBackgroundMusic();
        }
        if (_playLevelThreeSongAgain)
        {
            PlayLevelThreeBackgroundMusic();
        }
    }

    public void PlayChopAudio()
    {
        _source.PlayOneShot(_chop);
    }

    public void PlayCorrectOrderAudio()
    {
        _source.PlayOneShot(_correctOrder);
    }
    public void PlayWrongOrderAudio()
    {
        _source.PlayOneShot(_wrongOrder);
    }
    public void PlayRunningAudio()
    {
        _source.PlayOneShot(_running);
    }

    public void PlayCookingAudio()
    {
        if (_playCookingSoundAgain)
        {
            _source.PlayOneShot(_cooking);
            StartCoroutine(CookSoundWait());
        }
    }

    public void PlayToastingAudio()
    {
        if (_playToastingSoundAgain)
        {
            _source.PlayOneShot(_toasting);
            StartCoroutine(ToastSoundWait());
        }
    }

    public void PlayPickupAudio()
    {
        _source.PlayOneShot(_pickup);
    }

    public void PlayPlaceAudio()
    {
        _source.PlayOneShot(_place);
    }

    public void PlayLevelOneBackgroundMusic()
    {
        _source.PlayOneShot(_levelOneBackground);
        StartCoroutine(LevelOneSongWait());
    }
    
    public void PlayLevelTwoBackgroundMusic()
    {
        _source.PlayOneShot(_levelTwoBackground);
        StartCoroutine(LevelTwoSongWait());
    }

    public void PlayLevelThreeBackgroundMusic()
    {
        _source.PlayOneShot(_levelThreeBackground);
        StartCoroutine(LevelThreeSongWait());
    }

    public void PlayTitleScreenBackgroundMusic()
    {
        _source.PlayOneShot(_titleScreenBackground);
    }

    private IEnumerator LevelOneSongWait()
    {
        _playLevelOneSongAgain = false;
        yield return new WaitForSeconds(137);
        _playLevelOneSongAgain = true;
    }

    private IEnumerator LevelTwoSongWait()
    {
        _playLevelTwoSongAgain = false;
        yield return new WaitForSeconds(207);
        _playLevelTwoSongAgain = true;
    }

    private IEnumerator LevelThreeSongWait()
    {
        _playLevelThreeSongAgain = false;
        yield return new WaitForSeconds(145);
        _playLevelThreeSongAgain = true;
    }

    private IEnumerator CookSoundWait()
    {
        _playCookingSoundAgain = false;
        yield return new WaitForSeconds(4);
        _playCookingSoundAgain = true;
    }

    private IEnumerator ToastSoundWait()
    {
        _playToastingSoundAgain = false;
        yield return new WaitForSeconds(0.4f);
        _playToastingSoundAgain = true;
    }

    public void PlayEndSceneBackgroundMusic()
    {
        _source.PlayOneShot(_endSceneBackground);
    }

    public void Stop()
    {
        _source.Stop();
    }
}
