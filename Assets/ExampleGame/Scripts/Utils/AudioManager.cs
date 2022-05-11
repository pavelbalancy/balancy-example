using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip _buttonClick;

    private AudioSource _audioSource;

    private static AudioManager _instance;
    
    private void Awake()
    {
        _instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public static void PlayButtonClick()
    {
        _instance?.PlaySound(_instance?._buttonClick);
    }

    private void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
