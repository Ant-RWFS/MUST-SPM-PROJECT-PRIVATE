using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Bullet : MonoBehaviour
{
    #region Components
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    #endregion

    [SerializeField] private GameObject bulletGO;
    [SerializeField] private float speed;
    [SerializeField] private float flightTime;
    //[SerializeField] private int bulletDamage;

    private float flightTimer=0;
    private Vector3 flightDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        flightDir = new Vector3(Mathf.Cos((sr.transform.eulerAngles.z + CameraManager.instance.cameraAngle) * Mathf.Deg2Rad), Mathf.Sin((sr.transform.eulerAngles.z + CameraManager.instance.cameraAngle) * Mathf.Deg2Rad)).normalized;
    }

    private void Update()
    {
        BulletFlight();   
    }

    private void OnDestroy()
    {
        
    }

    private void BulletFlight()
    {
        flightTimer += Time.deltaTime;//增值计算避免倒计时直接删除物体

        rb.velocity = flightDir * (speed + PlayerManager.instance.player.rb.velocity.magnitude);

        if (flightTimer >= flightTime)
            Destroy(bulletGO);
    }
}

