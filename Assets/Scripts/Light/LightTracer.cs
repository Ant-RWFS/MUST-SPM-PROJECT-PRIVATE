using UnityEngine;

public class LightTracer : MonoBehaviour
{
    public Vector3 offset;
    public Transform target;

    private void Awake()
    {
       
    }

    private void Start()
    {
        
    }

    private void Update() 
    {
        transform.position = target.position + new Vector3(offset.x, offset.y, offset.z);
    }
}
