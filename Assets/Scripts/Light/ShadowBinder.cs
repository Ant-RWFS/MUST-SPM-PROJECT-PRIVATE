using UnityEngine;

public class ShadowBinder : MonoBehaviour
{
    public Resource resource { get; private set; }
    public Animator anim { get; private set; }
    private void Awake()
    {
        resource = GetComponentInParent<Resource>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        anim.SetInteger("Stage", resource.initStage);
    }

    private void Update()
    {
    }
}
