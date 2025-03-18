using System.Collections;
using UnityEngine;

public class SunLightRotation : MonoBehaviour
{
    [Header("Time Info")]
    [SerializeField] public float dayTime;
    [SerializeField] public float gapTime; 
    
    private float dayTimer;
    private float gapTimer;
    private float angle;
    
    private void Start()
    {
        dayTimer = 600 * dayTime;
        gapTimer = gapTime;
        angle = 360 / dayTimer;
    }
    
    private void Update()
    {
        SunLightRotate();
    }

    private void SunLightRotate() 
    {
        dayTimer -= Time.deltaTime;
        gapTimer -= Time.deltaTime;

        if (gapTimer < 0)
        {
            transform.Rotate(angle, 0, 0);
            gapTimer = gapTime;
        }

        if (dayTimer < 0) 
        {
            transform.Rotate(-360, 0, 0);
            dayTimer = dayTime;
        }
    }
}
