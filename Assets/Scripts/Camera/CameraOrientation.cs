using UnityEngine;

public class CameraOrientation: MonoBehaviour
{
    Transform[] entities;

    void Start()
    {
        entities = new Transform[transform.childCount];
        
        for(int i=0; i<transform.childCount; i++)
            entities[i]= transform.GetChild(i);
    }

    void Update()
    {
        for(int i=0;i<entities.Length;i++)
            entities[i].rotation = Camera.main.transform.rotation;
    }
}
