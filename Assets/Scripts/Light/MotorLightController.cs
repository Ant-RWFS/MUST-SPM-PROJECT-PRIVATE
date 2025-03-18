using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorLightController : MonoBehaviour
{
    [SerializeField] private Motor motor;
    [SerializeField] private Light headLight;
    [SerializeField] private Light bodyLight;

    private Dictionary<(float, float), Vector3> headLightDirs;

    private float xInput;
    private float yInput;
    private bool lightOn;

    private void Awake()
    {
        headLightDirs = new Dictionary<(float, float), Vector3>
        {
            { (0, 0),   new Vector3(0, 90, 0) },
            { (1, 0),   new Vector3(0, 90, 0) },
            { (1, 1),   new Vector3(-30, 60, 0) },
            { (0, 1),   new Vector3(-45, 0, 0) },
            { (-1, 1),  new Vector3(-30, -60, 0) },
            { (-1, 0),  new Vector3(0, -90, 0) },
            { (-1, -1), new Vector3(15, -120, 0) },
            { (0, -1),  new Vector3(45, -180, 0) },
            { (1, -1),  new Vector3(15, 120, 0) }
        };

        xInput = 0;
        yInput = 0;
        lightOn = false;
    }

    private void Start()
    {

    }

    private void Update()
    {
        SwitchLight();
    }

    private void SwitchLight()
    {
        xInput = motor.anim.GetFloat("xInput");
        yInput = motor.anim.GetFloat("yInput");

        ManualLightSwitch();
        LightSwitchLogic();
    }

    private void ManualLightSwitch()
    {
        if (!motor.anim.GetBool("Off") && Input.GetKeyDown(KeyCode.F))
        {
            if (lightOn)
                lightOn = false;
            else
                lightOn = true;
        }
    }

    private void LightSwitchLogic()
    {
        if (!motor.anim.GetBool("Off") && lightOn)
        {
            LightsOn();
            SwitchLightDirection();
        }
        else
        {
            LightsOff();
            lightOn = false;
        }
    }

    private void LightsOn() 
    {
        headLight.enabled = true;
        bodyLight.enabled = true;
    }

    private void LightsOff() 
    {
       headLight.enabled = false;
       bodyLight.enabled = false;
    }
    private void SwitchLightDirection() => headLight.transform.localRotation = Quaternion.Euler(headLightDirs[(xInput, yInput)].x, headLightDirs[(xInput, yInput)].y, headLightDirs[(xInput, yInput)].z);
}
