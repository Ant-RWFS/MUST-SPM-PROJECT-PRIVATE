using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

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

    [Range(0, 1f)]
    public float lacunarity;

    [Range(0, 1f)]
    public float waterRatio;

    [Range(0, 1f)]
    public float resourceDensity;

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
            itemGenerator.CleanSpanwedItems();
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
                    int resourceType, resourceInitID;
                    bool tileType, resourceInstantiated;
                    float offsetX, offsetY, mapNoiseValue, resourceNoise, resourceTypeNoise;

                    mapNoiseValue = Mathf.PerlinNoise((playerX + x) * lacunarity + randomOffset, (playerY + y) * lacunarity + randomOffset);
                    tileType = mapNoiseValue < waterRatio;

                    resourceNoise = Mathf.PerlinNoise((playerX + x) * lacunarity + randomOffset + 10000, (playerY + y) * lacunarity + randomOffset + 10000);

                    if (!tileType && resourceNoise < resourceDensity)
                    {
                        resourceTypeNoise = Mathf.PerlinNoise((playerX + x) * lacunarity + randomOffset + 20000, (playerY + y) * lacunarity + randomOffset + 20000);

                        offsetX = HashToOffset(playerX + x, playerY + y, seed, 0.4f);
                        offsetY = HashToOffset(playerX + x, playerY + y, seed, 0.4f);

                        if (resourceTypeNoise < 0.5f)
                            resourceType = 0;
                        else
                            resourceType = 1;

                        resourceInitID = Mathf.Abs((int)(MurmurHash(playerX + x, playerY + y, seed + resourceType) % itemGenerator.items[resourceType].maxStage));
                        resourceInstantiated = IsItemInstantiated(playerX + x, playerY + y);
                        itemGenerator.UpdateSpawnedItems(resourceType, playerX + x, playerY + y, offsetX, offsetY);
                    }
                    else
                    {
                        offsetX = 0;
                        offsetY = 0;
                        resourceType = -1;
                        resourceInitID = 0;
                        resourceInstantiated = false;
                    }

                    var tileData = new TileData
                    {
                        x = playerX + x,
                        y = playerY + y,
                        offsetX = offsetX,
                        offsetY = offsetY,
                        tileType = tileType,
                        resourceType = resourceType,
                        resourceInstantiatable = resourceInstantiated,
                        resourceInitID = resourceInitID
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

            if (tileData.resourceType != -1 && tileData.resourceInstantiatable)
            {
                GameObject item = itemGenerator.InstantiateSpanwedItems(tileData.x, tileData.y, tileData.offsetX, tileData.offsetY);

                if (item)
                {
                    Resource resource = item.GetComponentInChildren<Resource>();
                    resource.initStage = tileData.resourceInitID;
                    resource.x = tileData.x;
                    resource.y = tileData.y;
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

    private bool IsItemInstantiated(int _x, int _y) => !mapData.ContainsKey((_x, _y));
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