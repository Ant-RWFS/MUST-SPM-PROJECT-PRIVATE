using UnityEngine;

public class PlayerManager : MonoBehaviour 
{ 
    public static PlayerManager instance;
    public Transform playerTransform;
    public Player player;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
        {
            instance = this;

            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            player = playerTransform.GetComponent<Player>();
        }
    }
}