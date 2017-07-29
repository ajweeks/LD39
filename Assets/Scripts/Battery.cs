using UnityEngine;

public class Battery : MonoBehaviour
{
    public float RotationSpeed;
    public float PowerLevel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Child"))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.back, Time.deltaTime * RotationSpeed);
    }
}
