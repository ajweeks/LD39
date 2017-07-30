using UnityEngine;

public class MainMenuEnemyMovement : MonoBehaviour
 {
    public float MoveSpeed;
    public float MinDistToWaypoint;
    public Transform[] WayPoints;

    private int _currentIndex;
    private float _startY;

	void Start() 
	{
        _currentIndex = 0;
        _startY = transform.position.y;
	}
	
	void Update() 
	{
        float dist = Vector3.Distance(WayPoints[_currentIndex].position, transform.position);
        if (dist <= MinDistToWaypoint)
        {
            ++_currentIndex;
            _currentIndex %= WayPoints.Length;
        }

        Vector3 target = Vector3.MoveTowards(
            transform.position, 
            WayPoints[_currentIndex].position, 
            MoveSpeed * Time.deltaTime);
        target.y = _startY;
        transform.position = target;

        Vector3 forward = WayPoints[_currentIndex].position - transform.position;
        transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
	}
}
