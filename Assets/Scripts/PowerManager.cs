using UnityEngine;

public class PowerManager : MonoBehaviour
{
    public float PowerLevelDrainPerSecond;
    public float PowerLevel { get { return _powerLevel; } }
    private float _powerLevel; // [0.0f, 1.0f]

    private GameManager _gameManager;

	void Start ()
    {
        _powerLevel = 1.0f;

        _gameManager = GameObject.Find("Managers").GetComponent<GameManager>();

    }
	
	void Update ()
    {
        _powerLevel -= PowerLevelDrainPerSecond * Time.deltaTime;
        _powerLevel = Mathf.Clamp01(_powerLevel);

        if (_powerLevel <= 0.0f)
        {
            _gameManager.OnGameOver();
        }
	}

    public void OnBatteryPickup(Battery battery)
    {
        _powerLevel += battery.PowerLevel;
        _powerLevel = Mathf.Clamp01(_powerLevel);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GetComponent<Collider>().CompareTag("Enemy"))
        {
            _powerLevel -= GameManager.EnemyDrainOnTouchAmount;
            _powerLevel = Mathf.Clamp01(_powerLevel);
        }
    }

    //private void OnGUI()
    //{
    //  GUILayout.Label("\n\n\nPower level: " + _powerLevel);
    //}
}
