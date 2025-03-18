using UnityEngine;

public class MotorStats : EntityStats
{
    private Motor motor;
    protected override void Awake()
    {
        base.Awake();

        motor = GetComponent<Motor>();
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
