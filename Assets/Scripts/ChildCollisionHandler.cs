using UnityEngine;

public class ChildCollisionHandler : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float ScreenShakeScale; // How much less to shake the screen than the main player body

    private Rigidbody _rb;
    private PowerManager _powerManager;
    private PlayerMovement _playerMovement;
    private CameraController _cameraController;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _powerManager = player.GetComponent<PowerManager>();
        _playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Battery"))
        {
            Battery battery = collision.gameObject.GetComponent<Battery>();
            _powerManager.OnBatteryPickup(battery);
        }
        else if (collision.gameObject.CompareTag("Level"))
        {
            float xIntensity = _rb.velocity.x * _playerMovement.VelocityShakeScale * ScreenShakeScale;
            float yIntensity = _rb.velocity.z * _playerMovement.VelocityShakeScale * ScreenShakeScale;
            float duration = _rb.velocity.magnitude * _playerMovement.VelocityShakeDurationScale * ScreenShakeScale;
            Debug.Log("Child shake: " + xIntensity.ToString().Substring(0, 3) + ", " + 
                yIntensity.ToString().Substring(0, 3) + " for " + duration.ToString().Substring(0, 3));
            _cameraController.Shake(xIntensity, yIntensity, duration);
        }
    }
}
