using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip background;
    public AudioClip death;
    public AudioClip win;
    public AudioClip coin;
    public AudioClip enemyDeath;
    public AudioClip bomb;
    public AudioClip save;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
    public void playSFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}

