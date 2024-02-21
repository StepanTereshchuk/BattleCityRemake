using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private SoundManagerData _soundManagerData;
    private List<AudioSource> _audioSourcesPool;
    private Transform _poolTransform;
    private Sound _desiredSound;

    private void Awake()
    {
        CreatePoolAudioSources();
    }
    private void CreatePoolAudioSources(int SourceCount = 0)
    {
        _audioSourcesPool = new List<AudioSource>();
        _poolTransform = new GameObject("PoolAudioSources" + (_audioSourcesPool.Count + 1)).transform;
        _poolTransform.parent = transform;
        while (SourceCount >= 1)
        {
            CreateAdditionalSource();
        }

    }

    public void PlaySound(SoundKey soundKey)
    {
        _desiredSound = _soundManagerData.GetSoundData(soundKey);
        int flag = 0;
        foreach (var source in _audioSourcesPool)
        {
            if (source && !source.isPlaying)
            {
                UseAudioSource(source); flag = 1; break;
            }
            else if (source && source.loop && _desiredSound.Looping)
            {
                if (CheckForReplacement(source))
                {
                    UseAudioSource(source);
                    flag = 1;
                    break;
                }
                else if (_desiredSound.AudioClip.Equals(source.clip))
                {
                    flag = 1;
                    break;
                }
            }
        }
        if (flag == 0)
        {
            UseAudioSource(CreateAdditionalSource());
        }
    }

    public void StopSound(SoundKey soundKey)
    {
        if (_audioSourcesPool.Count > 0)
        {
            _desiredSound = _soundManagerData.GetSoundData(soundKey);
            foreach (var source in _audioSourcesPool)
            {
                if (source && source.isPlaying)
                {
                    if (source.clip.Equals(_desiredSound.AudioClip))
                    {
                        source.Stop(); break;
                    }
                }
            }
        }
    }

    public IEnumerator PlaySoundAndWaitFinish(SoundKey soundKey)
    {
        PlaySound(soundKey);
        AudioSource playedAudioSource = null;
        foreach (var source in _audioSourcesPool)
        {
            if (source.isPlaying)
            {
                if (_desiredSound.AudioClip.Equals(source.clip))
                    playedAudioSource = source;
            }
        }
        while (playedAudioSource != null && playedAudioSource.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return true;
    }

    public float GetSoundLenght(SoundKey soundKey)
    {
        return _soundManagerData.GetSoundData(soundKey).Lenght;
    }

    private bool CheckForReplacement(AudioSource source)
    {
        if (source.outputAudioMixerGroup.Equals(_desiredSound.AudioMixerGroup))
        {
            if (_desiredSound.LayerType.Equals(LayerType.Single)
                && !_desiredSound.AudioClip.Equals(source.clip))
                return true;
        }
        return false;
    }

    private AudioSource CreateAdditionalSource()
    {
        GameObject newAudioSourceObj = new GameObject("AudioSource" + (_audioSourcesPool.Count + 1));
        newAudioSourceObj.transform.parent = _poolTransform;

        newAudioSourceObj.AddComponent<AudioSource>();
        AudioSource newAudioSource = newAudioSourceObj.GetComponent<AudioSource>();
        _audioSourcesPool.Add(newAudioSource);
        return newAudioSource;
    }

    private void UseAudioSource(AudioSource source)
    {
        if (_desiredSound != null)
        {
            source.clip = _desiredSound.AudioClip;
            source.loop = _desiredSound.Looping;
            source.outputAudioMixerGroup = _desiredSound.AudioMixerGroup;
            source.Play();
        }
        else
            Debug.Log("DESIRED SOUND NULL :(");
    }
}