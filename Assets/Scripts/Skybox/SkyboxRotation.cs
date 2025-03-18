using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    [SerializeField] private Material skyboxMaterial;

    [SerializeField] private SkyRotation sunLight;

    private float exposure;
    void Start()
    {
        exposure = skyboxMaterial.GetFloat("_Exposure");    
    }

    void Update()
    {
        RoatateSkybox();
    }

    private void RoatateSkybox() 
    {
        skyboxMaterial.SetFloat("_MainRotation", Time.time % 360);

        //exposure 

        //skyboxMaterial.SetFloat("_Exposure",)
    }
}
