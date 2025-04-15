using System.Collections.Generic;
using UnityEngine;

public class MapItem : MonoBehaviour
{
    private bool edgeItem;

    [SerializeField] protected GameObject self;

    #region Components
    public List<SpriteRenderer> sr;
    public List<Collider2D> coll;
    #endregion

    #region Init Info
    [HideInInspector] public int x;
    [HideInInspector] public int y;
    [HideInInspector] public int initStage;
    [HideInInspector] public int currentStage;
    #endregion

    private float destroyTime = 5f;
    private float destroyTimer;

    protected virtual void Awake()
    {
        edgeItem = false;
    }

    protected virtual void Start()
    {
        currentStage = initStage;
    }

    protected virtual void Update()
    {
        AvoidSpawnedInWater();

        if (Mathf.Abs(Vector3.Distance(transform.position, PlayerManager.instance.playerTransform.position)) >= MapGenerator.instance.radius - 2.5f)
        {
            DeactivateItem();
            CalculateDestroyTime();
        }
        else
        {
            ActivateItem();
            ResetDestroyTime();
        }
    }

    protected virtual void OnDestroy()
    {
        if (edgeItem)
            MapGenerator.instance.ItemAdjust(x, y);
        else
            MapGenerator.instance.ItemRegenerate(x, y);
    }

    private void DeactivateItem() 
    {
        foreach (var s in sr)
            s.enabled = false;

        foreach (var c in coll) 
            c.enabled = false;
    }

    private void ActivateItem() 
    {
        foreach (var s in sr)
            s.enabled = true;

        foreach (var c in coll)
            c.enabled = true;     
    }

    private void CalculateDestroyTime()
    {
        if (currentStage == initStage)
            destroyTimer += Time.deltaTime;

        if (destroyTimer >= destroyTime)
            Destroy(self);
    }

    private void ResetDestroyTime()
    {
        if (destroyTimer != 0)
            destroyTimer = 0;
    }

    private void AvoidSpawnedInWater()
    {
        if (Physics2D.OverlapCircle(transform.position, .5f, LayerMask.GetMask("Water")))
        {
            edgeItem = true;
            Destroy(self);
        }
    }
}
