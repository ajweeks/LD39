using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
 {
    public Image PowerBarImage;

    public Gradient PowerBarColor;

    private float _powerBarStartWidth;
    private PowerManager _playerPowerManager;

    private void Start()
    {
        _playerPowerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PowerManager>();
        _powerBarStartWidth = PowerBarImage.rectTransform.sizeDelta.x;
    }

    void Update () 
	{
        RectTransform powerBarRT = PowerBarImage.rectTransform;
        powerBarRT.sizeDelta = new Vector2(_playerPowerManager.PowerLevel * _powerBarStartWidth, powerBarRT.sizeDelta.y);
        PowerBarImage.color = PowerBarColor.Evaluate(_playerPowerManager.PowerLevel);
    }
}
