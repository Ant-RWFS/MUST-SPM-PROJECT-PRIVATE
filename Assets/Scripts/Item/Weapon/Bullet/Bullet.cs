using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Components
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private CircleCollider2D coll;
    #endregion

    [SerializeField] private GameObject bulletGO;
    [SerializeField] private float speed;
    [SerializeField] private float flightTime;
    [SerializeField] private int bulletDamage;

    private float flightTimer = 0;
    private Vector3 flightDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        coll = GetComponentInChildren<CircleCollider2D>();
    }

    private void Start()
    {
        flightDir = new Vector3(Mathf.Cos((sr.transform.eulerAngles.z + CameraManager.instance.cameraAngle) * Mathf.Deg2Rad), Mathf.Sin((sr.transform.eulerAngles.z + CameraManager.instance.cameraAngle) * Mathf.Deg2Rad)).normalized;
    }

    private void Update()
    {
        BulletInRange();
        BulletFlight();
        BulletCollision();
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

    private void BulletInRange()
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, PlayerManager.instance.playerTransform.position)) >= MapGenerator.instance.radius - 2.5f)
            sr.enabled = false;
        else
            sr.enabled = true;
    }

    private void BulletCollision()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, coll.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<EnemyInterface>() != null)
            {
                var enemy = hit.GetComponent<EnemyInterface>();
                if (!enemy.statsI.isInvisible)
                {
                    enemy.Damage(bulletDamage);
                    Destroy(bulletGO);
                }
            }

            else if (hit.GetComponent<Motor>())
            {
                var motor = hit.GetComponent<Motor>();
                if (!motor.stats.isInvisible)
                {
                    //Damage
                    Destroy(bulletGO);
                }
            }

            else if (hit.GetComponentInParent<Resource>())
            {
                var resource = hit.GetComponentInParent<Resource>();
                if (!resource.stats.isInvisible)
                {
                    resource.Damage(bulletDamage);
                    Destroy(bulletGO);
                }
            }

            else if (hit.GetComponentInParent<Building>())
            {
                var building = hit.GetComponentInParent<Building>();
                //Damage
                Destroy(bulletGO);
            }

            else if (hit.GetComponentInParent<MapItem>()) 
            {
                var mapItem = hit.GetComponentInParent<MapItem>();
                //Damage
                Destroy(bulletGO);
            }
        }
    }
}

