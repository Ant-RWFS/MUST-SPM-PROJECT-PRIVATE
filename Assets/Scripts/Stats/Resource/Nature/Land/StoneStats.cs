using UnityEngine;

public class StoneStats : ResourceStats
{
    private Stone stone;
    protected override void Awake()
    {
        base.Awake();

        stone = GetComponent<Stone>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
