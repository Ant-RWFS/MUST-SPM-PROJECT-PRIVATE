using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    public Transform itemTransform;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
        {
            instance = this;
            itemTransform = GameObject.FindGameObjectWithTag("Item").transform;
        }
    }
}
