using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
 {
    public GameObject PlayerPrefab;
    public Color[] PlayerColors;

    public Vector2 PlayingFieldDimensions;

    public Sprite HighScoreBackgroundPanelImage;
    public Color HighScoreBackgroundPanelColor;
    public Color HighScoreUserBackgroundPanelColor;

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

    private static Transform _playersParent;
    private static GameObject _pausePanel;
    private static GameObject _pauseMenuResumeButton;
    private static GameObject _gameOverPanel;
    private static GameObject _gameOverMenuRestartButton;
    private static GameObject _highScoresViewPortContent;
    private static Text _timerText;
    private static Image _warningScreenOverlayImage;
    private static PowerManager _powerManager;

    private Slider _pauseVolumeSlider;
    private Slider _gameOverVolumeSlider;

    // Retrive from loading text
    private Vector3 _highScoreStartPos;
    private Font _highScoreFont;
    private int _highScoreFontSize;
    private Color _highScoreFontColor;
    private float _highScoreLineHeight;
    private float _highScoreLineWidth;
    private float _finalTime;

    private InputField _usernameInputField;
    private string _username = "";

    public float SecondsElapsed { get { return _secondsElapsed; } }
    private float _secondsElapsed;

    void Start() 
	{
        // Spawn players if not single player
        GameObject player = GameObject.Find("Player");
        if (!player)
        {
            _playersParent = GameObject.Find("Level").transform.Find("Players");

            float dTheta = (2.0f * Mathf.PI) / PlayerColors.Length;
            float theta = 0.0f;
            for (int i = 0; i < PlayerColors.Length; ++i)
            {
                GameObject newPlayer = Instantiate(PlayerPrefab);
                float radius = Mathf.Min(PlayingFieldDimensions.x, PlayingFieldDimensions.y) * 0.45f;
                Vector3 newPlayerPos = new Vector3(
                    Mathf.Cos(theta) * radius, 
                    newPlayer.transform.position.y, 
                    Mathf.Sin(theta) * radius);
                newPlayer.transform.position = newPlayerPos;
                newPlayer.transform.parent = _playersParent;
                theta += dTheta;
            }
        }

        _paused = false;
        _gameOver = false;

        _pausePanel = GameObject.Find("PausePanel");
        _gameOverPanel = GameObject.Find("GameOverPanel");

        _pauseMenuResumeButton = _pausePanel.transform.Find("ResumeButton").gameObject;
        _gameOverMenuRestartButton = _gameOverPanel.transform.Find("RestartButton").gameObject;

        _highScoresViewPortContent = GameObject.Find("HighScoresViewport").transform.Find("Content").gameObject;
        Text highScoreLoadingText = _highScoresViewPortContent.transform.Find("LoadingText").gameObject.GetComponent<Text>();
        _highScoreFont = highScoreLoadingText.font;
        _highScoreFontSize = highScoreLoadingText.fontSize;
        _highScoreFontColor = highScoreLoadingText.color;
        RectTransform rectTransform = highScoreLoadingText.GetComponent<RectTransform>();
        _highScoreStartPos = rectTransform.anchoredPosition;
        _highScoreLineWidth = rectTransform.sizeDelta.x;
        _highScoreLineHeight = rectTransform.sizeDelta.y;

        _usernameInputField = _highScoresViewPortContent.transform.Find("UserNameInputField").gameObject.GetComponent<InputField>();

        UpdateCanvases();

        if (!_pauseVolumeSlider)
        {
            _pauseVolumeSlider = _pausePanel.transform.Find("VolumeSlider").GetComponent<Slider>();
            _pauseVolumeSlider.value = AudioListener.volume;
        }

        if (!_gameOverVolumeSlider)
        {
            _gameOverVolumeSlider = _gameOverPanel.transform.Find("VolumeSlider").GetComponent<Slider>();
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
        if (_gameOver) return;

        _secondsElapsed += Time.deltaTime;

        if (_timerText)
        {
            _timerText.text = _secondsElapsed.ToString("#0.00") + "s";
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

        _finalTime = _secondsElapsed;

        // The user has set their username previously, don't ask for it again
        if (_username.Length > 0)
        {
            _usernameInputField.enabled = false;
            StartCoroutine(AddScoreToLeaderboard());
        }

        UpdateCanvases();
    }

    private IEnumerator AddScoreToLeaderboard()
    {
        int scoreScale = 100;

        if (_username.Length == 0) Debug.Log("Username hasn't been set!");

        string sentScoreStr = (_finalTime * scoreScale).ToString("#0.");
        IEnumerator result = AccessLeaderBoard.SendAndGet(_username + "/" + sentScoreStr);
        yield return result;

        string[] lines = AccessLeaderBoard.www.text.Split('\n');

        if (lines.Length > 0)
        {
            for (int i = 0; i < _highScoresViewPortContent.transform.childCount; ++i)
            {
                Destroy(_highScoresViewPortContent.transform.GetChild(i).gameObject);
            }
            _highScoresViewPortContent.transform.DetachChildren();

            int maxScores = 6;
            int scoresAdded = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length > 0)
                {
                    string[] fields = lines[i].Split('|');

                    string name = fields[0];
                    string scoreStr = fields[1];
                    float score = (float)int.Parse(scoreStr) / scoreScale;

                    bool highlighted = _username == name;
                    AddHighScoreText(name, i, false, highlighted);
                    AddHighScoreText(score.ToString("#.00") + "s", i, true);

                    ++scoresAdded;
                    if (scoresAdded >= maxScores)
                    {
                        break;
                    }
                }
            }
        }
        else
        {
            AddHighScoreText("Unable to access server");
        }
    }

    private GameObject AddHighScoreText(string text, int index = 0, bool right = false, bool highlighted = false)
    {
        GameObject highScore = new GameObject("highscore " + name);
        highScore.transform.parent = _highScoresViewPortContent.transform;
        highScore.layer = LayerMask.NameToLayer("UI");

        Text textComponent = highScore.AddComponent<Text>();
        textComponent.text = text;
        textComponent.font = _highScoreFont;
        textComponent.fontSize = _highScoreFontSize;
        textComponent.color = _highScoreFontColor;

        RectTransform textRectTransform = highScore.GetComponent<RectTransform>();
        textRectTransform.localScale = Vector3.one;
        Vector2 sizeDelta = new Vector2(_highScoreLineWidth, _highScoreLineHeight);
        textRectTransform.sizeDelta = sizeDelta;

        Vector3 textPos = _highScoreStartPos;
        textPos.y -= index * _highScoreLineHeight;
        if (right)
        {
            textComponent.alignment = TextAnchor.MiddleRight;
        }
        else
        {
            textComponent.alignment = TextAnchor.MiddleLeft;
        }
        textRectTransform.anchoredPosition = textPos;

        // Add panel behind every other line
        if (index % 2 == 0 || highlighted)
        {
            AddHighScorePanel(textPos, highlighted);
        }

        return highScore;
    }

    private void AddHighScorePanel(Vector3 pos, bool highlighted = false)
    {
        GameObject panel = new GameObject("highscore background panel " + name);
        panel.transform.SetParent(_highScoresViewPortContent.transform, false);
        panel.layer = LayerMask.NameToLayer("UI");

        Image image = panel.AddComponent<Image>();
        image.sprite = HighScoreBackgroundPanelImage;
        image.color = highlighted ? HighScoreUserBackgroundPanelColor : HighScoreBackgroundPanelColor;
        image.raycastTarget = false;
        image.type = Image.Type.Sliced;

        RectTransform panelRectTransform = panel.GetComponent<RectTransform>();
        panelRectTransform.anchoredPosition = pos;
        panelRectTransform.sizeDelta = new Vector2(0.0f, _highScoreLineHeight);
        panelRectTransform.anchorMin = new Vector2(0.0f, panelRectTransform.anchorMin.y);
        panelRectTransform.anchorMax = new Vector2(1.0f, panelRectTransform.anchorMax.y);
    }

    public void OnUserNameInputSubmit(BaseEventData eventData)
    {
        eventData.Use();
        string fieldText = _usernameInputField.text;

        OnUserNameInputSubmit(fieldText);
    }

    public void OnUserNameInputSubmit()
    {
        string fieldText = _usernameInputField.text;

        OnUserNameInputSubmit(fieldText);
    }

    private void OnUserNameInputSubmit(string username)
    {
        if (username.Length == 3 &&
            username.Contains("*") == false)
        {
            _username = _usernameInputField.text;

            _usernameInputField.enabled = false;
            StartCoroutine(AddScoreToLeaderboard());
        }
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
        _pausePanel.SetActive(GameOver ? false : _paused);
        _gameOverPanel.SetActive(GameOver);
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
