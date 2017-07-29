using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MovementSpeed;

    [Range(0.0f, 0.1f)]
    public float VelocityShakeScale;

    [Range(0.0f, 0.5f)]
    public float VelocityShakeDurationScale;

    private Rigidbody _rb;

    private CameraController _cameraController;

	void Start ()
    {
        _rb = GetComponent<Rigidbody>();
        _cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }
	
	void Update ()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0.0f)
        {
            _rb.AddForce(new Vector3(MovementSpeed * horizontalInput * Time.deltaTime, 0, 0), ForceMode.Impulse);
        }

        if (verticalInput != 0.0f)
        {
            _rb.AddForce(new Vector3(0, 0, MovementSpeed * verticalInput * Time.deltaTime), ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Level"))
        {
            float xIntensity = _rb.velocity.x * VelocityShakeScale;
            float yIntensity = _rb.velocity.z * VelocityShakeScale;
            float duration = _rb.velocity.magnitude * VelocityShakeDurationScale;
            Debug.Log("Shake: " + xIntensity.ToString().Substring(0, 3) + ", " + 
                yIntensity.ToString().Substring(0, 3) + " for " + 
                duration.ToString().Substring(0, 3));
            _cameraController.Shake(xIntensity, yIntensity, duration);
        }
    }
}
