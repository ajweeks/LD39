using UnityEngine;
using UnityEngine.EventSystems;
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

    public AudioClip ExplosionSound;
    private AudioSource ExplosionSoundSource;

    public AudioClip PlayerDamageSound;
    private AudioSource PlayerDamageSoundSource;

    public AudioClip BackgroundMusic;
    private AudioSource BackgroundMusicSource;

    [HideInInspector]
    public static float EnemyDrainOnTouchAmount;

    [HideInInspector]
    public bool Paused { get { return _paused; } set { OnPause(value); } }
    public bool _paused = false;

    public bool GameOver {  get { return _gameOver; } }
    private bool _gameOver = false;

    private static GameObject _pauseMenu;
    private static GameObject _pauseMenuResumeButton;
    private static GameObject _gameOverMenu;
    private static GameObject _gameOverMenuRestartButton;
    private static Text _timerText;
    private static Image _warningScreenOverlayImage;
    private static PowerManager _powerManager;

    private Slider _pauseVolumeSlider;
    private Slider _gameOverVolumeSlider;

    public float SecondsElapsed { get { return _secondsElapsed; } }
    private float _secondsElapsed;

    void Start() 
	{
        _paused = false;
        _gameOver = false;

        _pauseMenu = GameObject.Find("PausePanel");
        _gameOverMenu = GameObject.Find("GameOverPanel");

        _pauseMenuResumeButton = _pauseMenu.transform.Find("ResumeButton").gameObject;
        _gameOverMenuRestartButton = _gameOverMenu.transform.Find("RestartButton").gameObject;

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

        _timerText = GameObject.Find("TimerText").GetComponent<Text>();

        ButtonClickSource = gameObject.AddComponent<AudioSource>();
        ButtonClickSource.clip = ButtonClickSound;

        BounceSoundSource = gameObject.AddComponent<AudioSource>();
        BounceSoundSource.clip = BounceSound;

        BatteryPickupSoundSource = gameObject.AddComponent<AudioSource>();
        BatteryPickupSoundSource.clip = BatteryPickupSound;

        BigBatteryPickupSoundSource = gameObject.AddComponent<AudioSource>();
        BigBatteryPickupSoundSource.clip = BigBatteryPickupSound;

        ExplosionSoundSource = gameObject.AddComponent<AudioSource>();
        ExplosionSoundSource.clip = ExplosionSound;

        GameOverSoundSource = gameObject.AddComponent<AudioSource>();
        GameOverSoundSource.clip = GameOverSound;

        PlayerDamageSoundSource = gameObject.AddComponent<AudioSource>();
        PlayerDamageSoundSource.clip = PlayerDamageSound;

        BackgroundMusicSource = gameObject.AddComponent<AudioSource>();
        BackgroundMusicSource.loop = true;
        BackgroundMusicSource.clip = BackgroundMusic;
        BackgroundMusicSource.Play();

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("GroundPlane"));

        _powerManager = GameObject.Find("Player").GetComponent<PowerManager>();
        _warningScreenOverlayImage = GameObject.Find("WarningScreenOverlayImage").GetComponent<Image>();
    }

    void Update() 
	{
        _secondsElapsed += Time.deltaTime;

        if (_timerText)
        {
            _timerText.text = _secondsElapsed.ToString("#.0") + "s";
        }


        if (Input.GetButtonUp("Cancel") && !_gameOver)
        {
            OnPause(!_paused);
            UpdateCanvases();
            OnButtonClick();
        }

        float minPowerLevel = 0.35f;
        if (_powerManager.PowerLevel < minPowerLevel && _powerManager.PowerLevel != 0.0f)
        {
            Color color =_warningScreenOverlayImage.color;
            float alpha = (1.0f - _powerManager.PowerLevel / minPowerLevel);
            color.a = alpha;
            _warningScreenOverlayImage.color = color;
        }
        else
        {
            if (_warningScreenOverlayImage.color.a != 0.0f)
            {
                Color color = _warningScreenOverlayImage.color;
                color.a = 0.0f;
                _warningScreenOverlayImage.color = color;
            }
        }
	}

    public void OnMainMenuButtonClick()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void OnGameOver()
    {
        GameOverSoundSource.Play();
        PlayerDamageSoundSource.Play();

        _gameOver = true;
        _paused = true;
        EventSystem.current.SetSelectedGameObject(_gameOverMenuRestartButton);

        UpdateCanvases();
    }

    public void OnPause(bool paused)
    {
        _paused = paused;

        if (paused)
        {
            BackgroundMusicSource.Pause();
            EventSystem.current.SetSelectedGameObject(_pauseMenuResumeButton);
            _pauseMenuResumeButton.SetActive(true);
        }
        else
        {
            BackgroundMusicSource.Play();
            EventSystem.current.SetSelectedGameObject(null);
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
        EventSystem.current.SetSelectedGameObject(null);

        _secondsElapsed = 0.0f;
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
        if (ButtonClickSource)
        {
            ButtonClickSource.Play();
        }
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

    public void OnExplosion()
    {
        ExplosionSoundSource.Play();
    }

    public void OnPlayerDamage()
    {
        PlayerDamageSoundSource.Play();
    }
}
