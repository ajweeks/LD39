using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
 {
    public static bool Paused { get { return _paused; } set { OnPause(value); } }
    public static bool _paused = false;

    public static bool GameOver {  get { return _gameOver; } }
    private static bool _gameOver = false;

    private static GameObject _pauseMenu;
    private static GameObject _gameOverMenu;

    void Start() 
	{
        _paused = false;
        _gameOver = false;

        _pauseMenu = GameObject.Find("PausePanel");
        _gameOverMenu = GameObject.Find("GameOverPanel");

        UpdateCanvases();
    }

    void Update() 
	{
		if (Input.GetButtonUp("Cancel"))
        {
            _paused = !_paused;
            UpdateCanvases();
        }
	}

    public static void OnGameOver()
    {
        _gameOver = true;
        _paused = true;

        UpdateCanvases();
    }

    public static void OnPause(bool paused)
    {
        _paused = paused;

        UpdateCanvases();
    }

    private static void UpdateCanvases()
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
}
