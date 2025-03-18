using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] protected GameObject self;
    [SerializeField] public int maxAnimStage;

    #region Components
    public ResourceStats stats { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public Collider2D coll { get; private set; }
    public Animator anim { get; private set; }
    #endregion

    #region Init Info
    [HideInInspector] public int x;
    [HideInInspector] public int y;
    [HideInInspector] public int initStage;
    [HideInInspector] public int currentStage;
    #endregion

    private float destroyTime = 5f;
    private float destroyTimer;

    //private float activateTime = 5f;
    //private float activateTimer;

    //currentId������¼��ԴĿǰ״̬�����䶯�� ���Զ��ʱ��Դsprite������
    //���Զ��ʱ���InitId == CurrentId��ʱ����ʼ��ʱ ��ʱ��������gameObject�ͷ���Դ
    //���������������������� ֻ�ж����Զ��������sprite������sprite��Ⱦ

    protected virtual void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        stats = GetComponent<ResourceStats>();
        coll = GetComponentInChildren<Collider2D>();
    }
    protected virtual void Start()
    {
        anim.SetInteger("Stage", initStage);
        currentStage = initStage;
    }

    protected virtual void Update()
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, PlayerManager.instance.playerTransform.position)) >= MapGenerator.instance.radius - 2.5f)
        {
            DeactivateResource();
            CalculateDestroyTime();
        }
        else
        {
            ActivateResource();
            ResetDestroyTime();
        }
    }

    protected virtual void OnDestroy()
    {
        
    }

    private void DeactivateResource()
    {
        sr.enabled = false;
        anim.enabled = false;
        coll.enabled = false;
    }

    private void ActivateResource()
    {
        sr.enabled = true;
        anim.enabled = true;
        coll.enabled = true;
        anim.SetInteger("Stage", currentStage);
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
}
