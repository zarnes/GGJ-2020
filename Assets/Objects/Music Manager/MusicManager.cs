using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    internal static MusicManager Instance;

    [SerializeField]
    private AudioSource _musicSource;
    [SerializeField]
    private float _musicSwitchTime = 1f;
    [SerializeField]
    private List<CachedSoundConfig> _sounds;
    
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public bool PlayMusic(string musicName)
    {
        CachedSoundConfig config = Instance._sounds.Find(s => s.Name == musicName);
        if (config == null)
            return false;

        Instance.StartCoroutine(PlayMusicCoroutine(config));
        return true;
    }

    private IEnumerator PlayMusicCoroutine(CachedSoundConfig config)
    {
        float start = Time.time;
        float end = start + _musicSwitchTime / 2f;
        float current = start;

        float baseVolume = _musicSource.volume;

        while (current <= end)
        {
            current = Time.time;
            float percentage = Mathf.InverseLerp(start, end, current);
            float volume = Mathf.Lerp(baseVolume, 0, percentage);

            yield return null;
        }

        start = Time.time;
        end = start + _musicSwitchTime / 2f;
        current = start;

        _musicSource.clip = config.Clip;

        while (current <= end)
        {
            current = Time.time;
            float percentage = Mathf.InverseLerp(start, end, current);
            float volume = Mathf.Lerp(0, config.Volume, percentage);

            yield return null;
        }

        _musicSource.volume = config.Volume;
    }

    public bool PlaySound(string soundName)
    {
        CachedSoundConfig config = Instance._sounds.Find(s => s.Name == soundName);
        if (config == null)
            return false;
        
        return PlaySound(config);
    }

    public bool PlaySound(SoundConfig config)
    {
        AudioSource source = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        source.volume = config.Volume;
        source.clip = config.Clip;
        source.Play();
        Destroy(source, config.Clip.length + .2f);

        return true;
    }

    [System.Serializable]
    public class SoundConfig
    {
        public AudioClip Clip;
        public float Volume = 1;
    }

    [System.Serializable]
    public class CachedSoundConfig : SoundConfig
    {
        public string Name;
    }
}
