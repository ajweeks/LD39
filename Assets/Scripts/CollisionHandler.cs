using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private PowerManager _powerManager;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _powerManager = player.GetComponent<PowerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
            Battery battery = other.GetComponent<Battery>();
            _powerManager.OnBatteryPickup(battery);
        }
    }
}
