using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
 {
    public AudioClip ButtonClickSound;
    private AudioSource ButtonClickSource;

    public void OnPlayButtonClick()
    {
        SceneManager.LoadSceneAsync(1);

        ButtonClickSource = gameObject.AddComponent<AudioSource>();
        ButtonClickSource.clip = ButtonClickSound;
        ButtonClickSource.Play();
    }
}
