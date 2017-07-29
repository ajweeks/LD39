using UnityEngine;

public class Battery : MonoBehaviour
{
    public float RotationSpeed;
    public float PowerLevel;
    public bool Big;

    private ItemManager _itemManager;
    private GameManager _gameManager;

    private void Start()
    {
        _itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
        _gameManager = GameObject.Find("Managers").GetComponent<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Child"))
        {
            _itemManager.OnBatteryRemoved(gameObject);

            if (Big)
            {
                _gameManager.OnBigBatteryPickup();
            }
            else
            {
                _gameManager.OnBatteryPickup();
            }
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.back, Time.deltaTime * RotationSpeed);
    }
}
