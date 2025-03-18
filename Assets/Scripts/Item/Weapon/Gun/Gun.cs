using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Inventory/Item")]
public class Gun : Item
{
    public AnimatorController ac;
    public float firingRatePerSec;
    public GameObject bullet;
    public bool isEquipped = false;
    public override void Use()
    {
        isEquipped = !isEquipped;
        Debug.Log($"{itemName} 已{(isEquipped ? "装备" : "卸下")}");
        // 可以添加装备到玩家的逻辑
    }
}