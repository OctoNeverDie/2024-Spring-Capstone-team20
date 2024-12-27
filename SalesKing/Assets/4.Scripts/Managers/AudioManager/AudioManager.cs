using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private List<Sound> musicSounds = new List<Sound>();
    private List<Sound> sfxSounds = new List<Sound>();
    private bool isMusicMuted = false;


    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        // 싱글톤 관리
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 오브젝트 유지
            LoadSounds(); // 사운드 자동 로드
        }
        else
        {
            Destroy(gameObject); // 중복 생성 방지
        }
    }

    private void LoadSounds()
    {
        // Resources 폴더에서 음악 및 효과음 자동 로드
        AudioClip[] musicClips = Resources.LoadAll<AudioClip>("Audio/Music");
        AudioClip[] sfxClips = Resources.LoadAll<AudioClip>("Audio/SFX");

        foreach (var clip in musicClips)
        {
            musicSounds.Add(new Sound(clip.name, clip));
        }

        foreach (var clip in sfxClips)
        {
            sfxSounds.Add(new Sound(clip.name, clip));
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (sceneName == "Start")
        {
            PlayMusic("Start");
        }
        else if (sceneName == "CityMap" || sceneName == "InfiniteNPCs")
        {
            StopMusic();
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = musicSounds.Find(x => x.name == name);

        if (s == null)
        {
            Debug.LogError($"Music '{name}' 없음");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }
    public void StopMusic()
    {
        musicSource.Stop(); // 음악 재생 중단
    }
    public void PlaySFX(string name)
    {
        Sound s = sfxSounds.Find(x => x.name == name);

        if (s == null)
        {
            Debug.LogError($"SFX '{name}' 없음");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
    public void ToggleMusicMute()
    {
        isMusicMuted = !isMusicMuted; // 뮤트 상태 변경
        musicSource.mute = isMusicMuted;
    }
}
