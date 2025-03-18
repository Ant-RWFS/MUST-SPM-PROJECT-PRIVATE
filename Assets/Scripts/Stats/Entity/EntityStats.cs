using UnityEngine;

public class EntityStats : MonoBehaviour
{
    #region Info

    [Header("Stats Info")] 
    public Stat maxHealth;
    public int currentHealth;
    public Stat damage;

    [Header("Velocity Info")]
    public Stat rollSpeed;
    public Stat moveSpeed;
    public Stat runSpeed;
    public Vector2 currentVector;

    [Header("Knock Info")]
    public bool isKnocked;
    public float knockDuration;
    public Vector3 knockVector;

    [Header("Survive Info")]
    public bool isDead;

    [Header("Other Info")]
    public bool isBusy;
    public bool isInvisible;
    #endregion

    protected virtual void Awake()
    {
        isDead = false;
    }

    protected virtual void Start()
    {
        currentHealth = GetMaxHealthValue();
    }

    protected virtual void Update()
    {
        EntityDie();
    }

    private void EntityDie() 
    {
        if (currentHealth < 0)
            isDead = true;
    }
    
    public int GetMaxHealthValue() => maxHealth.GetValue();
}
