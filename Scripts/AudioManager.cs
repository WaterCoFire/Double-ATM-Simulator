using UnityEngine;

// Manages the audio playing for the application
public class AudioManager : MonoBehaviour {
    public static AudioManager instance;
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Awake() {
        // Ensure that there is only one AudioManager instance
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null) {
                Debug.LogError("AudioSource component is missing on AudioManager GameObject.");
            }

            if (clickSound == null) {
                Debug.LogError("Click sound is not assigned in AudioManager.");
            }
        } else {
            Destroy(gameObject);
        }
    }

    // Play the sound with given path
    public void PlayClickSound(string soundPath) {
        AudioClip clip = Resources.Load<AudioClip>(soundPath);
        if (clip != null) {
            audioSource.PlayOneShot(clip);
        } else {
            Debug.LogError($"AudioClip not found at path: {soundPath}");
        }
    }
}