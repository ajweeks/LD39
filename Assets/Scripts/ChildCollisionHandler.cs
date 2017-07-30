using UnityEngine;

public class ChildCollisionHandler : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float ScreenShakeScale; // How much less to shake the screen than the main player body

    private Rigidbody _rb;
    private PowerManager _powerManager;
    private PlayerMovement _playerMovement;
    private CameraController _cameraController;
    private GameManager _gameManager;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.WakeUp();

        _cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

        _gameManager = GameObject.Find("Managers").GetComponent<GameManager>();

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
            if (collision.collider.gameObject.layer != LayerMask.NameToLayer("GroundPlane"))
            {
                float xIntensity = _rb.velocity.x * _playerMovement.VelocityShakeScale * ScreenShakeScale;
                float yIntensity = _rb.velocity.z * _playerMovement.VelocityShakeScale * ScreenShakeScale;
                float duration = _rb.velocity.magnitude * _playerMovement.VelocityShakeDurationScale * ScreenShakeScale;
                _cameraController.Shake(xIntensity, yIntensity, duration);

                _gameManager.OnPlayerBounce();
            }
        }
    }
}
