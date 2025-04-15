using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.Rendering;

public struct TileData
{
    public int x;
    public int y;
    public float offsetX;
    public float offsetY;
    public bool tileType; // False: Land; True: Water
    public int resourceType;
    public int resourceInitID;
    public bool resourceInstantiatable;
}

[System.Serializable]
public struct Area
{
    [Range(0, 1f)]
    public float ratio;
    [Range(0, 1f)]
    public float density;
    [Range(0, 1f)]
    public float lacunarity;
}

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance;
    public Dictionary<(int, int), TileData> mapData;

    [Header("Tilemap")]
    public Tilemap landTilemap;
    public Tilemap waterTilemap;

    public TileBase landTile;
    public TileBase waterTile;

    public int radius;
    public int seed;
    public bool randomSeed;

    public Area map;
    public Area item;

    [Header("Detector")]
    [SerializeField] private Transform mapDetector;

    private Transform playerTransform;
    private ItemGenerator itemGenerator;

    private float randomOffset;

    private bool isPlayerDetected;

    private void Awake()
    {
        itemGenerator = GetComponent<ItemGenerator>();

        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        InitMap();
    }

    private void Update()
    {
        UpdateMap();
    }
  
    #region Initialization
    private void InitMap()
    {
        InitPlayerTransform();
        InitMapData();
        UpdateTilemapData();
        itemGenerator.CleanSpanwedObjects();
        UpdateTilemapData();//Reupdate the mapdata to ensure the instantiation of objects
    }

    private void InitPlayerTransform() => playerTransform = PlayerManager.instance.playerTransform;

    private void InitMapData()
    {
        if (!randomSeed)
            seed = Time.time.GetHashCode();

        UnityEngine.Random.InitState(seed);

        mapData = new Dictionary<(int, int), TileData>();

        isPlayerDetected = false;

        randomOffset = UnityEngine.Random.Range(-100000, 100000);
    }
    #endregion

    #region Update
    private void UpdateMap()
    {
        Collider2D[] detector = Physics2D.OverlapBoxAll(mapDetector.position, new Vector2(.25f, .25f), 0);

        isPlayerDetected = false;

        foreach (var hit in detector)
        {
            if (hit.GetComponent<Player>())
                isPlayerDetected = true;
        }

        if (!isPlayerDetected)
        {
            itemGenerator.CleanSpanwedObjects();
            UpdateTilemapData();

            mapDetector.position = new Vector3Int((int)playerTransform.position.x, (int)playerTransform.position.y);
        }
    }

    #region Hash
    private uint MurmurHash(int x, int y, int seed)
    {
        uint hash = (uint)seed;
        uint key = (uint)(x ^ y);

        hash ^= key;
        hash ^= hash >> 14;
        hash ^= hash << 7;
        hash ^= hash >> 19;

        return hash;
    }

    private float HashToOffset(int x, int y, int seed, float range)
    {
        uint hashValue = MurmurHash(x, y, seed);
        return (hashValue % 1000) / 1000.0f * 2 * range - range;
    }
    #endregion

    private void UpdateTilemapData()
    {
        Dictionary<(int, int), TileData> updatedMapData = new Dictionary<(int, int), TileData>();

        int playerX = (int)playerTransform.position.x;
        int playerY = (int)playerTransform.position.y;

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if ((x * x + y * y) <= (radius * radius))
                {
                    int objectType, objectInitID;
                    bool tileType, objectInstantiated;
                    float offsetX, offsetY, mapNoise, itemNoise, objectTypeNoise;

                    mapNoise = Mathf.PerlinNoise((playerX + x) * map.lacunarity + randomOffset, (playerY + y) * map.lacunarity + randomOffset);
                    tileType = mapNoise < map.ratio;

                    itemNoise = Mathf.PerlinNoise((playerX + x) * item.lacunarity + randomOffset + 10000, (playerY + y) * item.lacunarity + randomOffset + 10000);
              

                    if (!tileType && itemNoise < map.density)
                    {
                        offsetX = HashToOffset(playerX + x, playerY + y, seed, 0.4f);
                        offsetY = HashToOffset(playerX + x, playerY + y, seed, 0.4f);

                        objectTypeNoise = Mathf.PerlinNoise((playerX + x) * map.lacunarity + randomOffset + 20000, (playerY + y) * map.lacunarity + randomOffset + 20000);

                        if (objectTypeNoise < 0.5f)
                            objectType = 0;
                        else
                            objectType = 1;

                        objectInitID = Mathf.Abs((int)(MurmurHash(playerX + x, playerY + y, seed + objectType) % itemGenerator.resources[objectType].maxStage));
                        objectInstantiated = IsObjectInstantiated(playerX + x, playerY + y);

                        itemGenerator.UpdateSpawnedResources(objectType, playerX + x, playerY + y, offsetX, offsetY);
                    }

                    else if (!tileType && mapNoise < item.ratio && itemNoise > 1 - item.density)
                    {
                        offsetX = HashToOffset(playerX + x, playerY + y, seed, 0.6f);
                        offsetY = HashToOffset(playerX + x, playerY + y, seed, 0.6f);
                        objectTypeNoise = Mathf.PerlinNoise((playerX + x) * item.lacunarity + randomOffset + 20000, (playerY + y) * item.lacunarity + randomOffset + 20000);
                        objectType = -1;
                        objectInitID = Mathf.Abs((int)(MurmurHash(playerX + x, playerY + y, seed) % itemGenerator.items.Count));
                        objectInstantiated = IsObjectInstantiated(playerX + x, playerY + y);
                        itemGenerator.UpdateSpawnedItems(objectInitID, playerX + x, playerY + y, offsetX, offsetY);
                    }

                    else
                    {
                        offsetX = 0;
                        offsetY = 0;
                        objectType = -1;
                        objectInitID = 0;
                        objectInstantiated = false;
                    }

                    var tileData = new TileData
                    {
                        x = playerX + x,
                        y = playerY + y,
                        offsetX = offsetX,
                        offsetY = offsetY,
                        tileType = tileType,
                        resourceType = objectType,
                        resourceInstantiatable = objectInstantiated,
                        resourceInitID = objectInitID
                    };

                    updatedMapData[(tileData.x, tileData.y)] = tileData;
                }
            }
        }

        UpdateItemData();
        UpdateMapData(updatedMapData);

        foreach (var tileData in updatedMapData.Values)
        {
            if (!tileData.tileType)
                landTilemap.SetTile(new Vector3Int(tileData.x, tileData.y), landTile);
            else
                waterTilemap.SetTile(new Vector3Int(tileData.x, tileData.y), waterTile);
        }
    }

    private void UpdateMapData(Dictionary<(int, int), TileData> _addDict)
    {
        foreach (var kvp in _addDict)
            mapData[kvp.Key] = kvp.Value;
    }

    private void UpdateItemData()
    {
        Dictionary<(int, int), TileData> updatedMapData = new Dictionary<(int, int), TileData>();

        foreach (var kvp in mapData)
        {
            var tileData = kvp.Value;

            if (tileData.resourceInstantiatable)
            {
                if (tileData.resourceType != -1)
                {
                    GameObject spawnedResource = itemGenerator.InstantiateSpanwedResources(tileData.x, tileData.y, tileData.offsetX, tileData.offsetY);

                    if (spawnedResource)
                    {
                        Resource resource = spawnedResource.GetComponentInChildren<Resource>();
                        resource.initStage = tileData.resourceInitID;
                        resource.x = tileData.x;
                        resource.y = tileData.y;
                    }
                }
                else
                {
                    GameObject spawnedItem = itemGenerator.InstantiateSpanwedItems(tileData.x, tileData.y, tileData.offsetX, tileData.offsetY);

                    if (spawnedItem)
                    {
                        MapItem item =spawnedItem.GetComponentInChildren<MapItem>();
                        item.initStage = 1;
                        item.x = tileData.x;
                        item.y = tileData.y;
                    }
                }
            }

            updatedMapData[(tileData.x, tileData.y)] = new TileData
            {
                x = tileData.x,
                y = tileData.y,
                resourceType = tileData.resourceType,
                resourceInstantiatable = false,
                resourceInitID = tileData.resourceInitID
            };
        }

        mapData = updatedMapData;
    }

    private bool IsObjectInstantiated(int _x, int _y) => !mapData.ContainsKey((_x, _y));
    public void ItemAdjust(int _x, int _y)
    {
        if (mapData.ContainsKey((_x, _y)))
        {
            TileData tileData = mapData[((_x, _y))];
            tileData.resourceType = -1;
        }
    }
    public void ItemRegenerate(int _x, int _y)
    {
        if (mapData.ContainsKey((_x, _y)))
            mapData.Remove((_x, _y));
    }
    #endregion
}