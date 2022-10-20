using UnityEngine;

public class AudioPlayer : Singleton<AudioPlayer>
{
    private AudioSource _audioSource;
    
    public void Play(AudioClip clip)
    {
        if (_audioSource == null)
        {
            _audioSource = FindObjectOfType<AudioSource>();
        }

        _audioSource.clip = clip;
        _audioSource.Play();
    }
}