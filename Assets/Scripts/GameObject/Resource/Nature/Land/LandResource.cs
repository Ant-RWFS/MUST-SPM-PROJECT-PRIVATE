using UnityEngine;

public class LandResource : Resource
{
    private bool edgeItem;

    protected override void Awake() 
    {
        base.Awake();

        edgeItem = false;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (edgeItem)
            MapGenerator.instance.ItemAdjust(x, y);
        else
            MapGenerator.instance.ItemRegenerate(x, y);
    }

    protected override void Update()
    {
        base.Update();

        if (Physics2D.OverlapCircle(transform.position, .5f, LayerMask.GetMask("Water"))) 
        {
            edgeItem = true;
            Destroy(self);
        }
    }
}
