using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AudioClipsSO;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private int maxSimultaneousAudios = 10;

    private List<AudioSource> audioSources;
    public AudioClipsSO audioClipsSO;

    AudioClipData audioClipData;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioSources()
    {
        audioSources = new List<AudioSource>();
        for (int i = 0; i < maxSimultaneousAudios; i++)
        {
            audioSources.Add(gameObject.AddComponent<AudioSource>());
        }
    }

    public void SetVolumeOfCategory(string category, float volume)
    {
        var categoryData = audioClipsSO.audioCategories.Find(cat => cat.category == category);
        if (categoryData != null)
        {
            foreach (var clipData in categoryData.audioClips)
            {
                clipData.volume = volume;
            }
        }
        var audioSource = audioSources.Find(source => source.clip != null && categoryData.audioClips.Any(clip => clip.audioClip == source.clip));
        audioSource.volume = volume;
    }
    public void PlayAudioClip(string category, string clipName, bool loop = false)
    {
        AudioClipData clip = audioClipsSO.GetAudioClipData(category, clipName);

        if (clip == null)
        {
            Debug.LogWarning($"Audio clip '{clipName}' in category '{category}' not found!");
            return;
        }
        AudioSource availableAudioSource = GetAvailableAudioSource();

        if (availableAudioSource == null)
        {
            Debug.LogWarning("No available audio sources. Cannot play audio.");
            return;
        }
        availableAudioSource.volume = clip.volume;
        availableAudioSource.clip = clip.audioClip;
        availableAudioSource.loop = loop;
        availableAudioSource.Play();
    }

    public AudioSource PlayAudioClipAndGetSource(string category, string clipName, bool loop = false)
    {
        AudioClipData clip = audioClipsSO.GetAudioClipData(category, clipName);

        if (clip == null)
        {
            Debug.LogWarning($"Audio clip '{clipName}' in category '{category}' not found!");
            return null;
        }

        AudioSource availableAudioSource = GetAvailableAudioSource();

        if (availableAudioSource == null)
        {
            Debug.LogWarning("No available audio sources. Cannot play audio.");
            return null;
        }
        availableAudioSource.Stop();

        availableAudioSource.clip = clip.audioClip;
        availableAudioSource.loop = loop;
        availableAudioSource.Play();

        return availableAudioSource;
    }

    public void StopAllAudio()
    {
        foreach (var source in audioSources)
        {
            source.Stop();
        }
    }

    public bool IsAnyPlaying()
    {
        return audioSources.Exists(source => source.isPlaying);
    }

    private AudioSource GetAvailableAudioSource()
    {
        return audioSources.Find(source => !source.isPlaying);
    }
}
