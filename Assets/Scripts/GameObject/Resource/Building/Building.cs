using UnityEngine;

public class Building : MonoBehaviour
{
    public SpriteRenderer sr { get; private set; }
    public Collider2D coll { get; private set; }

    protected virtual void Awake() 
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        coll = GetComponentInChildren<Collider2D>();
    }

    protected virtual void Update() 
    {
        HandleVisualRange();
    }

    private void HandleVisualRange()
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, PlayerManager.instance.playerTransform.position)) >= MapGenerator.instance.radius - 2.5f)
            BuildingOutRange();
        else
            BuildingInRange();
    }

    private void BuildingOutRange() 
    {
        sr.enabled = false;
        coll.enabled = false;
    }

    private void BuildingInRange() 
    {
        sr.enabled = true;
        coll.enabled = true;
    }
}
