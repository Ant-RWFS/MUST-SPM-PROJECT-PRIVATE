using UnityEngine;

public class ResourceStats : MonoBehaviour
{
    #region Info
    [Header("Stats Info")]
    public Stat hp;
    public Stat consumption;//comsumption to tool

    [Header("Survive Info")]
    public bool isDead;

    [Header("Other Info")]
    public bool isInvisible;
    #endregion

    protected virtual void Awake()
    {
        isDead = false;
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        EntityDie();
    }

    private void EntityDie()
    {
        if (hp.GetValue() < 0)
            isDead = true;
    }
}
