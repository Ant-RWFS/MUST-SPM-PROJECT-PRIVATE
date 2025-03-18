using UnityEngine;

public class SkyRotation : MonoBehaviour
{
    [Header("Time Info")]
    [SerializeField] private float realTimeQuotient;

    [Header("Skybox Info")]
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private float exposureValue;

    [Header("Morning Info")]
    [Range(-1f, 1f)]
    [SerializeField] private float amStart;
    [Range(-1f, 1f)]
    [SerializeField] private float amEnd;

    [Header("Afternoom Info")]
    [Range(-1f, 1f)]
    [SerializeField] private float pmStart;
    [Range(-1f, 1f)]
    [SerializeField] private float pmEnd;

    private float exposureRate;
    private float angle;
    private float rotation;

    private void Awake()
    {
        SkyboxRotate();
    }
    private void Start()
    {
    }
    
    private void Update()
    {
        SunLightRotate();
        SkyboxRotate();
    }

    private void SunLightRotate() 
    {
        angle = (Time.deltaTime * realTimeQuotient) % 180;
        transform.Rotate(angle, 0, 0, Space.Self);
    }

    private void SkyboxRotate()
    {
        if (transform.rotation.x >= amStart && transform.rotation.x < amEnd) // 90 degree : .5f; 180 :1f
            exposureRate = ((transform.rotation.x - amStart) / (amEnd - amStart)) * exposureValue;

        else if (transform.rotation.x >= pmStart && transform.rotation.x < pmEnd)
            exposureRate = exposureValue - (transform.rotation.x / (pmEnd - pmStart)) * exposureValue;

        skyboxMaterial.SetFloat("_MainRotation", Time.time % 180);
        skyboxMaterial.SetFloat("_Exposure", exposureRate + 1);
    }
}
