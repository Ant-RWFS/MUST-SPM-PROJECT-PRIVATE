using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct SpawnedEnemy
{
    public GameObject prefab;
    [HideInInspector] public int x;
    [HideInInspector] public int y;
    [HideInInspector] public float offsetX;
    [HideInInspector] public float offsetY;
}

public class EnemyGenerator : MonoBehaviour
{
    public SpawnedEnemy dustEnemies;
    public Dictionary<(int, int), SpawnedEnemy> spawnedDustEnemies;
    private void Awake()
    {
        spawnedDustEnemies = new Dictionary<(int, int), SpawnedEnemy> ();
    }
    private void Start()
    {
    }

    private void Update()
    {

    }
    public void CleanSpawnedEnemies() 
    {
        spawnedDustEnemies.Clear ();
    }
    public void UpdateSpawnedDustEnemies(int _x, int _y, float _offsetX, float _offsetY) => spawnedDustEnemies.Add((_x, _y), new SpawnedEnemy { prefab = dustEnemies.prefab, x = _x, y = _y, offsetX = _offsetX, offsetY = _offsetY });
    public GameObject InstantiateSpanwedDustEnemies(int _x, int _y, float _offsetX, float _offsetY)
    {
        if (spawnedDustEnemies.ContainsKey((_x, _y)))
            return Instantiate(spawnedDustEnemies[(_x, _y)].prefab, new Vector3(_x + _offsetX, _y + _offsetY), Quaternion.identity, ItemManager.instance.itemTransform);
        else
            return null;
    }
}
