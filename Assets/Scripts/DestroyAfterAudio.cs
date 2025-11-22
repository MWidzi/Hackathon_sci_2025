using UnityEngine;
using System.Collections;

public class DestroyAfterAudio : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        if (!audioSource.loop)
            StartCoroutine(DestroyWhenFinished());
    }

    private IEnumerator DestroyWhenFinished()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }
}
