﻿using System.Collections;
using UnityEngine;


public class BombManager : MonoBehaviour
 {
    public GameObject Bomb;

    public float SecondsOfInitialDelay;
    public float SecondsBetweenBombs;

    public float SpawnHeight;

    private float _secondsSinceBombSpawn;

    private int _bombCount;
    private Bomb[] _bombs;

    private Transform _bombParent;
    private GameManager _gameManager;

	void Start () 
	{
        _gameManager = GameObject.Find("Managers").GetComponent<GameManager>();
        _bombParent = GameObject.Find("Bombs").transform;
    }
	
	void Update () 
	{
		if (_gameManager.SecondsElapsed > SecondsOfInitialDelay)
        {
            _secondsSinceBombSpawn += Time.deltaTime;
            if (_secondsSinceBombSpawn > SecondsBetweenBombs)
            {
                _secondsSinceBombSpawn = 0.0f;

                GameObject newBomb = Instantiate(Bomb);
                Vector2 halfFieldDim = _gameManager.PlayingFieldDimensions / 2.0f;
                newBomb.transform.position = new Vector3(
                    Random.Range(-halfFieldDim.x, halfFieldDim.x),
                    SpawnHeight,
                    Random.Range(-halfFieldDim.y, halfFieldDim.y));
                newBomb.transform.parent = _bombParent;

                // Add random torque
                float randomTorqueScale = 500.0f;
                Rigidbody bombRB = newBomb.GetComponent<Rigidbody>();
                bombRB.AddTorque(new Vector3(
                    Random.Range(-randomTorqueScale, randomTorqueScale),
                    Random.Range(-randomTorqueScale, randomTorqueScale),
                    Random.Range(-randomTorqueScale, randomTorqueScale)));

                Bomb bombComponent = newBomb.GetComponent<Bomb>();
                bombComponent.LightFuse();
            }
        }
	}

    public void OnBombExplosion(Bomb bomb)
    {
        bomb.GetComponent<MeshRenderer>().enabled = false;
        DestroyBombAfterSeconds(1.0f, bomb);
    }

    private IEnumerator DestroyBombAfterSeconds(float seconds, Bomb bomb)
    {
        yield return new WaitForSeconds(seconds);

        Destroy(bomb.gameObject);
    }

    private void InsertSpawnedItemInFirstEmptySlot(ref Bomb bomb)
    {
        ++_bombCount;

        for (int i = 0; i < _bombs.Length; i++)
        {
            if (_bombs[i] == null)
            {
                _bombs[i] = bomb;
                return;
            }
        }

        if (_bombCount > _bombs.Length)
        {
            System.Array.Resize(ref _bombs, Mathf.Max(_bombs.Length, 1) * 2);
        }

        _bombs[_bombCount - 1] = bomb;
    }

    private void RemoveSpawnedItem(int bombIndex)
    {
        Destroy(_bombs[bombIndex]);
        _bombs[bombIndex] = null;
        --_bombCount;
    }
}
