using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Stone", menuName = "Inventory/Resource/Stone")]
public class Stones : Item
{
    public override void Use()
    {
        Debug.Log("this is a stone");
    }
}