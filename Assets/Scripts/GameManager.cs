using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
 {
    public Vector2 PlayingFieldDimensions;

    public AudioClip BackgroundMusic;
    private AudioSource BackgroundMusicSource;

    public AudioClip ButtonClickSound;
    private AudioSource ButtonClickSource;

    [HideInInspector]
    public static float EnemyDrainOnTouchAmount;

    public bool Paused { get { return _paused; } set { OnPause(value); } }
    public bool _paused = false;

    public bool GameOver {  get { return _gameOver; } }
    private bool _gameOver = false;

    private static GameObject _pauseMenu;
    private static GameObject _gameOverMenu;

    private Slider _volumeSlider;

    void Start() 
	{
        _paused = false;
        _gameOver = false;

        _pauseMenu = GameObject.Find("PausePanel");
        _gameOverMenu = GameObject.Find("GameOverPanel");

        UpdateCanvases();

        _volumeSlider = _pauseMenu.transform.Find("VolumeSlider").GetComponent<Slider>();

        ButtonClickSource = gameObject.AddComponent<AudioSource>();
        ButtonClickSource.clip = ButtonClickSound;

        BackgroundMusicSource = gameObject.AddComponent<AudioSource>();
        BackgroundMusicSource.loop = true;
        BackgroundMusicSource.clip = BackgroundMusic;
        BackgroundMusicSource.Play();
    }

    void Update() 
	{
		if (Input.GetButtonUp("Cancel"))
        {
            OnPause(!_paused);
            UpdateCanvases();
        }
	}

    public void OnGameOver()
    {
        _gameOver = true;
        _paused = true;

        UpdateCanvases();
    }

    public void OnPause(bool paused)
    {
        _paused = paused;

        if (paused)
        {
            BackgroundMusicSource.Pause();
        }
        else
        {
            BackgroundMusicSource.Play();
        }

        UpdateCanvases();
    }

    private void UpdateCanvases()
    {
        _pauseMenu.SetActive(GameOver ? false : _paused);
        _gameOverMenu.SetActive(GameOver);
        Time.timeScale = _paused ? 0.0f : 1.0f;
    }

    public void OnRestart()
    {
        _gameOver = false;
        _paused = false;
        UpdateCanvases();

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnVolumeSliderChange()
    {
        AudioListener.volume = _volumeSlider.value;
    }

    public void OnButtonClick()
    {
        ButtonClickSource.Play();
    }
}
