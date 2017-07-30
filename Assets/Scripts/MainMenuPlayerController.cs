using UnityEngine;

public class MainMenuPlayerController : MonoBehaviour
 {
    public float RotationSpeed;
    public bool FlipRotation;

    private Rigidbody _rb;

	void Start () 
	{
        _rb = GetComponent<Rigidbody>();
	}
	
	void Update () 
	{
        _rb.AddTorque(new Vector3(
            0.0f,
            Random.Range(0.0f, RotationSpeed * Time.deltaTime) * (FlipRotation ? -1 : 1),
            0.0f));
	}
}
