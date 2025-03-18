using UnityEngine;

public class MotorManager : MonoBehaviour
{
    public static MotorManager instance;
    public Transform motorTransform;
    public Motor motor;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
        {
            instance = this;

            motorTransform = GameObject.FindGameObjectWithTag("Motor").transform;
            motor = motorTransform.GetComponent<Motor>();
        }
    }
}
