using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("UI Sounds")]
    public AudioClip clickSound;
    public AudioClip usedSound;
    public AudioClip purchaseSound;

    [Header("Game Sounds")]
    public AudioClip completedSound;
    public AudioClip failedSound;
    public AudioClip coinSound;
    public AudioClip bombSound;
    public AudioClip bombCountDownSound;
    public AudioClip increaseSpeedSound;
    public AudioClip decreaseSpeedSound;
    
    [Header("AudioSource")]
    [SerializeField] private AudioSource audioSource;
    public AudioSource vehicleAudioSource;

    public void Awake()
    {
        if (Instance != null) 
        {
            DestroyImmediate(gameObject);
        } 
        else 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Start()
    {
        audioSource.Play();
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip, 3f);
    }
}
