using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager main;
    [Range(0, 1)]
    public float music_volume_max = 0.7f;

    AudioSource music;
    AudioSource sfx;

    public float musicVolume
    {
        get
        {
            if (!PlayerPrefs.HasKey("Music Volume"))
                return 0.7f;
            return PlayerPrefs.GetFloat("Music Volume");
        }
        set
        {
            PlayerPrefs.SetFloat("Music Volume", value);
        }
    }

    public float sfxVolume
    {
        get
        {
            if (!PlayerPrefs.HasKey("SFX Volume"))
                return 1f;
            return PlayerPrefs.GetFloat("SFX Volume");
        }
        set
        {
            PlayerPrefs.SetFloat("SFX Volume", value);
        }
    }

    public List<MusicTrack> tracks = new List<MusicTrack>();
    public List<Sound> sounds = new List<Sound>();
    Sound GetSoundByName(string name)
    {
        return sounds.Find(x => x.name == name);
    }

    static List<string> mixBuffer = new List<string>();
    static float mixBufferClearDelay = 0.05f;

    public bool mute = false;
    public bool quiet_mode = false;
    public string currentTrack;

    void Awake()
    {
        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (main != null)
        {
            Destroy(gameObject);
        }

        AudioSource[] sources = GetComponents<AudioSource>();
        music = sources[0];
        sfx = sources[1];

        // Initialize
        sfxVolume = sfxVolume;
        musicVolume = musicVolume;

        ChangeMusicVolume(musicVolume);
        ChangeSFXVolume(sfxVolume);

        StartCoroutine(MixBufferRoutine());

        mute = PlayerPrefs.GetInt("Mute") == 1;
    }

    // Corotina responsável por limitar a frequência de reprodução de sons
    IEnumerator MixBufferRoutine()
    {
        float time = 0;

        while (true)
        {
            time += Time.unscaledDeltaTime;
            yield return 0;
            if (time >= mixBufferClearDelay)
            {
                mixBuffer.Clear();
                time = 0;
            }
        }
    }

    // Iniciando uma faixa de música
    public void PlayMusic(string trackName)
    {
        if (trackName != "")
            currentTrack = trackName;
        AudioClip to = null;
        foreach (MusicTrack track in tracks)
            if (track.name == trackName)
                to = track.track;
        StartCoroutine(main.CrossFade(to));
    }

    // Uma transição suave de uma música para outra
    IEnumerator CrossFade(AudioClip to)
    {
        float delay = 0.3f;
        if (music.clip != null)
        {
            while (delay > 0)
            {
                music.volume = delay * musicVolume * music_volume_max;
                delay -= Time.unscaledDeltaTime;
                yield return 0;
            }
        }
        music.clip = to;
        if (to == null || mute)
        {
            music.Stop();
            yield break;
        }
        delay = 0;
        if (!music.isPlaying) music.Play();
        while (delay < 0.3f)
        {
            music.volume = delay * musicVolume * music_volume_max;
            delay += Time.unscaledDeltaTime;
            yield return 0;
        }
        music.volume = musicVolume * music_volume_max;
    }

    // Um único efeito sonoro
    public void Shot(string clip)
    {
        Sound sound = main.GetSoundByName(clip);

        if (sound != null && !mixBuffer.Contains(clip))
        {
            if (sound.clips.Count == 0) return;
            mixBuffer.Add(clip);
            main.sfx.PlayOneShot(sound.clips.GetRandom());
        }
    }

    // Ligar / desligar música
    public void MuteButton()
    {
        mute = !mute;
        //PlayerPrefs.SetInt("Mute", mute ? 1 : 0);
        //PlayerPrefs.Save();
        PlayMusic(mute ? "" : currentTrack);
    }

    // Slide de volume
    public void ChangeMusicVolume(float v)
    {
        musicVolume = v;
        music.volume = musicVolume * music_volume_max;
    }

    // Slide de volume
    public void ChangeSFXVolume(float v)
    {
        sfxVolume = v;
        sfx.volume = sfxVolume;
    }

    /// <summary>
    /// Transição da velocidade da musica
    /// </summary>
    /// <param name="newPitch"></param>
    /// <returns></returns>
    IEnumerator CrossPitch(float newPitch)
    {
        float delay = 0.2f;
        while (delay > 0)
        {
            delay = delay - 1f * Time.deltaTime;
            music.pitch = Mathf.Lerp(music.pitch, newPitch, delay / 1f);
            yield return null;
        }

        music.pitch = newPitch;
    }

    [System.Serializable]
    public class MusicTrack
    {
        public string name;
        public AudioClip track;
    }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public List<AudioClip> clips = new List<AudioClip>();
    }
}

public static class Ultilitario
{
    public static T GetRandom<T>(this ICollection<T> collection)
    {
        if (collection == null)
            return default(T);
        int t = UnityEngine.Random.Range(0, collection.Count);

        foreach (T element in collection)
        {
            if (t == 0)
                return element;
            t--;
        }

        return default(T);
    }
}
