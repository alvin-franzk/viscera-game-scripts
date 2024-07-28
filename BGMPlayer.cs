using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioClip[] soundtrack;
    public float volume;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        volume = 0.15f;
        if (!_audioSource.isPlaying)
        {
            ChangeSong(Random.Range(0, soundtrack.Length));
        }
    }

    // Update is called once per frame
    void Update()
    {
        _audioSource.volume = volume;

        if (!_audioSource.isPlaying)
        {
            ChangeSong(Random.Range(0, soundtrack.Length));
        }
    }

    private void ChangeSong(int songPicked)
    {
        _audioSource.clip = soundtrack[songPicked];
        _audioSource.Play();
    }
}
