using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }
    public GameObject SFX;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // singleton
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayAudio(AudioClip clip, bool looping = false)
    {
        GameObject sfx = Instantiate(SFX);
        sfx.GetComponent<AudioSource>().clip = clip;
        sfx.GetComponent<AudioSource>().loop = looping;
    }
}
