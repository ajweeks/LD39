using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
 {
    public Image PowerBarImage;

    public Gradient PowerBarColor;

    private PowerManager _playerPowerManager;

    private void Start()
    {
        _playerPowerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PowerManager>();
    }

    void Update () 
	{
        PowerBarImage.fillAmount = _playerPowerManager.PowerLevel;
        PowerBarImage.color = PowerBarColor.Evaluate(_playerPowerManager.PowerLevel);
    }
}
