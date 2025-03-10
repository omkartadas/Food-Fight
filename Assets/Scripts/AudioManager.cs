using UnityEngine;

public class AudioManager : MonoBehaviour {
    private static AudioManager instance;
    private AudioSource audioSource;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (!audioSource.isPlaying) {
            audioSource.Play(); // Play only if not already playing
        }
    }
}
