using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
 {
    public AudioClip ButtonClickSound;
    private AudioSource ButtonClickSource;

    private GameObject _buttonsPanel;
    private GameObject _playButton;
    private GameObject _aboutPanel;
    private GameObject _aboutPanelBackButton;

    private void Start()
    {
        _buttonsPanel = GameObject.Find("Buttons");
        _buttonsPanel.SetActive(true);

        _playButton = _buttonsPanel.transform.Find("PlayButton").gameObject;

        _aboutPanel = GameObject.Find("About");
        _aboutPanel.SetActive(false);

        _aboutPanelBackButton = _aboutPanel.transform.Find("AboutBackButton").gameObject;

        ButtonClickSource = gameObject.AddComponent<AudioSource>();
        ButtonClickSource.clip = ButtonClickSound;
    }

    public void OnPlayButtonClick()
    {
        SceneManager.LoadSceneAsync(1);

        ButtonClickSource.Play();
    }

    public void OnAboutButtonClick()
    {
        _buttonsPanel.SetActive(false);
        _aboutPanel.SetActive(true);
        ButtonClickSource.Play();

        EventSystem.current.SetSelectedGameObject(_aboutPanelBackButton);
    }

    public void OnBackButtonClick()
    {
        _buttonsPanel.SetActive(true);
        _aboutPanel.SetActive(false);
        ButtonClickSource.Play();

        EventSystem.current.SetSelectedGameObject(_playButton);
    }

    public void OnTwitterHandleClick()
    {
        ButtonClickSource.Play();
        Application.OpenURL("https://twitter.com/liqwidice");
    }
}
