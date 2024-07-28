using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource soundObject; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySound(AudioClip audioClip, Transform spawnTransform, float volume) // NOTE: add float volume parameter for future audio settings shenanigans
    {
        // instantiate object
        AudioSource audioSource = Instantiate(soundObject, spawnTransform.position, Quaternion.identity);

        // pass sound clip
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlaySoundRandom(AudioClip[] audioClip, Transform spawnTransform, float volume) // NOTE: add float volume parameter for future audio settings shenanigans
    {
        int rand = Random.Range(0, audioClip.Length);
        // instantiate object
        AudioSource audioSource = Instantiate(soundObject, spawnTransform.position, Quaternion.identity);

        // pass sound clip
        audioSource.clip = audioClip[rand];
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
}
