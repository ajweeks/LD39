using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MovementSpeed;

    private Rigidbody _rb;

	void Start ()
    {
        _rb = GetComponent<Rigidbody>();
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
}
