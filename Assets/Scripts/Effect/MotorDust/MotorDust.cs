using UnityEngine;

public class MotorDust : MonoBehaviour
{
    private Motor motor;
    private ParticleSystem dust;
    private ParticleSystem.EmissionModule dustEmission;
    private ParticleSystem.ForceOverLifetimeModule dustDir;

    #region Modifier
    [SerializeField] private float maxEmission;
    [SerializeField] private float minEmission;
    [SerializeField] private float variableEmission;
    [SerializeField] private float dustSpeed;
    #endregion

    private Vector2 input;
    private float xInput;
    private float yInput;

    private float accumulateTimer;
    private float decayTimer;
    private float vanishTimer;
    private void Awake()
    {
        dust = GetComponent<ParticleSystem>();
        dustEmission = dust.emission;
        dustDir = dust.forceOverLifetime;

        accumulateTimer = 0;
        decayTimer = 0;
    }

    private void Start()
    {
        motor = MotorManager.instance.motor;
    }

    private void Update()
    {
        FollowMotor();
        MotorRaiseDust();
    }

    private void FollowMotor() => transform.position = MotorManager.instance.motorTransform.position;

    private void MotorRaiseDust() 
    {
        if (motor.anim.GetBool("Move"))
        {
            xInput = Input.GetAxisRaw("Horizontal");
            yInput = Input.GetAxisRaw("Vertical");

            input = (MotorManager.instance.motorTransform.right * xInput + MotorManager.instance.motorTransform.up * yInput).normalized;

            SetDustDir(-input.x * dustSpeed, -input.y * dustSpeed);
            DustAccumulate();
        }
        else if (motor.anim.GetBool("Idle"))
        {
            SetDustDir(0, 0);
            DustDecay();
        }
        else 
        {
            SetDustDir(0, 0);
            DustVanish();
        }
    }

    private void DustAccumulate() 
    {
        if (dustEmission.rateOverTime.constant < maxEmission)
        {
            accumulateTimer += Time.deltaTime;

            if (accumulateTimer >= 1f)
            {
                dustEmission.rateOverTime = dustEmission.rateOverTime.constant + variableEmission;
                accumulateTimer = 0;
            }
        }
        else 
        {
            if (dustEmission.rateOverTime.constant > maxEmission)
                dustEmission.rateOverTime = maxEmission;

            accumulateTimer = 0;
        }
    }

    private void DustDecay() 
    {
        if (dustEmission.rateOverTime.constant > minEmission)
        {
            if (dustEmission.rateOverTime.constant >= 0)
            {
                decayTimer += Time.deltaTime;

                if (decayTimer >= 1f)
                {
                    dustEmission.rateOverTime = dustEmission.rateOverTime.constant - variableEmission;
                    decayTimer = 0;
                }
            }
            else
                dustEmission.rateOverTime = 0;
        }
        else 
        {
            if (dustEmission.rateOverTime.constant <minEmission)
                dustEmission.rateOverTime = minEmission;

            decayTimer = 0;
        }
    }

    private void SetDustDir(float _x, float _y) 
    {
        dustDir.x = _x;
        dustDir.y = _y;
    } 
    private void DustVanish() => dustEmission.rateOverTime = 0;
}
