using UnityEngine;

public class PowerManager : MonoBehaviour
{
    public float PowerLevelDrainPerSecond;

    [HideInInspector]
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

        if (!_gameManager.GameOver && _powerLevel <= 0.0f)
        {
            _gameManager.OnGameOver();
        }
	}

    public void OnBombExplosion(Bomb bomb)
    {
        float dist = Vector3.Distance(bomb.transform.position, transform.position);
        float distScale = 1.0f - (dist / bomb.ExplosionRadius); // The closer we are the more we get drained
        distScale = Mathf.Clamp01(distScale);
        _powerLevel -= bomb.PlayerPowerDrain * distScale;
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
