using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct SpawnedItem
{
    public GameObject prefab;
    [HideInInspector] public int x;
    [HideInInspector] public int y;
    [HideInInspector] public float offsetX;
    [HideInInspector] public float offsetY;
}
[System.Serializable]
public struct SpawnedResource
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
    public List<SpawnedResource> resources;
    public Dictionary<(int, int), SpawnedItem> spawnedItems;
    public Dictionary<(int, int), SpawnedResource> spawnedResources;

    private void Awake()
    {
        spawnedItems = new Dictionary<(int, int), SpawnedItem>();
        spawnedResources = new Dictionary<(int, int), SpawnedResource>();
    }

    private void Start()
    {
      
    }

    private void Update()
    {

    }
    public void CleanSpanwedObjects() 
    {
        spawnedResources.Clear();
        spawnedItems.Clear();
    }

    #region Items
    public void UpdateSpawnedItems(int _index, int _x, int _y, float _offsetX, float _offsetY) => spawnedItems.Add((_x, _y), new SpawnedItem { prefab = items[_index].prefab, x = _x, y = _y, offsetX = _offsetX, offsetY = _offsetY });
    public GameObject InstantiateSpanwedItems(int _x, int _y, float _offsetX, float _offsetY)
    {
        if (spawnedItems.ContainsKey((_x, _y)))
            return Instantiate(spawnedItems[(_x, _y)].prefab, new Vector3(_x + _offsetX, _y + _offsetY), Quaternion.identity, ItemManager.instance.itemTransform);
        else
            return null;
    }
    #endregion

    #region Resources
    public void UpdateSpawnedResources(int _index, int _x, int _y, float _offsetX, float _offsetY) => spawnedResources.Add((_x, _y), new SpawnedResource { prefab = resources[_index].prefab, x = _x, y = _y, offsetX = _offsetX, offsetY = _offsetY });
    public GameObject InstantiateSpanwedResources(int _x, int _y, float _offsetX, float _offsetY) 
    {
        if (spawnedResources.ContainsKey((_x, _y)))
            return Instantiate(spawnedResources[(_x, _y)].prefab, new Vector3(_x + _offsetX, _y + _offsetY), Quaternion.identity, ItemManager.instance.itemTransform);
        else
            return null;
    }
    #endregion
}
