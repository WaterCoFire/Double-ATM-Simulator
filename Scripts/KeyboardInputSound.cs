using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the effect when keyboard pressed
public class KeyboardInputSound : MonoBehaviour {
    private string soundPath = "ButtonPress";
    public List<GameObject> numKeyboardEffects;

    public bool isMouseActive;

    void Update() {
        if (!isMouseActive) return;

        if (Input.anyKeyDown) {
            // Backspace key pressed
            if (Input.GetKeyDown(KeyCode.Backspace)) {
                AudioManager.instance.PlayClickSound(soundPath);
                return;
            }

            // Number key pressed
            for (int i = 0; i <= 9; i++) {
                if (Input.GetKeyDown(i.ToString()) ||
                    Input.GetKeyDown((KeyCode)(KeyCode.Keypad0 + i))) {
                    AudioManager.instance.PlayClickSound(soundPath);
                    ToggleNumKeyboardEffect(i);
                    break;
                }
            }
        }
    }

    public void ToggleNumKeyboardEffect(int index, float delay = 0.1f) {
        StartCoroutine(ToggleActiveCoroutine(index, delay));
    }

    private IEnumerator ToggleActiveCoroutine(int index, float delay) {
        // Active the visual effect on the ATM machine
        numKeyboardEffects[index].SetActive(true);
        yield return new WaitForSeconds(delay);
        numKeyboardEffects[index].SetActive(false);
    }
}