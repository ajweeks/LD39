using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public AudioClip ButtonClickSound;
    private AudioSource ButtonClickSource;

    public AudioClip BackgroundMusic;
    private AudioSource BackgroundMusicSource;

    private GameObject _buttonsPanel;
    private GameObject _singlePlayerButton;
    //private GameObject _multiPlayerButton;
    private GameObject _aboutPanel;
    private GameObject _aboutPanelBackButton;
    private Text _multiPlayerComingSoonText;

    private void Start()
    {
        _buttonsPanel = GameObject.Find("Buttons");
        _buttonsPanel.SetActive(true);

        _singlePlayerButton = _buttonsPanel.transform.Find("SinglePlayerButton").gameObject;
        //_multiPlayerButton = _buttonsPanel.transform.Find("MultiPlayerButton").gameObject;
        _multiPlayerComingSoonText = _buttonsPanel.transform.Find("ComingSoonText").gameObject.GetComponent<Text>();

        _aboutPanel = GameObject.Find("About");
        _aboutPanel.SetActive(false);

        _aboutPanelBackButton = _aboutPanel.transform.Find("AboutBackButton").gameObject;

        ButtonClickSource = gameObject.AddComponent<AudioSource>();
        ButtonClickSource.clip = ButtonClickSound;

        BackgroundMusicSource = gameObject.AddComponent<AudioSource>();
        BackgroundMusicSource.clip = BackgroundMusic;
        BackgroundMusicSource.loop = true;
        BackgroundMusicSource.Play();
    }

    public void OnPlayButtonClick()
    {
        SceneManager.LoadSceneAsync(1);

        BackgroundMusicSource.Stop();
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

        EventSystem.current.SetSelectedGameObject(_singlePlayerButton);
    }

    public void OnMultiplayerButtonSelect()
    {
        _multiPlayerComingSoonText.enabled = true;
    }

    public void OnMultiplayerButtonDeselect()
    {
        _multiPlayerComingSoonText.enabled = false;
    }
}
