using UnityEngine;

public class EnemyManager : MonoBehaviour
 {
    public float SecondsBetweenSpawns;
    public AnimationCurve SecondsBetweenSpawnsCurve;
    public float RoundLength;

    public int MaxEnemyCount;

    public GameObject Enemy;

    private PlayerMovement _player;
    private Transform _enemyParent;
    private float _secondsSinceLastSpawn;

    private ItemManager _itemManager;

    private int _spawnedEnemyCount;
    private GameObject[] _spawnedEnemies;

	void Start () 
	{
        _spawnedEnemies = new GameObject[10];

        _enemyParent = GameObject.Find("Enemies").transform;
        _player = GameObject.Find("Player").GetComponent<PlayerMovement>();

        _itemManager = GameObject.Find("Item Manager").GetComponent<ItemManager>();

        // Give player an easy start
        _secondsSinceLastSpawn = SecondsBetweenSpawns * 3;

    }
	
	void Update () 
	{
        _secondsSinceLastSpawn += Time.deltaTime;

        //SecondsBetweenSpawnsCurve.Evaluate();

        if (_secondsSinceLastSpawn >= SecondsBetweenSpawns && _spawnedEnemyCount < MaxEnemyCount)
        {
            _secondsSinceLastSpawn = 0.0f;

            GameObject newEnemy = Instantiate(Enemy);
            Vector2 halfFieldDim = _itemManager.PlayingFieldDimensions / 2.0f;
            newEnemy.transform.position = new Vector3(
                Random.Range(-halfFieldDim.x, halfFieldDim.x), 
                0.0f,
                Random.Range(-halfFieldDim.y, halfFieldDim.y));
            newEnemy.transform.parent = _enemyParent;

            Vector3 dPos = _player.transform.position - newEnemy.transform.position;
            newEnemy.transform.rotation = Quaternion.LookRotation(dPos, Vector3.up);

            InsertSpawnedItemInFirstEmptySlot(ref newEnemy);
        }
    }

    private void InsertSpawnedItemInFirstEmptySlot(ref GameObject enemy)
    {
        ++_spawnedEnemyCount;

        for (int i = 0; i < _spawnedEnemies.Length; i++)
        {
            if (_spawnedEnemies[i] == null)
            {
                _spawnedEnemies[i] = enemy;
                return;
            }
        }

        if (_spawnedEnemyCount > _spawnedEnemies.Length)
        {
            System.Array.Resize(ref _spawnedEnemies, Mathf.Max(_spawnedEnemies.Length, 1) * 2);
        }

        _spawnedEnemies[_spawnedEnemyCount - 1] = enemy;
    }

}
