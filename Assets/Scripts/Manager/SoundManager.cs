using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    public GameObject SoundContainer { get; private set; }

    public Dictionary<string, AudioSource> AudioData { get; private set; }

    protected override void Initialize()
    {
        AudioData = new Dictionary<string, AudioSource>();
    }

    private void Awake()
    {
        this.Initialize();

        SoundContainer = GameObject.Find("SoundContainer");
    }

    public void CreateSound(Dictionary<string, AudioClip> audioClipData)
    {
        foreach (KeyValuePair<string, AudioClip> p in audioClipData)
        {
            GameObject obj = new GameObject("Audio_" + p.Key);
            obj.AddComponent<AudioSource>().clip = p.Value;
            AudioSource audioValue = obj.GetComponent<AudioSource>();
            AudioData.Add(p.Key, audioValue);
            obj.transform.parent = SoundContainer.transform;
        }
    }

    public void PlaySound(string soundName)
    {
        if (AudioData.ContainsKey(soundName))
        {
            AudioData[soundName].Play();
        }
    }

    public void PlayOneShotSound(string soundName, float volume = 1.0f)
    {
        if (AudioData.ContainsKey(soundName))
        {
            AudioData[soundName].PlayOneShot(AudioData[soundName].clip, volume);
        }
    }
}
