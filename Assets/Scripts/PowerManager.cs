using UnityEngine;

public class PowerManager : MonoBehaviour
{
    public float PowerLevelDrainPerSecond;
    public float PowerLevel { get { return _powerLevel; } }
    private float _powerLevel; // [0.0f, 1.0f]

	void Start ()
    {
        _powerLevel = 1.0f;
	}
	
	void Update ()
    {
        _powerLevel -= PowerLevelDrainPerSecond * Time.deltaTime;
        _powerLevel = Mathf.Clamp01(_powerLevel);
	}

    public void OnBatteryPickup(Battery battery)
    {
        _powerLevel += battery.PowerLevel;
    }

    private void OnGUI()
    {
       // GUILayout.Label("\n\n\nPower level: " + _powerLevel);
    }
}
