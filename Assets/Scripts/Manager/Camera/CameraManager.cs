using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public Transform cameraParent;
    public float cameraAngle;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
        {
            instance = this;

            cameraParent = GameObject.FindGameObjectWithTag("MainCamera").transform.parent.transform;
        }
    }

    private void Update()
    {
        cameraAngle = cameraParent.rotation.eulerAngles.z;
    }
}
