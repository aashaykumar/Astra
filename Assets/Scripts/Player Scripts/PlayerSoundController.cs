using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public AudioSource source;
    public AudioClip PlayerWalk;
    public AudioClip PlayerShoot;
    public AudioClip PlayerDie;

    private void Awake()
    {
       float tempSoundValue = systems.Instance.GetComponentInChildren<AudioSystem>().SoundEffectsFloat;
        source.volume = tempSoundValue;
    }
    void WalkSound()
    {
        source.clip = PlayerWalk;
        source.Play();
    }

    void DeadSound() 
    {
        source.clip = PlayerDie;
        source.Play();
    }

    void ShootSound()
    {
        source.clip = PlayerShoot;
        source.Play();
    }
}
