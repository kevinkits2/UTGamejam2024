using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioClipGroup")]
public class AudioClipGroup : ScriptableObject
{
    public List<AudioClip> AudioClips;
    [Range(0, 2)]
    public float VolumeMin = 1;
    [Range(0, 2)]
    public float VolumeMax = 1;
    [Range(1, 2)]
    public float PitchMin = 1;
    [Range(1, 2)]
    public float PitchMax = 1;
    public float Cooldown = 0.02f;
    public bool isLooped = false;

    static float nextPlayTime = 0;
    public void Play(AudioSource source)
    {
        if (Time.timeScale >= 0.1f) {
            if (nextPlayTime > Time.time) return;
        }
        source.clip = AudioClips[Random.Range(0, AudioClips.Count)];
        source.volume = Random.Range(VolumeMin, VolumeMax);
        source.pitch = Random.Range(PitchMin, PitchMax);
        Debug.Log("Watafak??!?");
        source.Play();
        nextPlayTime = Time.time + Cooldown;
    }
    public void Play()
    {
        if (isLooped)
        {
            AudioSourcePool.Instance.AudioSourcePrefab.loop = true;
            Play(AudioSourcePool.Instance.GetLooped());
        }
        else
        {
            AudioSourcePool.Instance.AudioSourcePrefab.loop = false;
            Play(AudioSourcePool.Instance.GetSource());
        }
        Play(AudioSourcePool.Instance.GetSource());
        AudioSourcePool.Instance.AudioSourcePrefab.loop = false;
    }
}