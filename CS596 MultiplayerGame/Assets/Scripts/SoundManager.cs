using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioSource RunningFX;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySound(AudioClip audioclip, Transform spawnTransform, float volume)
    {
        AudioSource audiosource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        
        audiosource.clip = audioclip;

        audiosource.volume = volume;
        
        audiosource.Play();

        float cliplength = audiosource.clip.length;

        Destroy(audiosource.gameObject, cliplength);
    }
    public void RunSound(AudioClip audioclip, Transform spawnTransform, float volume)
    {
        AudioSource audiosource = Instantiate(RunningFX, spawnTransform.position, Quaternion.identity);

        audiosource.clip = audioclip;

        audiosource.volume = volume;

        audiosource.Play();

        float cliplength = audiosource.clip.length;
        
        Destroy(audiosource.gameObject, cliplength);
    }
}
