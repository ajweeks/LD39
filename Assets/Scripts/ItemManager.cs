using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Vector2 PlayingFieldDimensions;
    public GameObject Battery;

    public float SecondsBetweenItemSpawns;
    public float SecondsBeforeItemDespawn;
    private float _secondsSinceItemSpawn;

    private struct SpawnedItem
    {
        public GameObject Item;
        public float SecondsSinceSpawn;
        public float Lifetime;
    }

    private Transform _itemsParent;
    private int _spawnedItemCount;
    private SpawnedItem[] _spawnedItems;

	void Start ()
    {
        _itemsParent = GameObject.Find("Items").transform;
        _spawnedItems = new SpawnedItem[10];
    }

    Vector3 FindOpenSpotOnPlayingField()
    {
        Vector2 halfFieldDim = PlayingFieldDimensions / 2.0f;

        // TODO: Check if result is inside another object
        Vector3 result = new Vector3(
            Random.Range(-halfFieldDim.x, halfFieldDim.x),
            0.2f,
            Random.Range(-halfFieldDim.y, halfFieldDim.y));

        return result;
    }

    void Update()
    {
        _secondsSinceItemSpawn += Time.deltaTime;

        if (_secondsSinceItemSpawn > SecondsBetweenItemSpawns)
        {
            _secondsSinceItemSpawn = 0.0f;
            ++_spawnedItemCount;

            SpawnedItem newItem = new SpawnedItem();
            newItem.Lifetime = SecondsBeforeItemDespawn;
            newItem.SecondsSinceSpawn = 0.0f;
            newItem.Item = Instantiate(Battery);
            newItem.Item.transform.position = FindOpenSpotOnPlayingField();
            newItem.Item.transform.rotation = Quaternion.Euler(90, Random.Range(0, 180), 0);
            newItem.Item.transform.parent = _itemsParent;

            InsertSpawnedItemInFirstEmptySlot(ref newItem);
        }

        for (int i = 0; i < _spawnedItems.Length; ++i)
        {
            if (_spawnedItems[i].Item)
            {
                _spawnedItems[i].SecondsSinceSpawn += Time.deltaTime;
                if (_spawnedItems[i].SecondsSinceSpawn > _spawnedItems[i].Lifetime)
                {
                    RemoveSpawnedItem(i);
                }
            }
        }
    }

    void InsertSpawnedItemInFirstEmptySlot(ref SpawnedItem item)
    {
        for (int i = 0; i < _spawnedItems.Length; i++)
        {
            if (!_spawnedItems[i].Item)
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

    void RemoveSpawnedItem(int itemIndex)
    {
        Destroy(_spawnedItems[itemIndex].Item);
        _spawnedItems[itemIndex].Item = null;
        --_spawnedItemCount;
    }

    private void OnGUI()
    {
        GUILayout.Label("spawned item count: " + _spawnedItemCount);
        GUILayout.Label("spawned item array size: " + _spawnedItems.Length);
    }
}
