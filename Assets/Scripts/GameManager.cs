using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
 {
    public Vector2 PlayingFieldDimensions;

    public AudioClip BounceSound;
    private AudioSource BounceSoundSource;

    public AudioClip ButtonClickSound;
    private AudioSource ButtonClickSource;

    public AudioClip GameOverSound;
    private AudioSource GameOverSoundSource;

    public AudioClip BatteryPickupSound;
    private AudioSource BatteryPickupSoundSource;

    public AudioClip BigBatteryPickupSound;
    private AudioSource BigBatteryPickupSoundSource;

    public AudioClip BackgroundMusic;
    private AudioSource BackgroundMusicSource;

    [HideInInspector]
    public static float EnemyDrainOnTouchAmount;

    public bool Paused { get { return _paused; } set { OnPause(value); } }
    public bool _paused = false;

    public bool GameOver {  get { return _gameOver; } }
    private bool _gameOver = false;

    private static GameObject _pauseMenu;
    private static GameObject _gameOverMenu;

    private Slider _pauseVolumeSlider;
    private Slider _gameOverVolumeSlider;

    void Start() 
	{
        _paused = false;
        _gameOver = false;

        _pauseMenu = GameObject.Find("PausePanel");
        _gameOverMenu = GameObject.Find("GameOverPanel");

        UpdateCanvases();

        if (!_pauseVolumeSlider)
        {
            _pauseVolumeSlider = _pauseMenu.transform.Find("VolumeSlider").GetComponent<Slider>();
            _pauseVolumeSlider.value = AudioListener.volume;
        }

        if (!_gameOverVolumeSlider)
        {
            _gameOverVolumeSlider = _gameOverMenu.transform.Find("VolumeSlider").GetComponent<Slider>();
            _gameOverVolumeSlider.value = AudioListener.volume;
        }

        ButtonClickSource = gameObject.AddComponent<AudioSource>();
        ButtonClickSource.clip = ButtonClickSound;

        BounceSoundSource = gameObject.AddComponent<AudioSource>();
        BounceSoundSource.clip = BounceSound;

        BatteryPickupSoundSource = gameObject.AddComponent<AudioSource>();
        BatteryPickupSoundSource.clip = BatteryPickupSound;

        BigBatteryPickupSoundSource = gameObject.AddComponent<AudioSource>();
        BigBatteryPickupSoundSource.clip = BigBatteryPickupSound;

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
        GameOverSoundSource.Play();

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

        GameOverSoundSource.Stop();

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnPauseVolumeSliderChange()
    {
        if (_pauseVolumeSlider && _gameOverVolumeSlider)
        {
            AudioListener.volume = _pauseVolumeSlider.value;
            _gameOverVolumeSlider.value = _pauseVolumeSlider.value;
        }
    }

    public void OnGameOverVolumeSliderChange()
    {
        if (_pauseVolumeSlider && _gameOverVolumeSlider)
        {
            AudioListener.volume = _gameOverVolumeSlider.value;
            _pauseVolumeSlider.value = _gameOverVolumeSlider.value;
        }
    }

    public void OnButtonClick()
    {
        ButtonClickSource.Play();
    }

    public void OnPlayerBounce()
    {
        BounceSoundSource.Play();
    }

    public void OnBatteryPickup()
    {
        BatteryPickupSoundSource.Play();
    }

    public void OnBigBatteryPickup()
    {
        BigBatteryPickupSoundSource.Play();
    }
}
