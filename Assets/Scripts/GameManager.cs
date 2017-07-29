using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
 {
    [HideInInspector]
    public static bool Paused = false;

    public static bool GameOver {  get { return _gameOver; } }
    private static bool _gameOver = false;

    private static GameObject _pauseMenu;
    private static GameObject _gameOverMenu;

    void Start() 
	{
        Paused = false;
        _gameOver = false;

        _pauseMenu = GameObject.Find("PausePanel");
        _gameOverMenu = GameObject.Find("GameOverPanel");

        UpdateCanvases();
    }

    void Update() 
	{
		if (Input.GetButtonUp("Cancel"))
        {
            Paused = !Paused;
            UpdateCanvases();
        }
	}

    public static void OnGameOver()
    {
        _gameOver = true;
        Paused = true;

        UpdateCanvases();
    }

    private static void UpdateCanvases()
    {
        _pauseMenu.SetActive(Paused);
        _gameOverMenu.SetActive(GameOver);
        Time.timeScale = Paused ? 0.0f : 1.0f;
    }

    public void OnRestart()
    {
        _gameOver = false;
        Paused = false;
        UpdateCanvases();

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
