using UnityEngine;

public class Battery : MonoBehaviour
{
    public float RotationSpeed;
    public float PowerLevel;

    private ItemManager _itemManager;

    private void Start()
    {
        _itemManager = GameObject.Find("Item Manager").GetComponent<ItemManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Child"))
        {
            _itemManager.OnBatteryRemoved(gameObject);
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.back, Time.deltaTime * RotationSpeed);
    }
}
