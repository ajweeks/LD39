using UnityEngine;

public class ItemAttractor : MonoBehaviour
 {
    public float Range;
    public float DistanceStrengthScale;
    public float Strength;

    private ItemManager _itemManager;

    private void Start()
    {
        _itemManager = GameObject.Find("Item Manager").GetComponent<ItemManager>();
        Debug.Assert(_itemManager, "Couldn't find Item Manager object!");
    }

    private void Update()
    {
        GameObject[] items = _itemManager.GetSpawnedItems();
        if (items != null)
        {
            for (int i = 0; i < items.Length; ++i)
            {
                float dist = Vector3.Distance(transform.position, items[i].transform.position);
                if (dist <= Range)
                {
                    items[i].transform.position = Vector3.MoveTowards(
                        items[i].transform.position, 
                        transform.position, 
                        Time.deltaTime * Strength * ((Range / dist) * DistanceStrengthScale));
                }
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(1, 0, 0, 0.3f);
    //    Gizmos.DrawSphere(transform.position, Range);
    //}

}
