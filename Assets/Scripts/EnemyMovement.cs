using UnityEngine;

public class EnemyMovement : MonoBehaviour
 {
    public float MovementSpeed;
    public float MaxRotationRate;

    private GameObject _player;

	void Start () 
	{
        _player = GameObject.FindGameObjectWithTag("Player").gameObject;
	}
	
	void Update () 
	{
        Vector3 dPos = _player.transform.position - transform.position;
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, 
            Quaternion.LookRotation(dPos, Vector3.up),
            Time.deltaTime * MaxRotationRate);
        float movementSpeed = Time.deltaTime * MovementSpeed;
        Vector3 targetPos = transform.position + movementSpeed * transform.forward;
        transform.position = new Vector3(targetPos.x, 0.0f, targetPos.z);
    }
}
