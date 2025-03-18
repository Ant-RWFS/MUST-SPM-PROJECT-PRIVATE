using UnityEngine;

[CreateAssetMenu(fileName = "New Wood", menuName = "Inventory/Item/Wood")]
public class Wood : Item
{
    public override void Use()
    {
        Debug.Log($"检查 {itemName}，这是一块普通的木头，暂无特殊效果");
        // 可以添加未来用途，例如合成
    }
}