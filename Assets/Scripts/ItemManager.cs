﻿using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject Battery;
    public GameObject BigBattery;

    [Range(0.0f, 1.0f)]
    public float BigBatteryChance;

    public float SecondsBetweenBatterySpawns;
    public float SecondsBeforeBatteryDespawn;
    private float _secondsSinceBatterySpawn;

    private struct SpawnedItem
    {
        public GameObject Item;
        public float SecondsSinceSpawn;
        public float Lifetime;
    }

    private Transform _itemsParent;
    private int _spawnedItemCount;
    private SpawnedItem[] _spawnedItems;

    private GameManager _gameManager;

	void Start ()
    {
        _gameManager = GameObject.Find("Managers").GetComponent<GameManager>();
        _itemsParent = GameObject.Find("Items").transform;
        _spawnedItems = new SpawnedItem[10];

        // Spawn the first battery sooner
        _secondsSinceBatterySpawn = SecondsBetweenBatterySpawns * 0.75f;
    }

    void Update()
    {
        _secondsSinceBatterySpawn += Time.deltaTime;

        if (_secondsSinceBatterySpawn > SecondsBetweenBatterySpawns)
        {
            _secondsSinceBatterySpawn = 0.0f;

            SpawnedItem newItem = new SpawnedItem();
            newItem.Lifetime = SecondsBeforeBatteryDespawn;
            newItem.SecondsSinceSpawn = 0.0f;

            bool bigBattery = BigBatteryChance <= Random.Range(0.0f, 1.0f);
            if (bigBattery)
            {
                newItem.Item = Instantiate(BigBattery);
            }
            else
            {
                newItem.Item = Instantiate(Battery);
            }
            newItem.Item.transform.position = FindOpenSpotOnPlayingField();
            newItem.Item.transform.rotation = Quaternion.Euler(0, Random.Range(0, 180), 0);
            newItem.Item.transform.parent = _itemsParent;

            InsertSpawnedItemInFirstEmptySlot(ref newItem);
        }

        for (int i = 0; i < _spawnedItems.Length; ++i)
        {
            if (_spawnedItems[i].Item != null)
            {
                _spawnedItems[i].SecondsSinceSpawn += Time.deltaTime;
                if (_spawnedItems[i].SecondsSinceSpawn > _spawnedItems[i].Lifetime)
                {
                    RemoveSpawnedItem(i);
                }
                // Start blinking when about to disappear
                else if (_spawnedItems[i].SecondsSinceSpawn > _spawnedItems[i].Lifetime * 0.7f)
                {
                    float lifeRemaining = _spawnedItems[i].Lifetime - _spawnedItems[i].SecondsSinceSpawn;
                    float blinkRate = _spawnedItems[i].Lifetime / 24.0f;
                    bool hidden = Mathf.Repeat(lifeRemaining, blinkRate) < blinkRate / 2.0f;
                    _spawnedItems[i].Item.GetComponent<MeshRenderer>().enabled = !hidden;
                    var lights = _spawnedItems[i].Item.GetComponentsInChildren<Light>();
                    for (int l = 0; l < lights.Length; ++l)
                    {
                        lights[l].enabled = !hidden;
                    }
                }
            }
        }
    }

    public GameObject[] GetSpawnedItems()
    {
        if (_spawnedItemCount == 0) return null;

        GameObject[] spawnedItems = new GameObject[_spawnedItemCount];

        int itemCount = 0;
        for (int i = 0; i < _spawnedItems.Length; i++)
        {
            if (_spawnedItems[i].Item != null)
            {
                spawnedItems[itemCount++] = _spawnedItems[i].Item;
            }
        }

        Debug.Assert(_spawnedItemCount == itemCount);
        return spawnedItems;
    }

    private Vector3 FindOpenSpotOnPlayingField()
    {
        Vector2 halfFieldDim = _gameManager.PlayingFieldDimensions / 2.0f;

        // TODO: Check if result is inside another object
        Vector3 result = new Vector3(
            Random.Range(-halfFieldDim.x, halfFieldDim.x),
            0.2f,
            Random.Range(-halfFieldDim.y, halfFieldDim.y));

        return result;
    }

    private void InsertSpawnedItemInFirstEmptySlot(ref SpawnedItem item)
    {
        ++_spawnedItemCount;

        for (int i = 0; i < _spawnedItems.Length; i++)
        {
            if (_spawnedItems[i].Item == null)
            {
                _spawnedItems[i] = item;
                return;
            }
        }

        if (_spawnedItemCount > _spawnedItems.Length)
        {
            System.Array.Resize(ref _spawnedItems, Mathf.Max(_spawnedItems.Length, 1) * 2);
        }

        _spawnedItems[_spawnedItemCount - 1] = item;
    }

    private void RemoveSpawnedItem(int itemIndex)
    {
        Destroy(_spawnedItems[itemIndex].Item);
        _spawnedItems[itemIndex].Item = null;
        --_spawnedItemCount;
    }

    public void OnBatteryRemoved(GameObject battery)
    {
        for (int i = 0; i < _spawnedItems.Length; i++)
        {
            if (_spawnedItems[i].Item == battery)
            {
                RemoveSpawnedItem(i);
                return;
            }
        }

        Debug.Assert(false, "Attempt to remove non-existent battery!");
    }

    //private void OnGUI()
    //{
    //    GUILayout.Label("spawned item count: " + _spawnedItemCount);
    //    GUILayout.Label("spawned item array size: " + _spawnedItems.Length);
    //}
}
