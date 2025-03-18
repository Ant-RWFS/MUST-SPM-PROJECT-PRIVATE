using System.Collections;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float rotationTime = .2f;
    private bool isRotating = false;
    void Start()
    {
    
    }

    void Update()
    {
        transform.position = PlayerManager.instance.playerTransform.position;

        RotateTrigger();
    }

    void RotateTrigger()
    {
        if (!isRotating)
        {
            if (Input.GetKeyDown(KeyCode.Q)) 
                StartCoroutine(Rotate(-45, rotationTime));
            
            if (Input.GetKeyDown(KeyCode.E))
                StartCoroutine(Rotate(45, rotationTime));
        }
    }

    IEnumerator Rotate(float _angle, float _time)
    {
        float number = 60 * _time;
        float nextAngle = _angle / number;

        isRotating = true;

        for (int i = 0; i < number; i++)
        {
            transform.Rotate(new Vector3(0, 0, nextAngle));

            yield return new WaitForFixedUpdate();
        }

        isRotating = false;
    }
}

