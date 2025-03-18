using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Bag bag;

    private SpriteRenderer sr;
    private Animator anim;

    private Vector2 gunVector;
    private Vector2 bulletVector;

    private float attackTimer;
    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
    }

    private void Update()
    {
        PlayerArmed();
        FollowAimPoint();
        WeaponAttack();
    }

    private void PlayerArmed()
    {
        if (bag.weapon)
        {
            if (bag.weapon is Gun gun)
            { 
                transform.localPosition = bag.player.transform.position;
                anim.runtimeAnimatorController = gun.ac;
            }
        }
    }

    private void FollowAimPoint()
    {
        gunVector = new Vector2(
            Input.mousePosition.x - Screen.width / 2, 
            Input.mousePosition.y - Screen.height / 2
        ).normalized;

        bulletVector = new Vector2(
            gunVector.x * Mathf.Cos(CameraManager.instance.cameraAngle * Mathf.Deg2Rad) - gunVector.y * Mathf.Sin(CameraManager.instance.cameraAngle * Mathf.Deg2Rad),
            gunVector.x * Mathf.Sin(CameraManager.instance.cameraAngle * Mathf.Deg2Rad) + gunVector.y * Mathf.Cos(CameraManager.instance.cameraAngle * Mathf.Deg2Rad)
        ).normalized;

        if (gunVector.magnitude > 0)
        {
            float targetAngle = Mathf.Atan2(gunVector.y, gunVector.x) * Mathf.Rad2Deg;
            float currentAngle = Mathf.MoveTowardsAngle(sr.transform.eulerAngles.z, targetAngle + CameraManager.instance.cameraAngle, 500 * Time.deltaTime);

            sr.transform.eulerAngles = new Vector3(sr.transform.eulerAngles.x, sr.transform.eulerAngles.y, currentAngle);
        }

        SetGunPosition();
        SetGunAnim();
    }

    private void SetGunPosition()
    {
        if (Mathf.Abs(gunVector.x) < Mathf.Abs(gunVector.y))
        {
            if (gunVector.y >= 0)
                sr.sortingOrder = -1;
            else
                sr.sortingOrder = 0;

            sr.transform.localPosition = new Vector3(.05f * sr.sortingOrder, .2f, 0);
        }
        else 
        {
            sr.sortingOrder = 0;
            
            if (gunVector.x >= 0)
                sr.transform.localPosition = new Vector3(-.05f, .2f, 0);
            else
                sr.transform.localPosition = new Vector3(.05f, .2f, 0);
        }
    }

    private void SetGunAnim() 
    {
        if (gunVector.x >= 0)
            anim.SetBool("right", true);
        else
            anim.SetBool("right", false);
    }

    private void WeaponAttack() 
    {
        if (bag.weapon)
        {
            if (bag.weapon is Gun gun)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    attackTimer -= Time.deltaTime;

                    if (attackTimer < 0)
                    {
                        attackTimer = 1 / gun.firingRatePerSec;
                        GameObject bullet = Instantiate(gun.bullet, PlayerManager.instance.playerTransform.position + new Vector3(bulletVector.x * .25f, bulletVector.y * .25f, -.25f), Quaternion.identity, ItemManager.instance.itemTransform);//z轴高度生成后可能需要调整碰撞体的判断高度
                        bullet.GetComponentInChildren<SpriteRenderer>().transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(gunVector.y, gunVector.x) * Mathf.Rad2Deg);
                    }
                }
            }
        }
    }
}
