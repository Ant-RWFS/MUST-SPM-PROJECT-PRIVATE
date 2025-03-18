using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct SpawnedItem
{
    public GameObject prefab;
    public int maxStage;
    [HideInInspector] public int x;
    [HideInInspector] public int y;
    [HideInInspector] public float offsetX;
    [HideInInspector] public float offsetY;
}
public class ItemGenerator : MonoBehaviour
{
    public List<SpawnedItem> items;
    public Dictionary<(int, int), SpawnedItem> spawnedItems;

    private void Awake()
    {
        spawnedItems = new Dictionary<(int, int), SpawnedItem>();
    }

    private void Start()
    {
      
    }

    private void Update()
    {

    }
    public void CleanSpanwedItems() => spawnedItems.Clear();
    public void UpdateSpawnedItems(int _index, int _x, int _y, float _offsetX, float _offsetY) => spawnedItems.Add((_x, _y), new SpawnedItem { prefab = items[_index].prefab, x = _x, y = _y, offsetX = _offsetX, offsetY = _offsetY });
    public GameObject InstantiateSpanwedItems(int _x, int _y, float _offsetX, float _offsetY) 
    {
        if (spawnedItems.ContainsKey((_x, _y)))
            return Instantiate(spawnedItems[(_x, _y)].prefab, new Vector3(_x + _offsetX, _y + _offsetY), Quaternion.identity, ItemManager.instance.itemTransform);
        else
            return null;
    }
}
