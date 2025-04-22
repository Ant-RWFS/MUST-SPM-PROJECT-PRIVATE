using UnityEngine;

public class Bag : MonoBehaviour
{
    public Player player;
    public Inventory inventory;
    public Item weapon;
    public Transform weaponTransform;
    private void Awake()
    {

    }
    private void Start()
    {

    }

    private void Update()
    {
        UpdateWeapon();
    }

    private void UpdateWeapon()
    {
        weapon = inventory.armedWeapon;

        if (weapon && !player.anim.GetBool("Ride") && !player.anim.GetBool("Die"))
        {
            player.stats.isArmed = true;
            weaponTransform.gameObject.SetActive(true);
        }
        else
        {
            player.stats.isArmed = false;
            weaponTransform.gameObject.SetActive(false);
        }
    }
}
